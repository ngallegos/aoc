const $ = (selector, root = document) => root.querySelector(selector);

function element(tag, properties = {}, ...children) {
  const node = Object.assign(document.createElement(tag), properties);
  node.append(...children);
  return node;
}

// Runs inside the worker, so it must not reference anything outside its own body.
function runTests(code, tests) {
  const results = [];
  const stable = value => JSON.stringify(value, (key, item) =>
    item && item.constructor === Object ? Object.fromEntries(Object.entries(item).sort()) : item);
  const check = (name, actual, expected) => results.push({
    name,
    passed: stable(actual) === stable(expected),
    detail: `expected ${stable(expected)}, got ${stable(actual)}`,
  });
  try {
    new Function("check", `${code}\n;${tests}`)(check);
  } catch (error) {
    results.push({ name: "Code threw", passed: false, detail: String(error) });
  }
  return results;
}

// A worker lets an accidental infinite loop be stopped without freezing the page.
function runInWorker(code, tests, timeout = 3000) {
  const source = `onmessage = e => postMessage((${runTests})(e.data.code, e.data.tests));`;
  const url = URL.createObjectURL(new Blob([source], { type: "text/javascript" }));
  const worker = new Worker(url);
  return new Promise(resolve => {
    const finish = results => {
      clearTimeout(timer);
      worker.terminate();
      URL.revokeObjectURL(url);
      resolve(results);
    };
    const timer = setTimeout(() => finish([{ name: "Timed out", passed: false,
      detail: "Check for an infinite loop, or work that grows too fast." }]), timeout);
    worker.onmessage = event => finish(event.data);
    worker.onerror = event => finish([{ name: "Could not run", passed: false, detail: event.message }]);
    worker.postMessage({ code, tests });
  });
}

function renderExercises(container, exercises) {
  exercises.forEach(({ title, description, starter, tests, solution }, index) => {
    const editor = element("textarea", { value: starter, spellcheck: false, ariaLabel: title,
      rows: Math.max(starter.split("\n").length, solution.split("\n").length) + 1 });
    const run = element("button", {}, "Run tests");
    const reset = element("button", {}, "Reset");
    const results = element("ul", { className: "results", role: "status" });
    run.onclick = async () => {
      run.disabled = true;
      const outcomes = await runInWorker(editor.value, tests);
      results.replaceChildren(...outcomes.map(({ name, passed, detail }) =>
        element("li", { className: passed ? "correct" : "wrong" },
          passed ? `✓ ${name}` : `✗ ${name}: ${detail}`)));
      run.disabled = false;
    };
    reset.onclick = () => { editor.value = starter; results.replaceChildren(); };
    const prompt = element("p");
    prompt.innerHTML = description;
    container.append(element("div", { className: "exercise" },
      element("h3", {}, `${index + 1}. ${title}`), prompt, editor,
      element("div", { className: "row" }, run, reset), results,
      element("details", {}, element("summary", {}, "Show a solution"), element("pre", {}, solution))));
  });
}

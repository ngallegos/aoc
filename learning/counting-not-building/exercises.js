const exercises = [
  {
    title: "Build it the direct way",
    description: "Start with the obvious solution. On each step, replace every symbol in the string using <code>rules</code>, all at once. You will use this idea as the baseline to improve on.",
    starter: `function expand(rules, start, steps) {
  let text = start;
  // TODO: apply the rules to every symbol, "steps" times
  return text;
}`,
    solution: `function expand(rules, start, steps) {
  let text = start;
  for (let step = 0; step < steps; step++) {
    text = Array.from(text, symbol => rules[symbol]).join("");
  }
  return text;
}`,
    tests: `const rules = { A: "B", B: "AB" };
check("no steps", expand(rules, "A", 0), "A");
check("one step", expand(rules, "A", 1), "B");
check("four steps", expand(rules, "A", 4), "ABBAB");`,
  },
  {
    title: "Throw away the order",
    description: "If only the length matters, the order of symbols doesn't. Return an object that records how many times each symbol appears.",
    starter: `function countSymbols(text) {
  const counts = {};
  // TODO: counts[symbol] = occurrences
  return counts;
}`,
    solution: `function countSymbols(text) {
  const counts = {};
  for (const symbol of text) {
    counts[symbol] = (counts[symbol] ?? 0) + 1;
  }
  return counts;
}`,
    tests: `check("mixed", countSymbols("ABBAB"), { A: 2, B: 3 });
check("empty", countSymbols(""), {});`,
  },
  {
    title: "Step the counts",
    description: "Advance one step without building a string. If there are 3 of a symbol, then everything that symbol turns into appears 3 more times. Several symbols can produce the same thing, so add to what is already there.",
    starter: `function step(counts, rules) {
  const next = {};
  for (const [symbol, count] of Object.entries(counts)) {
    // TODO: add "count" to every symbol produced by rules[symbol]
  }
  return next;
}`,
    solution: `function step(counts, rules) {
  const next = {};
  for (const [symbol, count] of Object.entries(counts)) {
    for (const product of rules[symbol]) {
      next[product] = (next[product] ?? 0) + count;
    }
  }
  return next;
}`,
    tests: `const rules = { A: "B", B: "AB" };
check("one symbol", step({ A: 1 }, rules), { B: 1 });
check("many copies", step({ A: 2, B: 3 }, rules), { B: 5, A: 3 });
check("repeated product", step({ C: 4 }, { C: "CC" }), { C: 8 });`,
  },
  {
    title: "Go far",
    description: "Put the pieces together and return the length after many steps. At 60 steps the string would be trillions of characters long, so only counting will finish in time.",
    starter: `function lengthAfter(rules, start, steps) {
  // TODO: count the starting symbols, step the counts, then total them
}`,
    solution: `function lengthAfter(rules, start, steps) {
  let counts = {};
  for (const symbol of start) counts[symbol] = (counts[symbol] ?? 0) + 1;
  for (let i = 0; i < steps; i++) {
    const next = {};
    for (const [symbol, count] of Object.entries(counts)) {
      for (const product of rules[symbol]) {
        next[product] = (next[product] ?? 0) + count;
      }
    }
    counts = next;
  }
  return Object.values(counts).reduce((total, count) => total + count, 0);
}`,
    tests: `const rules = { A: "B", B: "AB" };
check("small", lengthAfter(rules, "A", 4), 5);
check("longer start", lengthAfter(rules, "AB", 2), 5);
check("60 steps", lengthAfter(rules, "A", 60), 2504730781961);`,
  },
  {
    title: "Remember each rule",
    description: "Now there is no rule table. <code>breakDown(part)</code> returns the array of parts that one part turns into, and pretend it is slow. Save each answer in <code>known</code> so it is called at most once for each distinct part. This is memoization.",
    starter: `function partsAfter(start, breakDown, steps) {
  const known = new Map();
  let counts = new Map([[start, 1]]);
  for (let i = 0; i < steps; i++) {
    const next = new Map();
    for (const [part, count] of counts) {
      // TODO: look up the products in "known", calling breakDown only when missing
      // TODO: add "count" to each product in "next"
    }
    counts = next;
  }
  return [...counts.values()].reduce((total, count) => total + count, 0);
}`,
    solution: `function partsAfter(start, breakDown, steps) {
  const known = new Map();
  let counts = new Map([[start, 1]]);
  for (let i = 0; i < steps; i++) {
    const next = new Map();
    for (const [part, count] of counts) {
      if (!known.has(part)) known.set(part, breakDown(part));
      for (const product of known.get(part)) {
        next.set(product, (next.get(product) ?? 0) + count);
      }
    }
    counts = next;
  }
  return [...counts.values()].reduce((total, count) => total + count, 0);
}`,
    tests: `let calls = 0;
const breakDown = part => { calls++; return part === "x" ? ["y"] : ["x", "y"]; };
check("total parts", partsAfter("x", breakDown, 40), 165580141);
check("breakDown calls", calls, 2);`,
  },
];

if (typeof module !== "undefined") module.exports = exercises;

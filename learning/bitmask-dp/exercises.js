const exercises = [
  {
    title: "Membership",
    description: "Practice the three basic operations. A set is one integer, and bit <code>i</code> is 1 when item <code>i</code> is in it. Start by building <code>1 << item</code>, then decide which operator combines it with <code>set</code>.",
    starter: `function contains(set, item) {
  // TODO: true when bit "item" is on
}

function add(set, item) {
  // TODO: return a set that also has "item"
}

function remove(set, item) {
  // TODO: return a set without "item"
}`,
    solution: `function contains(set, item) {
  return (set & (1 << item)) !== 0;
}

function add(set, item) {
  return set | (1 << item);
}

function remove(set, item) {
  return set & ~(1 << item);
}`,
    tests: `check("contains a member", contains(5, 2), true);
check("rejects a non-member", contains(5, 1), false);
check("adds a new item", add(5, 1), 7);
check("adding twice changes nothing", add(5, 2), 5);
check("removes a member", remove(5, 0), 4);
check("removing a non-member changes nothing", remove(5, 1), 5);`,
  },
  {
    title: "List the members",
    description: "Go the other way: turn the integer back into an ordinary array. <code>n</code> is how many items exist, so check each item from 0 to <code>n - 1</code> and keep the ones whose bit is on.",
    starter: `function members(set, n) {
  const result = [];
  // TODO: collect every item from 0 to n - 1 that is in the set
  return result;
}`,
    solution: `function members(set, n) {
  const result = [];
  for (let item = 0; item < n; item++) {
    if (set & (1 << item)) result.push(item);
  }
  return result;
}`,
    tests: `check("empty set", members(0, 4), []);
check("some items", members(10, 4), [1, 3]);
check("full set", members(15, 4), [0, 1, 2, 3]);`,
  },
  {
    title: "Build on smaller sets",
    description: "Your first DP over subsets. <code>sums[set]</code> should hold the total of the values in that set. Adding up each set from scratch repeats work. Instead, take a finished set, add one item it lacks, and you have the total for a bigger set.",
    starter: `function subsetSums(values) {
  const sums = new Array(1 << values.length).fill(0);
  for (let set = 0; set < sums.length; set++) {
    for (let item = 0; item < values.length; item++) {
      // TODO: if "item" is NOT in the set, fill in the set that adds it
    }
  }
  return sums;
}`,
    solution: `function subsetSums(values) {
  const sums = new Array(1 << values.length).fill(0);
  for (let set = 0; set < sums.length; set++) {
    for (let item = 0; item < values.length; item++) {
      if (!(set & (1 << item))) {
        sums[set | (1 << item)] = sums[set] + values[item];
      }
    }
  }
  return sums;
}`,
    tests: `check("two values", subsetSums([3, 5]), [0, 3, 5, 8]);
check("three values", subsetSums([1, 2, 4]), [0, 1, 2, 3, 4, 5, 6, 7]);`,
  },
  {
    title: "Cheapest assignment",
    description: "Give each worker exactly one task, as cheaply as possible. <code>cost[worker][task]</code> is the price. <code>best[taken]</code> is the cheapest way to hand out exactly the tasks in <code>taken</code> to the first workers. If 2 tasks are taken, workers 0 and 1 have them, so worker 2 is next (the starter computes this for you). Try each free task and keep the cheaper total.",
    starter: `function cheapestAssignment(cost) {
  const n = cost.length;
  const best = new Array(1 << n).fill(Infinity);
  best[0] = 0;
  for (let taken = 0; taken < best.length; taken++) {
    const worker = taken.toString(2).replaceAll("0", "").length;
    for (let task = 0; task < n; task++) {
      // TODO: if the task is free, try giving it to this worker
    }
  }
  return best[best.length - 1];
}`,
    solution: `function cheapestAssignment(cost) {
  const n = cost.length;
  const best = new Array(1 << n).fill(Infinity);
  best[0] = 0;
  for (let taken = 0; taken < best.length; taken++) {
    const worker = taken.toString(2).replaceAll("0", "").length;
    for (let task = 0; task < n; task++) {
      if (!(taken & (1 << task))) {
        const next = taken | (1 << task);
        best[next] = Math.min(best[next], best[taken] + cost[worker][task]);
      }
    }
  }
  return best[best.length - 1];
}`,
    tests: `check("2 workers", cheapestAssignment([[1, 10], [10, 1]]), 2);
check("greedy would fail", cheapestAssignment([[1, 2], [1, 50]]), 3);
check("3 workers", cheapestAssignment([[9, 2, 7], [6, 4, 3], [5, 8, 1]]), 9);
const big = Array.from({ length: 14 }, (_, w) => Array.from({ length: 14 }, (_, t) => w === t ? 1 : 5));
check("14 workers, too many to try every order", cheapestAssignment(big), 14);`,
  },
  {
    title: "Remember where you ended",
    description: "Count the ways to line up <code>n</code> items when <code>allowed(a, b)</code> says whether <code>b</code> may come right after <code>a</code>. Knowing which items are used is not enough, because the next choice depends on the last one. So the state is <code>ways[set][last]</code>. Here you add up counts instead of keeping the smallest value.",
    starter: `function countLineups(n, allowed) {
  // ways[set][last] = lineups using exactly "set" and ending in "last"
  const ways = Array.from({ length: 1 << n }, () => new Array(n).fill(0));
  // TODO: seed the one-item sets
  // TODO: extend every state by one unused, allowed item
  // TODO: total the states that use every item
}`,
    solution: `function countLineups(n, allowed) {
  const ways = Array.from({ length: 1 << n }, () => new Array(n).fill(0));
  for (let item = 0; item < n; item++) ways[1 << item][item] = 1;
  for (let set = 1; set < 1 << n; set++) {
    for (let last = 0; last < n; last++) {
      if (ways[set][last] === 0) continue;
      for (let next = 0; next < n; next++) {
        if (!(set & (1 << next)) && allowed(last, next)) {
          ways[set | (1 << next)][next] += ways[set][last];
        }
      }
    }
  }
  return ways[(1 << n) - 1].reduce((total, count) => total + count, 0);
}`,
    tests: `check("anything allowed", countLineups(3, () => true), 6);
check("only increasing", countLineups(4, (a, b) => a < b), 1);
check("neighbours must differ by 1", countLineups(4, (a, b) => Math.abs(a - b) === 1), 2);
check("10 items", countLineups(10, () => true), 3628800);`,
  },
];

if (typeof module !== "undefined") module.exports = exercises;

# Bitmask sets and Dynamic Programming over subsets

This lesson has two ideas that work well together:

1. **Dynamic programming (DP):** solve small versions of a problem once, save the answers, and build bigger answers from them.
2. **Bitmasks:** a compact way to write down "which items have I used?" as a single number.

To practice, open [index.html](index.html) from your local clone. It has coding exercises you can run in the browser.

## When to reach for it

Think of this technique when a problem asks you to pick or order a **small** group of things (roughly 20 or fewer), you want the best result or the number of possible results, and trying every order would take too long.

An example: you have a handful of workers and the same number of tasks, each pairing has a price, and you want the cheapest way to give everyone exactly one task.

## Part 1: what is dynamic programming?

The name is historical and doesn't describe much. The idea is:

> Break a problem into smaller versions of itself, solve each one **once**, save the answer, and reuse it.

### Example: climbing stairs

You can climb 1 or 2 steps at a time. How many different ways are there to reach step 5?

Your last move onto step 5 came from either step 4 (a 1-step move) or step 3 (a 2-step move). Every route to step 5 is one of those two kinds, so:

```
ways(5) = ways(4) + ways(3)
```

You could write that as a function that calls itself. Look at what it ends up doing:

```
ways(5)
├── ways(4)
│   ├── ways(3)      <- worked out here
│   │   ├── ways(2)
│   │   └── ways(1)
│   └── ways(2)
└── ways(3)          <- and again here
    ├── ways(2)
    └── ways(1)
```

The same questions are answered again and again, and the repeated work grows exponentially as you add stairs.

DP fixes this by starting small and writing each answer down:

```js
const ways = [1, 1]; // step 0 and step 1
for (let step = 2; step <= 5; step++) {
  ways[step] = ways[step - 1] + ways[step - 2];
}
```

| Step | 0 | 1 | 2 | 3 | 4 | 5 |
|---|---|---|---|---|---|---|
| Ways | 1 | 1 | 2 | 3 | 5 | **8** |

Each answer is computed once and then simply looked up.

### The recipe

Four questions will get you through most DP problems:

| Question | Name | For the stairs |
|---|---|---|
| What does one saved answer mean? | the **state** | `ways[step]` is the number of ways to reach that step |
| Which answers do you already know? | the **base cases** | steps 0 and 1 have one way each |
| How is an answer built from smaller ones? | the **transition** | add the previous two |
| Where is the final answer? | the **result** | `ways[5]` |

DP helps when the big answer can be built from smaller answers, **and** the same smaller question comes up more than once.

## Part 2: DP over subsets

In the stairs example the state was a step number. In many problems the state is instead a **set of items**: which tasks are taken, or which places have been visited.

Suppose `best[set]` means "the cheapest way to handle exactly these items". The set `{0, 2}` can be built in two ways:

- start with `{0}` and add item 2
- start with `{2}` and add item 0

Whichever is cheaper is the answer for `{0, 2}`. From then on, nothing needs to remember which order was used. That is where the savings come from: many different orders collapse into one set.

To make this work in code, we need to use a set as an array index. That is what bitmasks are for.

## Part 3: storing a set in an int

A computer stores an integer as a row of bits, each 0 or 1. We can give each bit a meaning: **bit `i` is 1 when item `i` is in the set**. Bits are numbered from the right, starting at 0.

Here is the set `{0, 2}` written both ways:

```
Plain array                          Bitmask
-----------                          -------
index:   0     1      2              bit:     2 1 0
value: [true, false, true]           value:   1 0 1  = 5
```

Another way to see it: items 0, 1, 2, 3 are "worth" 1, 2, 4, 8. A set is the sum of its members, so `{0, 2}` is 1 + 4 = 5. Every set gets its own number.

### The operators

- `1 << i` ("shift left") takes the number 1 and slides it `i` places to the left. The result has only bit `i` turned on, and equals 2 to the power `i`.
- `a | b` ("or") turns on every bit that is on in `a` **or** `b`. It merges sets.
- `a & b` ("and") keeps only the bits that are on in `a` **and** `b`. It finds what two sets share.
- `~a` ("not") flips every bit.

| Operation | Plain array | Bitmask |
|---|---|---|
| Empty set | `new bool[n]` | `0` |
| Is `i` in the set? | `set[i]` | `(set & (1 << i)) != 0` |
| Add `i` | `set[i] = true` | `set \| (1 << i)` |
| Remove `i` | `set[i] = false` | `set & ~(1 << i)` |
| Set of all `n` items | all `true` | `(1 << n) - 1` |

For example, adding item 1 to `{0, 2}`:

```
set              = 101    {0, 2}
1 << 1           = 010    {1}
set | (1 << 1)   = 111    {0, 1, 2}
```

A set has no order, and adding an item that is already there changes nothing.

### Why not just use the array of booleans?

An int can be an **array index**, so you can write `best[set]` exactly like `ways[step]`. An array of booleans can't be used that way. With `n` items there are 2ⁿ possible sets, so the table has 2ⁿ entries.

There is also a free bonus: adding an item always makes the number bigger. If you loop `set` from small to large, every smaller set is finished before you need it.

## Part 4: choosing the state

Ask: **"to decide what can happen next, what do I need to know about the past?"**

- If knowing which items are used is enough, use `best[set]`.
- If the next choice also depends on the **last** item used (for example, the cost of moving from it to the next one), use `best[set][last]`.

Using `best[set][last]` to find the best route through a group of places is known as the **Held-Karp** algorithm.

Include only what you need. Every extra detail in the state makes the table bigger.

## Cost

The notation below describes how the work grows as `n` grows. `n!` ("n factorial") is n × (n−1) × … × 1, the number of ways to order `n` items.

| Approach | Growth | Steps when n = 15 |
|---|---|---|
| Try every order | n! | about 1.3 trillion |
| `best[set]` | 2ⁿ × n | about 490 thousand |
| `best[set][last]` | 2ⁿ × n² | about 7.4 million |

DP is far faster, but 2ⁿ still doubles with each new item, and the table has to fit in memory. That is why the technique suits small groups only.

## Where this helped

[2015 Day 9](../../2015/csharp/docs/day-09.md#----day-9-all-in-a-single-night----) ([part two](../../2015/csharp/docs/day-09.md#----part-two----))

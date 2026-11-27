# Counting instead of building

Some problems describe something that grows step by step, then ask only how big it gets. This lesson shows how to answer that **without ever creating the thing**.

To practice, open [index.html](index.html) from your local clone. It has coding exercises you can run in the browser.

## When to reach for it

Look for both of these:

- something grows every step, often very quickly
- the question asks only for a **total**, such as a length or a count, and not for the contents

## An example that grows

Start with the letter `A`. On every step, replace all letters at once using two rules:

- `A` becomes `B`
- `B` becomes `AB`

| Step | String | Length |
|---|---|---|
| 0 | `A` | 1 |
| 1 | `B` | 1 |
| 2 | `AB` | 2 |
| 3 | `BAB` | 3 |
| 4 | `ABBAB` | 5 |

The obvious program builds each new string from the previous one. It works for a few steps. But the length keeps multiplying by about 1.6, so by step 60 the string would have about 2.5 trillion characters. That won't fit in memory.

### A related trap: appending to strings

In C# and many other languages, a string can't be changed after it is created. Writing `text += "x"` makes a whole new string and copies the old one into it. Doing that in a loop copies the same characters over and over.

A **string builder** collects the pieces and creates the string once, which fixes the copying. It does not fix the growth: the final string is just as large.

## Ask what the question really needs

Length doesn't depend on order. `ABBAB` and `AABBB` are both 5 long. So instead of the string, store **how many of each letter** there are.

To take a step, apply each rule to a whole group at once. With 2 `A`s and 3 `B`s:

- the 2 `A`s each become `B`, giving 2 `B`s
- the 3 `B`s each become `AB`, giving 3 `A`s and 3 `B`s
- the result is **3 `A`s and 5 `B`s**

Here is the same run as before, using counts only:

| Step | A | B | Length (A + B) |
|---|---|---|---|
| 0 | 1 | 0 | 1 |
| 1 | 0 | 1 | 1 |
| 2 | 1 | 1 | 2 |
| 3 | 1 | 2 | 3 |
| 4 | 2 | 3 | 5 |

The lengths match, and each step now handles just two numbers, no matter how large they get. The work depends on how many **different kinds** of thing there are, not on how many there are in total.

## When is this allowed?

It works only if each part changes **independently**: what a part turns into must not depend on its neighbors. In other words, splitting first and then stepping gives the same result as stepping first:

```
expand(left + right) == expand(left) + expand(right)
```

Our letter rules pass, because each letter is replaced without looking around it.

A rule such as "replace each run of identical letters with its length" does **not** pass letter by letter, because a letter's neighbors decide how long its run is. In cases like that, look for bigger chunks that can never affect each other, and count those chunks instead of single letters.

Before relying on the technique, convince yourself the parts really are independent. A good check is to compare against the direct, string-building version for a small number of steps.

## Discovering rules as you go

Sometimes nobody hands you a table of rules. You have to work out what a part turns into, and that may be slow.

The answer for a given part never changes, so compute it the first time you see that part, **save it**, and look it up afterwards. This is called **memoization**: remembering the results of work you have already done.

With a million copies of one part and ten distinct parts overall, you do the slow work only ten times.

## Watch the number type

Counting saves memory, but the totals still grow quickly and can overflow, meaning they become too large for the number type. In C#, move from `int` to `long`, then to `BigInteger`. In JavaScript, ordinary numbers stop being exact above about 9 quadrillion, after which you need `BigInt`.

## Where this helped

[2015 Day 10](../../2015/csharp/docs/day-10.md#----day-10-elves-look-elves-say----) ([part two](../../2015/csharp/docs/day-10.md#----part-two----))

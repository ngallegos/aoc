\--- Day 3: Mull It Over ---
----------------------------

"Our computers are having issues, so I have no idea if we have any Chief Historians in stock! You're welcome to check the warehouse, though," says the mildly flustered shopkeeper at the [North Pole Toboggan Rental Shop](/2020/day/2). The Historians head out to take a look.

The shopkeeper turns to you. "Any chance you can see why our computers are having issues again?"

The computer appears to be trying to run a program, but its memory (your puzzle input) is _corrupted_. All of the instructions have been jumbled up!

It seems like the goal of the program is just to _multiply some numbers_. It does that with instructions like `mul(X,Y)`, where `X` and `Y` are each 1-3 digit numbers. For instance, `mul(44,46)` multiplies `44` by `46` to get a result of `2024`. Similarly, `mul(123,4)` would multiply `123` by `4`.

However, because the program's memory has been corrupted, there are also many invalid characters that should be _ignored_, even if they look like part of a `mul` instruction. Sequences like `mul(4*`, `mul(6,9!`, `?(12,34)`, or `mul ( 2 , 4 )` do _nothing_.

For example, consider the following section of corrupted memory:

    xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))

Only the four highlighted sections are real `mul` instructions. Adding up the result of each instruction produces `_161_` (`2*4 + 5*5 + 11*8 + 8*5`).

Scan the corrupted memory for uncorrupted `mul` instructions. _What do you get if you add up all of the results of the multiplications?_

\--- Part Two ---
-----------------

The engineers are surprised by the low number of safe reports until they realize they forgot to tell you about the Problem Dampener.

The Problem Dampener is a reactor-mounted module that lets the reactor safety systems _tolerate a single bad level_ in what would otherwise be a safe report. It's like the bad level never happened!

Now, the same rules apply as before, except if removing a single level from an unsafe report would make it safe, the report instead counts as safe.

More of the above example's reports are now safe:

*   `7 6 4 2 1`: _Safe_ without removing any level.
*   `1 2 7 8 9`: _Unsafe_ regardless of which level is removed.
*   `9 7 6 2 1`: _Unsafe_ regardless of which level is removed.
*   `1 _3_ 2 4 5`: _Safe_ by removing the second level, `3`.
*   `8 6 _4_ 4 1`: _Safe_ by removing the third level, `4`.
*   `1 3 6 7 9`: _Safe_ without removing any level.

Thanks to the Problem Dampener, `_4_` reports are actually _safe_!

Update your analysis by handling situations where the Problem Dampener can remove a single level from unsafe reports. _How many reports are now safe?_
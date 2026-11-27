# Repository rules

## Folder structure (enforced by CI discovery)

- Every project lives at `<year>/<language>/` (e.g. `2025/csharp`, `2023/kotlin`, `2024/go`). The language folder is the project root: the solution, `go.mod`, `package.json`, or `gradlew` goes directly inside it, never in a nested wrapper folder.
- Language folder names are lowercase and must match a language in `ci.yml` (`LANGUAGES`): `csharp`, `kotlin`, `go`.
- Code shared across years lives in `shared/<language>/`. Changes there trigger tests for every year of that language.
- Don't put project files directly in `<year>/`.

## Adding a day

Don't create day files by hand; use the scaffolding script from the language folder.

- C#: `cd <year>/csharp && ../../shared/csharp/new-day.sh <day>`. One shared script serves every year and reads the year from the folder name. It creates `Day<NN>Tests.cs`, empty `day-<NN>-01.txt` and `sample-<NN>-01.txt` under `AOC<year>.Tests/Inputs`, `docs/day-<NN>.md` (populated from the puzzle page via `curl` + `pandoc`; falls back to a placeholder if either is missing or the day isn't unlocked), and links the day in the folder's `README.md` calendar. Stubs carry `[Ignore("Not attempted yet")]`; remove it once the day is attempted.
- Go: `cd 2024/go && ./new-day.sh <day>`. It creates `days/<day>/` with `README.md`, `sample.txt`, `input.txt` and a test file from `new-day-template.txt`.
- A new C# year copies the test project (`AOC<year>.Tests` csproj, `TestBase.cs`, `Usings.cs`), `AOC<year>.slnx` and `README.md` from the latest year and renames the year. Point the new `.slnx` config folder at `../../shared/csharp/new-day.sh`; do not copy the script.

## Adding a new language

Add `.github/workflows/test-<language>.yml` (reusable, `path` input), then update `ci.yml` (`LANGUAGES`, `changes` outputs, a matrix job, the `ci` gate's `needs`) and the README tables. See the README "CI" section.

## Learning lessons

- A lesson teaches a reusable concept, not a day's puzzle. Use neutral examples and don't solve the puzzle in the lesson.
- Be more verbose than usual: this overrides any terse response style for lesson content. Write for easy understanding: define jargon, use examples and walk through each step.
- Each lesson lives at `learning/<concept>/` with a `README.md` (written explanation), an `index.html` (interactive page) and `exercises.js`. Shared styles and the exercise runner are in `learning/assets/`.
- Exercises are JavaScript skeletons with TODOs, each with `starter`, `tests` and `solution`. The solution must pass the tests and the starter must not.
- Add a "Where this helped" link to the puzzle section in `<year>/<language>/docs/day-<NN>.md`, and link back from that folder's `README.md`. Don't edit the day docs; `new-day.sh --refresh` overwrites them.
- Pages must work from `file://`: relative links only, no external URLs, CDNs or build step.
- Add the lesson to `learning/README.md` and `learning/index.html`.

## Other

- Update the root `README.md` year list when adding a year or language.

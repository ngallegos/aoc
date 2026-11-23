# Repository rules

## Folder structure (enforced by CI discovery)

- Every project lives at `<year>/<language>/` (e.g. `2025/csharp`, `2023/kotlin`, `2024/go`). The language folder is the project root: the solution, `go.mod`, `package.json`, or `gradlew` goes directly inside it, never in a nested wrapper folder.
- Language folder names are lowercase and must match a language in `ci.yml` (`LANGUAGES`): `csharp`, `kotlin`, `go`.
- Code shared across years lives in `shared/<language>/`. Changes there trigger tests for every year of that language.
- Don't put project files directly in `<year>/`.

## Adding a new language

Add `.github/workflows/test-<language>.yml` (reusable, `path` input), then update `ci.yml` (`LANGUAGES`, `changes` outputs, a matrix job, the `ci` gate's `needs`) and the README tables. See the README "CI" section.

## Other

- Update the root `README.md` year list when adding a year or language.

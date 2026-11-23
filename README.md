# Advent of Code

My attempts over the years at Advent of Code.

- 2026
  - [C#](2026/csharp)
- 2025
  - [C#](2025/csharp)
- 2024
  - [Go](2024/go)
  - [C#](2024/csharp)
- 2023
  - [Kotlin](2023/kotlin)
  - [C#](2023/csharp)
- 2022
  - [C#](2022/csharp)
- 2021
  - [C#](2021/csharp)
- 2019
  - [C#](2019/csharp)
- 2018
  - [C#](2018/csharp)
- 2015
  - [C#](2015/csharp)

## Repository layout

```
<year>/<language>/   one self-contained project per year and language
shared/<language>/   code reused across years (e.g. shared/csharp/AOC.Helpers)
.github/workflows/   CI
```

The `<language>` folder is the project root: the solution, `go.mod`, or Gradle wrapper sits directly inside it.

## CI

`ci.yml` discovers every `<year>/<language>` folder and tests only the ones that changed, using the reusable `test-<language>.yml` workflow for that language.

A project is tested when:

- its `<year>/<language>` folder changed
- `shared/<language>` changed
- anything in `.github/workflows` changed
- the workflow was run manually (tests everything)

Require the `CI` check in branch protection.

| Folder | Workflow | Runs |
|---|---|---|
| `csharp` | `test-dotnet.yml` | `dotnet test` |
| `kotlin` | `test-gradle.yml` | `./gradlew test` |
| `go` | `test-go.yml` | `go test ./...` |

### Adding a language

1. Add `.github/workflows/test-<language>.yml` taking a `path` input.
2. In `ci.yml`, add the folder name to `LANGUAGES` and the `changes` job `outputs`, plus a job that mirrors the existing language jobs.
3. Add a row to the table above.

Adding a year needs no CI changes.

## Adding a day

From a year's language folder, run the scaffolding script: `cd 2026/csharp && ../../shared/csharp/new-day.sh 2` for C#, or `cd 2024/go && ./new-day.sh <day>` for Go. The C# script is shared by all years and takes the year from the folder name. It scaffolds the test file, empty sample and input files, and a docs stub, and links the day in that folder's README calendar.

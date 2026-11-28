#!/usr/bin/env bash
set -euo pipefail

refresh=false
if [ "${1:-}" = "--refresh" ]; then
  refresh=true
  shift
fi

if [ "$#" -ne 1 ]; then
  echo "Usage: $0 [--refresh] <day-number>"
  exit 1
fi

day="$1"

# Run from <year>/csharp; the year comes from the parent folder
year="$(basename "$(dirname "$PWD")")"
if ! [[ "$year" =~ ^[0-9]{4}$ ]]; then
  echo "Error: run this from a <year>/csharp folder."
  exit 1
fi

# Years without a test project only get the docs (and README link, if a README exists)
has_tests=false
if [ -d "./AOC${year}.Tests" ]; then
  has_tests=true
else
  echo "No AOC${year}.Tests found; only creating docs."
fi

if ! [[ "$day" =~ ^[0-9]+$ ]]; then
  echo "Error: day must be a positive integer."
  exit 1
fi

day_padded=$(printf "%02d" "$day")

if $has_tests; then
  # Inputs directory and files
  inputs_dir="./AOC${year}.Tests/Inputs"
  mkdir -p "$inputs_dir"

  day_file="$inputs_dir/day-${day_padded}-01.txt"
  sample_file="$inputs_dir/sample-${day_padded}-01.txt"

  for f in "$day_file" "$sample_file"; do
    if [ -e "$f" ]; then
      echo "Skipping existing: $f"
    else
      touch "$f"
      echo "Created: $f"
    fi
  done
fi

# Prints the puzzle description as markdown; fails (with a warning) so the caller can fall back
fetch_description() {
  if ! command -v pandoc >/dev/null 2>&1; then
    echo "Warning: pandoc not installed; using placeholder description." >&2
    return 1
  fi

  # Part 2 is only served to a logged-in session that has solved part 1
  local session="${AOC_SESSION:-}"
  local session_file="${XDG_CONFIG_HOME:-$HOME/.config}/aoc/session"
  if [ -z "$session" ] && [ -r "$session_file" ]; then
    session="$(<"$session_file")"
  fi

  local html article
  # The cookie goes through a config file descriptor to keep it out of the process list
  if ! html="$(curl -fsSL -A "new-day.sh (advent of code scaffolder)" \
    -K <(if [ -n "$session" ]; then printf 'cookie = "session=%s"\n' "$session"; fi) \
    "https://adventofcode.com/${year}/day/${day}")"; then
    echo "Warning: could not fetch puzzle page; using placeholder description." >&2
    return 1
  fi

  article="$(printf "%s" "$html" | perl -0777 -ne 'print "$1\n" while /<article class="day-desc">(.*?)<\/article>/sg')"
  if [ -z "$article" ]; then
    echo "Warning: no day-desc article found; using placeholder description." >&2
    return 1
  fi

  printf "%s" "$article" | pandoc -f html -t gfm-raw_html --wrap=none
}

# Docs directory and file
docs_dir="./docs"
mkdir -p "$docs_dir"

doc_file="$docs_dir/day-${day_padded}.md"
if [ -e "$doc_file" ] && ! $refresh; then
  echo "Skipping existing: $doc_file"
else
  if fetch_description > "$doc_file.tmp"; then
    mv "$doc_file.tmp" "$doc_file"
    echo "Wrote: $doc_file"
  elif [ -e "$doc_file" ]; then
    rm -f "$doc_file.tmp"
    echo "Keeping existing: $doc_file"
  else
    rm -f "$doc_file.tmp"
    printf "# Day %s\n\nDescription and notes for Day %s.\n" "$day" "$day" > "$doc_file"
    echo "Created: $doc_file"
  fi
fi

# Writes a README with a December calendar table (plain day numbers)
init_readme() {
  # Weekday of Dec 1 (0=Sunday), Sakamoto's algorithm
  local y=$((year))
  local first_weekday=$(( (y + y/4 - y/100 + y/400 + 5) % 7 ))
  {
    printf "# Advent of Code %s C#\n\n### December %s\n\n" "$year" "$year"
    printf "| Sun |  Mon  | Tue | Wed | Thu | Fri | Sat |\n"
    printf "|:---:|:-----:|:---:|:---:|:---:|:---:|:---:|\n"
    local cell
    local row=""
    for ((cell = 0; cell < 42; cell++)); do
      local d=$((cell - first_weekday + 1))
      if ((d >= 1 && d <= 31)); then row+="| $d "; else row+="|  "; fi
      if (((cell + 1) % 7 == 0)); then
        printf "%s|\n" "$row"
        row=""
        if ((d >= 31)); then break; fi
      fi
    done
  } > "$readme"
  echo "Created: $readme"
}

# Update README.md table cell with link to the doc (if not already present)
readme="./README.md"
if [ ! -e "$readme" ]; then
  init_readme
fi

link_line="- [Day ${day}](docs/day-${day_padded}.md)"

# Check by file path to avoid duplicate links with different text
if grep -Fq "docs/day-${day_padded}.md" "$readme"; then
  echo "README already contains link to docs/day-${day_padded}.md"
else
  tmp="$(mktemp)"
  # Attempt to replace the matching table cell that contains the plain day number with a link.
  # Split table lines by '|' and replace the cell that exactly equals the day number (after trimming).
  if awk -v day="$day" -v day_padded="$day_padded" '
    BEGIN { OFS="|" }
    /^\|/ {
      n = split($0, a, "|")
      changed = 0
      for (i = 2; i < n; i++) {
        field = a[i]
        gsub(/^ +| +$/, "", field)
        if (field == day) {
          a[i] = " " "[" day "]" "(" "docs/day-" day_padded ".md" ")" " "
          changed = 1
        }
      }
      if (changed) {
        out = a[1]
        for (i = 2; i <= n; i++) out = out "|" a[i]
        print out
        updated = 1
        next
      }
    }
    { print }
    END { if (updated == 1) exit 0; else exit 1 }
  ' "$readme" > "$tmp"; then
    mv "$tmp" "$readme"
    echo "Updated table in $readme for day $day"
  else
    rm -f "$tmp"
    # Fallback: append a bullet link if no matching table cell was found.
    printf "\n%s\n" "- $link_line" >> "$readme"
    echo "Appended link to $readme"
  fi
fi

if $has_tests; then
  # Tests directory and file (creates DayNNTests.cs)
  tests_dir="./AOC${year}.Tests"
  mkdir -p "$tests_dir"

  test_file="$tests_dir/Day${day_padded}Tests.cs"
  if [ -e "$test_file" ]; then
    echo "Skipping existing: $test_file"
  else
    cat > "$test_file" <<EOF
namespace AOC${year}.Tests;

public class Day${day_padded}Tests : TestBase
{
    [Ignore("Not attempted yet")]
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart1_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    [Ignore("Not attempted yet")]
    protected override void SolvePart2_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }
}
EOF
    echo "Created: $test_file"
  fi
fi

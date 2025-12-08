#!/usr/bin/env bash
set -euo pipefail

if [ "$#" -ne 1 ]; then
  echo "Usage: $0 <day-number>"
  exit 1
fi

day="$1"

if ! [[ "$day" =~ ^[0-9]+$ ]]; then
  echo "Error: day must be a positive integer."
  exit 1
fi

day_padded=$(printf "%02d" "$day")

# Inputs directory and files
inputs_dir="./AOC2025.tests/Inputs"
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

# Docs directory and file
docs_dir="./docs"
mkdir -p "$docs_dir"

doc_file="$docs_dir/day-${day_padded}.md"
if [ -e "$doc_file" ]; then
  echo "Skipping existing: $doc_file"
else
  printf "# Day %s\n\nDescription and notes for Day %s.\n" "$day" "$day" > "$doc_file"
  echo "Created: $doc_file"
fi

# Update README.md table cell with link to the doc (if not already present)
readme="./README.md"
if [ ! -e "$readme" ]; then
  touch "$readme"
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

# Tests directory and file (creates DayNNTests.cs)
tests_dir="./AOC2025.Tests"
mkdir -p "$tests_dir"

test_file="$tests_dir/Day${day_padded}Tests.cs"
if [ -e "$test_file" ]; then
  echo "Skipping existing: $test_file"
else
  cat > "$test_file" <<EOF
namespace AOC2025.Tests;

public class Day${day_padded}Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

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
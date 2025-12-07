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

day_file="$inputs_dir/day-${day}-01.txt"
sample_file="$inputs_dir/sample-${day}-01.txt"

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

doc_file="$docs_dir/day-${day}.md"
if [ -e "$doc_file" ]; then
  echo "Skipping existing: $doc_file"
else
  printf "# Day %s\n\nDescription and notes for Day %s.\n" "$day" "$day" > "$doc_file"
  echo "Created: $doc_file"
fi

# Update README.md with link to the doc (if not already present)
readme="./README.md"
if [ ! -e "$readme" ]; then
  touch "$readme"
fi

link_line="- [Day ${day}](docs/day-${day}.md)"

# Check by file path to avoid duplicate links with different text
if grep -Fq "docs/day-${day}.md" "$readme"; then
  echo "README already contains link to docs/day-${day}.md"
else
  printf "\n%s\n" "$link_line" >> "$readme"
  echo "Appended link to $readme"
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
        throw new System.NotImplementedException();
    }

    protected override void SolvePart1_Actual()
    {
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Sample()
    {
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        throw new System.NotImplementedException();
    }
}
EOF
  echo "Created: $test_file"
fi
package day04

import (
	"bufio"
	"os"
	"testing"
)

func Part1(isSample bool) int {
	var lines []string
	var err error
	if isSample {
		lines, err = readSample()
	} else {
		lines, err = readInput()
	}

	results := -1

	if &err != nil && len(lines) > 0 {
		// todo - implement

		return results
	}

	panic(err)
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 0 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "0")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 0 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "0")
	}
}

func Part2(isSample bool) int {
	var lines []string
	var err error
	if isSample {
		lines, err = readSample()
	} else {
		lines, err = readInput()
	}

	results := -1

	if &err != nil && len(lines) > 0 {
		// todo - implement

		return results
	}

	panic(err)
}

func TestPart2(t *testing.T) {
	result := Part2(false)
	if result != 0 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "0")
	}

}

func TestSamplePart2(t *testing.T) {
	result := Part2(true)
	if result != 0 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "0")
	}
}

func readInput() ([]string, error) {
	return readLines(`./input.txt`)
}

func readSample() ([]string, error) {
	return readLines(`./sample.txt`)
}

func readLines(path string) ([]string, error) {
	file, err := os.Open(path)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	var lines []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		lines = append(lines, scanner.Text())
	}
	//log.Println(lines)
	return lines, scanner.Err()
}

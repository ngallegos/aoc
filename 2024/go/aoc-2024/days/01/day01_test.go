package day01

import (
	"bufio"
	"log"
	"os"
	"testing"
)

func Part1(isSample bool) string {
	var lines []string
	var err error
	if isSample {
		lines, err = readSample()
	} else {
		lines, err = readInput()
	}

	log.Println(err)
	if &err != nil {
		return lines[0]
	}
	return "failed to read"
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != "day 01" {
		t.Errorf("Result was incorrect, got: %s, want: %s.", result, "day 1")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != "line1" {
		t.Errorf("Result was incorrect, got: %s, want: %s.", result, "line1")
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
	log.Println(lines)
	return lines, scanner.Err()
}

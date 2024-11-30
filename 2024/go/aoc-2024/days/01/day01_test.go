package main

import (
	"bufio"
	"fmt"
	"os"
	"testing"
)

func Part1() string {
	fmt.Println("day 1")
	return "day 1"
}

func TestPart1(t *testing.T) {
	result := Part1()
	if result != "day 1" {
		t.Errorf("Result was incorrect, got: %s, want: %s.", result, "day 1")
	}

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
	return lines, scanner.Err()
}

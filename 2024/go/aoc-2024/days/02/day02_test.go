package day02

import (
	"bufio"
	"os"
	"strconv"
	"strings"
	"testing"
)

func Part1(isSample bool) int {
	var reports [][]int
	var err error
	if isSample {
		reports, err = readSample()
	} else {
		reports, err = readInput()
	}
	return 0
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

func readInput() ([][]int, error) {
	return readLines(`./input.txt`)
}

func readSample() ([][]int, error) {
	return readLines(`./sample.txt`)
}

func readLines(path string) ([][]int, error) {
	file, err := os.Open(path)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	var lines [][]int
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := toIntArray(strings.Fields(scanner.Text()))

		lines = append(lines, line)
	}
	//log.Println(lines)
	return lines, scanner.Err()
}

func toIntArray(input []string) []int {
	var result = []int{}

	for _, stringVal := range input {
		intVal, err := strconv.Atoi(stringVal)
		if err != nil {
			panic(err)
		}
		result = append(result, intVal)
	}
}

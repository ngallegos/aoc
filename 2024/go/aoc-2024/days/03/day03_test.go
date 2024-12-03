package day03

import (
	"bufio"
	//"log"
	"os"
	"regexp"
	"strconv"
	"testing"
)

func Part1(isSample bool) int {
	var corruptedMemory []string
	var err error
	if isSample {
		corruptedMemory, err = readSample()
	} else {
		corruptedMemory, err = readInput()
	}

	results := 0

	if &err != nil {
		r := regexp.MustCompile(`mul\((\d+),(\d+)\)`)
		for i := 0; i < len(corruptedMemory); i++ {
			matches := r.FindAllStringSubmatch(corruptedMemory[i], -1)
			//log.Println(matches)
			for _, m := range matches {
				arg1, err1 := strconv.Atoi(m[1])
				arg2, err2 := strconv.Atoi(m[2])
				if err1 == nil && err2 == nil {
					results += arg1 * arg2
				}
			}

		}

		return results
	}

	panic(err)
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 175015740 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "175015740")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 161 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "161")
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

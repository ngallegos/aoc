package day01

import (
	"bufio"
	"os"
	"slices"
	"strconv"
	"strings"
	"testing"
)

func Part1(isSample bool) int {
	var col1 []int
	var col2 []int
	var err error
	if isSample {
		col1, col2, err = readSample()
	} else {
		col1, col2, err = readInput()
	}

	//log.Println(err)
	if &err != nil {
		slices.Sort(col1)
		slices.Sort(col2)
		sum := 0

		for i := 0; i < len(col1); i++ {
			sum += absDiffInt(col1[i], col2[i])
		}

		return sum
	}
	return -1
}

func absDiffInt(x, y int) int {
	if x < y {
		return y - x
	}
	return x - y
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 2367773 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "2367773")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 11 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "11")
	}
}

func Part2(isSample bool) int64 {
	var col1 []int
	var col2 []int
	var err error
	if isSample {
		col1, col2, err = readSample()
	} else {
		col1, col2, err = readInput()
	}

	//log.Println(err)
	if &err != nil {
		slices.Sort(col1)
		slices.Sort(col2)
		sum := int64(0)

		for i := 0; i < len(col1); i++ {
			sum += int64(col1[i]) * numOccurences(col1[i], col2)
		}

		return sum
	}
	return -1
}

func numOccurences(target int, pool []int) int64 {
	occurences := int64(0)
	for _, i := range pool {
		if i == target {
			occurences++
		}
	}

	return occurences
}

func TestPart2(t *testing.T) {
	result := Part2(false)
	if result != 21271939 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "21271939")
	}

}

func TestSamplePart2(t *testing.T) {
	result := Part2(true)
	if result != 31 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "31")
	}
}

func readInput() ([]int, []int, error) {
	return readLines(`./input.txt`)
}

func readSample() ([]int, []int, error) {
	return readLines(`./sample.txt`)
}

func readLines(path string) ([]int, []int, error) {
	file, err := os.Open(path)
	if err != nil {
		return nil, nil, err
	}
	defer file.Close()

	var col1 []int
	var col2 []int
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := strings.Fields(scanner.Text())
		if i, err := strconv.Atoi(line[0]); err == nil {
			col1 = append(col1, i)
		}
		if i, err := strconv.Atoi(line[1]); err == nil {
			col2 = append(col2, i)
		}
		//lines = append(lines, scanner.Text())
	}
	//log.Println(lines)
	return col1, col2, scanner.Err()
}

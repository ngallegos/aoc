package day02

import (
	"bufio"
	"log"
	"os"
	"slices"
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

	safeReports := 0

	if &err != nil {
		for i := 0; i < len(reports); i++ {
			report := reports[i]
			direction := 0
			reportIsSafe := true
			for j := 0; j < (len(report) - 1); j++ {
				prevDirection := direction
				diff, curDirection := absDiffInt(report[j], report[j+1])
				direction = curDirection
				//log.Printf("%d -> %d", prevDirection, direction)
				if diff < 1 || diff > 3 || (prevDirection != direction && prevDirection != 0) {
					reportIsSafe = false
					break
				}
			}
			if reportIsSafe {
				//log.Println(report)
				safeReports++
			}
		}

		return safeReports
	}

	panic(err)
}

func absDiffInt(x, y int) (int, int) {
	if x < y {
		return y - x, 1
	} else if y < x {
		return x - y, -1
	}
	return x - y, -2
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 369 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "369")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 2 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "2")
	}
}

func Part2(isSample bool) int {
	var reports [][]int
	var err error
	if isSample {
		reports, err = readSample()
	} else {
		reports, err = readInput()
	}

	safeReports := 0

	if &err != nil {
		for i := 0; i < len(reports); i++ {
			report := reports[i]
			direction := 0
			reportIsSafe := true
			unsafeReportFirstIndex := -1
			for j := 0; j < (len(report) - 1); j++ {
				prevDirection := direction
				diff, curDirection := absDiffInt(report[j], report[j+1])
				direction = curDirection
				//log.Printf("%d -> %d", prevDirection, direction)
				if diff < 1 || diff > 3 || (prevDirection != direction && prevDirection != 0) {
					unsafeReportFirstIndex = j
					break
				}
			}

			if unsafeReportFirstIndex > -1 {
				moddedReport := slices.Clone(report)
				moddedReport = append(moddedReport[:unsafeReportFirstIndex], moddedReport[unsafeReportFirstIndex+1:]...)
				log.Println(report)
				for j := 0; j < (len(moddedReport) - 1); j++ {
					prevDirection := direction
					diff, curDirection := absDiffInt(moddedReport[j], moddedReport[j+1])
					direction = curDirection
					//log.Printf("%d -> %d", prevDirection, direction)
					if diff < 1 || diff > 3 || (prevDirection != direction && prevDirection != 0) {
						log.Printf("first chance: %v", moddedReport)
						reportIsSafe = false
						break
					}
				}
			}

			if !reportIsSafe && unsafeReportFirstIndex < len(report)-1 {
				reportIsSafe = true
				moddedReport := slices.Clone(report)
				moddedReport = append(moddedReport[:unsafeReportFirstIndex+1], moddedReport[unsafeReportFirstIndex+2:]...)
				log.Println(report)
				for j := 0; j < (len(moddedReport) - 1); j++ {
					prevDirection := direction
					diff, curDirection := absDiffInt(moddedReport[j], moddedReport[j+1])
					direction = curDirection
					//log.Printf("%d -> %d", prevDirection, direction)
					if diff < 1 || diff > 3 || (prevDirection != direction && prevDirection != 0) {
						log.Printf("last chance: %v", moddedReport)
						reportIsSafe = false
						break
					}
				}
			}

			if reportIsSafe {
				//log.Println(report)
				safeReports++
			}
		}

		return safeReports
	}

	panic(err)
}

func TestPart2(t *testing.T) {
	result := Part2(false)
	if result != 369 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "369")
	}

}

func TestSamplePart2(t *testing.T) {
	result := Part2(true)
	if result != 4 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "4")
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

	return result
}

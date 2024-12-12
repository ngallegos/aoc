package day11

import (
	"bufio"
	//"log"
	"os"
	"strconv"
	"strings"
	"testing"
)

func Part1(isSample bool) int {
	var stones []int64
	var err error
	if isSample {
		stones, err = readSample()
	} else {
		stones, err = readInput()
	}

	if &err != nil && len(stones) > 0 {
		numBlinks := 25
		curStoneCount := len(stones)
		//log.Printf("blink %d: curStoneCount %d stones\n\t %v", 0, curStoneCount, stones)
		for i := 0; i < numBlinks; i++ {
			newStones := []int64{}
			for _, stone := range stones {
				newStones = append(newStones, blink(stone)...)
				//log.Printf("new stone: %d\n\t%v", si, newStones)
			}
			curStoneCount = len(newStones)
			stones = newStones
			//log.Printf("blink %d: curStoneCount %d stones\n\t %v\nnew stones\n\t %v", i+1, curStoneCount, stones, newStones)
		}

		return curStoneCount
	}

	panic(err)
}

func blink(stone int64) []int64 {
	if stone == 0 {
		return []int64{1}
	}
	stoneStr := strconv.FormatInt(stone, 10)
	if len(stoneStr)%2 == 0 {
		mid := len(stoneStr) / 2
		//log.Println(mid)
		newStone1, err1 := strconv.Atoi(stoneStr[:mid])
		newStone2, err2 := strconv.Atoi(stoneStr[mid:])
		if err1 != nil {
			panic(err1)
		}
		if err2 != nil {
			panic(err2)
		}
		return []int64{int64(newStone1), int64(newStone2)}
	}

	return []int64{stone * 2024}
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 229043 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "229043")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 55312 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "55312")
	}
}

func Part2(isSample bool) int {
	var stones []int64
	var err error
	if isSample {
		stones, err = readSample()
	} else {
		stones, err = readInput()
	}

	if &err != nil && len(stones) > 0 {
		numBlinks := 75
		curStoneCount := len(stones)
		//log.Printf("blink %d: curStoneCount %d stones\n\t %v", 0, curStoneCount, stones)
		for i := 0; i < numBlinks; i++ {
			newStones := []int64{}
			for _, stone := range stones {
				newStones = append(newStones, blink(stone)...)
				//log.Printf("new stone: %d\n\t%v", si, newStones)
			}
			curStoneCount = len(newStones)
			stones = newStones
			//log.Printf("blink %d: curStoneCount %d stones\n\t %v\nnew stones\n\t %v", i+1, curStoneCount, stones, newStones)
		}

		return curStoneCount
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

func readInput() ([]int64, error) {
	return readLines(`./input.txt`)
}

func readSample() ([]int64, error) {
	return readLines(`./sample.txt`)
}

func readLines(path string) ([]int64, error) {
	file, err := os.Open(path)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	var lines []int64
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := toInt64Array(strings.Fields(scanner.Text()))

		lines = line
		break
	}
	//log.Println(lines)
	return lines, scanner.Err()
}

func toInt64Array(input []string) []int64 {
	var result = []int64{}

	for _, stringVal := range input {
		intVal, err := strconv.Atoi(stringVal)
		if err != nil {
			panic(err)
		}
		result = append(result, int64(intVal))
	}

	return result
}

package day04

import (
	"bufio"
	"log"
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

	results := 0

	if &err != nil && len(lines) > 0 {
		for y := 0; y < len(lines); y++ {
			line := lines[y]
			for x := 0; x < len(line); x++ {
				if line[x] == 'X' {
					results += findXMASInRadius(x, y, lines)
				}
			}
		}

		return results
	}

	panic(err)
}

func findXMASInRadius(x int, y int, wordsearch []string) int {
	keyword := "XMAS"
	kLength := len(keyword)
	xmin := 0
	xmax := len(wordsearch[0]) - 1
	ymin := 0
	ymax := len(wordsearch) - 1

	instances := 0

	log.Printf("(%d, %d)", x, y)
	if x-kLength+1 >= xmin {
		//log.Println(Reverse(wordsearch[y][x-kLength+1 : x+1]))
		if Reverse(wordsearch[y][x-kLength+1:x+1]) == keyword {
			instances++
		}

		if y-kLength+1 >= ymin {
			word := ""
			for i := 0; i < kLength; i++ {
				word += string(wordsearch[y-i][x-i])
			}
			if Reverse(word) == keyword {
				instances++
			}
		}

		if y+kLength-1 <= ymax {
			word := ""
			for i := 0; i < kLength; i++ {
				word = string(wordsearch[y+i][x-i])
			}
			if Reverse(word) == keyword {
				instances++
			}

		}
	}

	if x+kLength-1 <= xmax {

		if wordsearch[y][x:x+kLength] == keyword {
			//log.Println(wordsearch[y][x : x+kLength])
			instances++
		}

		if y-kLength+1 >= ymin {
			word := ""
			for i := 0; i < kLength; i++ {
				word += string(wordsearch[y-i][x+i])
			}
			//log.Println(word)
			if word == keyword {
				instances++
			}
		}

		if y+kLength-1 <= ymax {
			runes := ""
			for i := 0; i < kLength; i++ {
				runes += string(wordsearch[y+i][x+i])
			}
			//log.Println(runes)
			if runes == keyword {
				instances++
			}

		}
	}

	if y-kLength+1 >= ymin {
		word := ""
		for i := 0; i < kLength; i++ {
			word += string(wordsearch[y-i][x])
		}
		//log.Println(word)
		if Reverse(word) == keyword {
			instances++
		}
	}

	if y+kLength-1 <= ymax {
		word := ""
		for i := 0; i < kLength; i++ {
			word += string(wordsearch[y+i][x])
		}
		//log.Println(word)
		if word == keyword {
			instances++
		}
	}

	return instances
}

func Reverse(s string) string {
	r := []rune(s)
	for i, j := 0, len(r)-1; i < len(r)/2; i, j = i+1, j-1 {
		r[i], r[j] = r[j], r[i]
	}
	return string(r)
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 0 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "0")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 18 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "18")
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

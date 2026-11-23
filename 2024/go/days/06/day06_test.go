package day06

import (
	"bufio"
	"log"
	"os"
	"slices"
	"testing"
)

func Part1(isSample bool) int {
	var positions [][]MapPosition
	var guard Guard
	var err error
	if isSample {
		positions, guard, err = readSample()
	} else {
		positions, guard, err = readInput()
	}

	results := 0

	if &err != nil && len(positions) > 0 && guard.X > -1 {

		patrolFinished := false

		for !patrolFinished {
			newPositions, finished := protocolStep(&guard, positions)
			results += newPositions
			patrolFinished = finished
		}

		return results
	}

	panic(err)
}

func protocolStep(guard *Guard, positions [][]MapPosition) (int, bool) {
	if guard.X < 0 ||
		guard.X >= len(positions[0]) ||
		guard.Y < 0 ||
		guard.Y >= len(positions) {
		return 0, true
	}

	positions[guard.Y][guard.X].TimesVisited++
	newPositionCount := 0
	if positions[guard.Y][guard.X].TimesVisited == 1 {
		newPositionCount = 1
	}

	guard.updatePosition(positions)

	return newPositionCount, false
}

func (g *Guard) updatePosition(positions [][]MapPosition) {
	nextX := g.X
	nextY := g.Y
	nextDirection := g.Direction

	switch g.Direction {
	case "^":
		nextY--
	case "v":
		nextY++
	case "<":
		nextX--
	case ">":
		nextX++
	}

	if nextX < 0 ||
		nextX >= len(positions[0]) ||
		nextY < 0 ||
		nextY >= len(positions) {
		g.X = nextX
		g.Y = nextY
		return
	}

	nextPosition := positions[nextY][nextX]
	for nextPosition.Obstruction {
		nextX = g.X
		nextY = g.Y
		switch nextDirection {
		case "^":
			nextDirection = ">"
			nextX++
		case ">":
			nextDirection = "v"
			nextY++
		case "v":
			nextDirection = "<"
			nextX--
		case "<":
			nextDirection = "^"
			nextY--
		}
		if nextX >= 0 &&
			nextX < len(positions[0]) &&
			nextY >= 0 &&
			nextY < len(positions) {
			break
		}
		nextPosition = positions[nextY][nextX]
		log.Println(nextPosition)
	}

	g.X = nextX
	g.Y = nextY
	g.Direction = nextDirection
}

func TestPart1(t *testing.T) {
	result := Part1(false)
	if result != 5329 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "5329")
	}

}

func TestSamplePart1(t *testing.T) {
	result := Part1(true)
	if result != 41 {
		t.Errorf("Result was incorrect, got: %d, want: %s.", result, "41")
	}
}

func Part2(isSample bool) int {
	var positions [][]MapPosition
	var guard Guard
	var err error
	if isSample {
		positions, guard, err = readSample()
	} else {
		positions, guard, err = readInput()
	}

	results := -1

	if &err != nil && len(positions) > 0 && guard.X > -1 {
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

func readInput() ([][]MapPosition, Guard, error) {
	return readLines(`./input.txt`)
}

func readSample() ([][]MapPosition, Guard, error) {
	return readLines(`./sample.txt`)
}

func readLines(path string) ([][]MapPosition, Guard, error) {
	file, err := os.Open(path)
	if err != nil {
		return nil, Guard{}, err
	}
	defer file.Close()

	var positions [][]MapPosition
	scanner := bufio.NewScanner(file)
	y := 0
	guard := Guard{
		X:         0,
		Y:         0,
		Direction: "^",
	}
	directions := []string{"^", "v", "<", ">"}
	for scanner.Scan() {
		line := []rune(scanner.Text())
		var mapLine []MapPosition
		for x, pos := range line {
			mapLine = append(mapLine, MapPosition{
				Obstruction: string(pos) == "#",
			})
			if slices.Contains(directions, string(pos)) {
				guard.X = x
				guard.Y = y
				guard.Direction = string(pos)
			}
		}
		positions = append(positions, mapLine)
		y++
	}
	//log.Println(lines)
	return positions, guard, scanner.Err()
}

type Guard struct {
	X         int
	Y         int
	Direction string
}
type MapPosition struct {
	Obstruction  bool
	TimesVisited int
}

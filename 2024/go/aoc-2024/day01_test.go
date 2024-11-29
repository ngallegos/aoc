package days

import (
	"fmt"
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

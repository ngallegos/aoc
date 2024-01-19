import kotlin.test.assertEquals

class Day01Tests : TestBase() {
    override fun SolvePart1_Sample() {

        val expectedCalibrationSum = 142;
        val calibrationValues = get_sample()
            .map { getNumberFromFirstAndLastDigitsInString(it) };
        val calibrationSum = calibrationValues.sum();

        assertEquals(expectedCalibrationSum, calibrationSum);
    }

    override fun SolvePart1_Actual() {

        val expectedCalibrationSum = 54601;
        val calibrationValues = get_input()
            .map { getNumberFromFirstAndLastDigitsInString(it) };
        val calibrationSum = calibrationValues.sum();

        assertEquals(expectedCalibrationSum, calibrationSum);
    }

    private fun getNumberFromFirstAndLastDigitsInString(input: String) : Int {
        val regex = Regex("""\D""");
        val justNumbers = input.replace(regex, "");
        val charArray = charArrayOf(justNumbers.first(), justNumbers.last());
        val firstAndLast = String(charArray);
        return firstAndLast.toInt();
    }
    override fun SolvePart2_Sample() {
        val expectedCalibrationSum = 281;
        val calibrationValues = get_sample(2)
            .map { getNumberFromFirstAndLastLDigitsOrSpellingsInString(it) };
        val calibrationSum = calibrationValues.sum();
        assertEquals(expectedCalibrationSum, calibrationSum);
    }

    override fun SolvePart2_Actual() {
        val expectedCalibrationSum = 54078;
        val calibrationValues = get_input()
            .map { getNumberFromFirstAndLastLDigitsOrSpellingsInString(it) };
        val calibrationSum = calibrationValues.sum();
        assertEquals(expectedCalibrationSum, calibrationSum);
    }

    private fun getNumberFromFirstAndLastLDigitsOrSpellingsInString(input: String): Int {
        val spellings = SpelledNumberMap.entries.map { e -> e.toString() };
        val numRegex = Regex("""(\d|${spellings.joinToString("|")})""");
        val matches = mutableListOf<MatchResult?>();
        matches.add(numRegex.matchAt(input, 0));
        while (matches.last() != null){
            matches.add(numRegex.matchAt(input, matches.last()!!.range.first + 1));
        }
        matches.removeLast();
        val firstMatch = matches.first()!!.value;
        val lastMatch = matches.last()!!.value;
        var firstNumber = SpelledNumberMap.entries.find { m -> m.name == firstMatch }?.value?:0;
        if (firstNumber == 0)
            firstNumber = firstMatch.toInt();
        var lastNumber = SpelledNumberMap.entries.find { m -> m.name == lastMatch }?.value?:0;
        if (lastNumber == 0)
            lastNumber = lastMatch.toInt();
        return firstNumber * 10 + lastNumber;
    }

    enum class SpelledNumberMap(val value: Int){
        one(1),
        two(2),
        three(3),
        four(4),
        five(5),
        six(6),
        seven(7),
        eight(8),
        nine(9)
    }
}
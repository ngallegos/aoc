import java.io.File
import kotlin.test.assertEquals

class Day01Tests : TestBase() {
    override fun SolvePart1_Sample() {
        assert(true);
        val expectedCalibrationSum = 142;
        val calibrationValues = javaClass.getResource("sample-01-01.txt")
            ?.readText()
            ?.split("\n")
            ?.map { GetNumberFromFirstAndLastDigitsInString(it) };
        val calibrationSum = calibrationValues?.sum();

        assertEquals(expectedCalibrationSum, calibrationSum);
    }

    private fun GetNumberFromFirstAndLastDigitsInString(input: String) : Int {
        val regex = Regex("""\D""");
        val justNumbers = input.replace(regex, "");
        val charArray = charArrayOf(justNumbers.first(), justNumbers.last());
        var firstAndLast = String(charArray);
        return firstAndLast.toInt();
    }

    override fun SolvePart1_Actual() {

        TODO("Not yet implemented")
    }

    override fun SolvePart2_Sample() {
        TODO("Not yet implemented")
    }

    override fun SolvePart2_Actual() {
        TODO("Not yet implemented")
    }
}
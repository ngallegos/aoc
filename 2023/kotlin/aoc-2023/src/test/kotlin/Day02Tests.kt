import kotlin.math.max
import kotlin.test.assertEquals

class Day02Tests : TestBase() {
    override fun SolvePart1_Sample() {
        // Arrange
        val games = get_sample()
            .map{GetGameStats(it)};
        val expectedSum = 8;

        // Act
        val possibleGames = games
            .filter { x -> x.MaxRed <= _maxRed && x.MaxGreen <= _maxGreen && x.MaxBlue <= _maxBlue };
        val actualSum = possibleGames.sumOf { it.GameNumber };

        // Assert
        assertEquals(expectedSum, actualSum);
    }

    override fun SolvePart1_Actual() {
        // Arrange
        val games = get_input()
            .map{GetGameStats(it)};
        val expectedSum = 2283;

        // Act
        val possibleGames = games
            .filter { x -> x.MaxRed <= _maxRed && x.MaxGreen <= _maxGreen && x.MaxBlue <= _maxBlue };
        val actualSum = possibleGames.sumOf { it.GameNumber };

        // Assert
        assertEquals(expectedSum, actualSum);
    }

    override fun SolvePart2_Sample() {
        // Arrange
        val games = get_sample()
            .map{GetGameStats(it)};
        val expectedSum = 2286;

        // Act
        val actualSum = games.sumOf { it.MinPower };

        // Assert
        assertEquals(expectedSum, actualSum);
    }

    override fun SolvePart2_Actual() {
        // Arrange
        val games = get_input()
            .map{GetGameStats(it)};
        val expectedSum = 78669;

        // Act
        val actualSum = games.sumOf { it.MinPower };

        // Assert
        assertEquals(expectedSum, actualSum);
    }

    private val _maxRed: Int = 12;
    private val _maxGreen: Int = 13;
    private val _maxBlue: Int = 14;

    private fun GetGameStats(gameInput: String): Game{
        val gameRegex = Regex("Game (?<gameNumber>\\d+):(?<handfuls>.*)");
        val cubesRegex = Regex("(?<cubes>\\d+) (?<color>(blue|green|red))");
        val match = gameRegex.find(gameInput);
        var game = Game();
        game.GameNumber = match?.groups!!["gameNumber"]?.value?.toInt()!!;

        val handfuls = match.groups["handfuls"]?.value?.split(';')?: listOf<String>();
        handfuls.forEach {
            handful ->
            val cubeMatches = cubesRegex.findAll(handful);
            cubeMatches.forEach {
                cubeMatch ->
                when (cubeMatch.groups["color"]?.value) {
                    "red" -> game.MaxRed = max(game.MaxRed, cubeMatch.groups["cubes"]?.value?.toInt()?:0);
                    "green" -> game.MaxGreen = max(game.MaxGreen, cubeMatch.groups["cubes"]?.value?.toInt()?:0);
                    "blue" -> game.MaxBlue = max(game.MaxBlue, cubeMatch.groups["cubes"]?.value?.toInt()?:0);
                }
            };
        };

        return game;
    }

    public class Game {
        public var GameNumber: Int = 0;
        public var MaxRed: Int = 0;
        public var MaxGreen: Int = 0;
        public var MaxBlue: Int = 0;
        public val MinPower get() = MaxRed * MaxBlue * MaxGreen;
    }
}
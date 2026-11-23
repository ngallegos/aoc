import kotlin.math.max
import kotlin.test.assertEquals

class Day02Tests : TestBase() {
    override fun solvePart1Sample() {
        // Arrange
        val games = getSample()
            .map{getGameStats(it)}
        val expectedSum = 8

        // Act
        val possibleGames = games
            .filter { x -> x.MaxRed <= _maxRed && x.MaxGreen <= _maxGreen && x.MaxBlue <= _maxBlue }
        val actualSum = possibleGames.sumOf { it.gameNumber }

        // Assert
        assertEquals(expectedSum, actualSum)
    }

    override fun solvePart1Actual() {
        // Arrange
        val games = getInput()
            .map{getGameStats(it)}

        val expectedSum = 2283

        // Act
        val possibleGames = games
            .filter { x -> x.MaxRed <= _maxRed && x.MaxGreen <= _maxGreen && x.MaxBlue <= _maxBlue }
        val actualSum = possibleGames.sumOf { it.gameNumber }

        // Assert
        assertEquals(expectedSum, actualSum)
    }

    override fun solvePart2Sample() {
        // Arrange
        val games = getSample()
            .map{getGameStats(it)}
        val expectedSum = 2286

        // Act
        val actualSum = games.sumOf { it.MinPower }

        // Assert
        assertEquals(expectedSum, actualSum)
    }

    override fun solvePart2Actual() {
        // Arrange
        val games = getInput()
            .map{getGameStats(it)}
        val expectedSum = 78669

        // Act
        val actualSum = games.sumOf { it.MinPower }

        // Assert
        assertEquals(expectedSum, actualSum)
    }

    private val _maxRed: Int = 12
    private val _maxGreen: Int = 13
    private val _maxBlue: Int = 14

    private fun getGameStats(gameInput: String): Game{
        val gameRegex = Regex("Game (?<gameNumber>\\d+):(?<handfuls>.*)")
        val cubesRegex = Regex("(?<cubes>\\d+) (?<color>(blue|green|red))")
        val match = gameRegex.find(gameInput)
        val game = Game()
        game.gameNumber = match?.groups!!["gameNumber"]?.value?.toInt()!!

        val handfuls = match.groups["handfuls"]?.value?.split(';')?: listOf<String>()
        handfuls.forEach {
            handful ->
            val cubeMatches = cubesRegex.findAll(handful)
            cubeMatches.forEach {
                cubeMatch ->
                when (cubeMatch.groups["color"]?.value) {
                    "red" -> game.MaxRed = max(game.MaxRed, cubeMatch.groups["cubes"]?.value?.toInt()?:0)
                    "green" -> game.MaxGreen = max(game.MaxGreen, cubeMatch.groups["cubes"]?.value?.toInt()?:0)
                    "blue" -> game.MaxBlue = max(game.MaxBlue, cubeMatch.groups["cubes"]?.value?.toInt()?:0)
                }
            }
        }

        return game
    }

    class Game {
        var gameNumber: Int = 0
        var MaxRed: Int = 0
        var MaxGreen: Int = 0
        var MaxBlue: Int = 0
        val MinPower get() = MaxRed * MaxBlue * MaxGreen
    }
}
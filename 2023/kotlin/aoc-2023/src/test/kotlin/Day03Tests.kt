import kotlin.test.assertEquals

class Day03Tests : TestBase() {
    override fun solvePart1Sample() {
        // Arrange
        val expectedSum = 4361
        val engineParts = mutableListOf<EnginePart>()
        val lines = getSample().toList()
        lines.forEachIndexed { index, line -> engineParts.addAll(EnginePart.getPartsFromSchematicLine(line, index, lines)) }

        // Act
        val validEngineParts = engineParts.filter { it.isAdjacentToSymbol }
        val actualSum = validEngineParts
            .sumOf { it.partNumber }

        // Assert
        assertEquals(expectedSum, actualSum)
    }

    override fun solvePart1Actual() {
        // Arrange
        val expectedSum = 543867
        val engineParts = mutableListOf<EnginePart>()
        val lines = getInput().toList()
        lines.forEachIndexed { index, line -> engineParts.addAll(EnginePart.getPartsFromSchematicLine(line, index, lines)) }

        // Act
        val validEngineParts = engineParts.filter { it.isAdjacentToSymbol }
        val actualSum = validEngineParts
            .sumOf { it.partNumber }

        // Assert
        assertEquals(expectedSum, actualSum)
    }

    override fun solvePart2Sample() {
        TODO("Not yet implemented")
    }

    override fun solvePart2Actual() {
        TODO("Not yet implemented")
    }

    class EnginePart{
        var partNumber: Int = 0
        var lineNumber: Int = 0
        var isAdjacentToSymbol: Boolean = false

        companion object {
            fun getPartsFromSchematicLine(line: String, lineNumber: Int, lines: List<String>): List<EnginePart>{
                val partNumberRegex = Regex("\\d+")
                val validSymbolRegex = Regex("[^\\d.]")
                val engineParts = partNumberRegex.findAll(line)
                    .filter { it.value.isNotEmpty() }
                    .map {
                        val ep = EnginePart()
                        val possibleSymbolIndices = mutableListOf<Int>()
                        ep.partNumber = it.value.toInt()
                        ep.lineNumber = lineNumber
                        if (it.range.first > 0){
                            if (line[it.range.first - 1] != '.') {
                                ep.isAdjacentToSymbol = true
                                return@map ep
                            }
                            possibleSymbolIndices.add(it.range.first - 1)
                        }
                        possibleSymbolIndices.addAll(it.range)
                        if (it.range.last < line.length - 1) {
                            if (line[it.range.last + 1] != '.'){
                                ep.isAdjacentToSymbol = true
                                return@map  ep
                            }
                            possibleSymbolIndices.add(it.range.last + 1)
                        }

                        if (lineNumber > 0){
                            val prevLineToCheck = lines[lineNumber - 1].substring(possibleSymbolIndices.first(), possibleSymbolIndices.last() + 1)
                            if (prevLineToCheck.any { c -> validSymbolRegex.matches(c.toString()) }){
                                ep.isAdjacentToSymbol = true
                                return@map ep
                            }
                        }

                        if (lineNumber < lines.count() - 1){
                            val nextLineToCheck = lines[lineNumber + 1].substring(possibleSymbolIndices.first(), possibleSymbolIndices.last() + 1)
                            if (nextLineToCheck.any { c -> validSymbolRegex.matches(c.toString()) }){
                                ep.isAdjacentToSymbol = true
                                return@map ep
                            }
                        }

                        return@map ep
                    }.toList()
                return engineParts
            }
        }
    }
}
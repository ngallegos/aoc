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
        // Arrange
        val expectedGearRatioSum = 467835
        val engineParts = mutableListOf<EnginePart>()
        val lines = getSample().toList()
        lines.forEachIndexed { index, line -> engineParts.addAll(EnginePart.getPartsFromSchematicLine(line, index, lines)) }

        // Act
        val validEngineParts = engineParts.filter { it.isAdjacentToGear }
            .groupBy { it.gearLocation }
            .filter { it.value.count() == 2 && it.key.isNotEmpty() }
            .map{
                object {
                    val gearID = it.key
                    val gearRatio = it.value[0].partNumber * it.value[1].partNumber
                }
            }
        val actualSum = validEngineParts
            .sumOf { it.gearRatio }

        // Assert
        assertEquals(expectedGearRatioSum, actualSum)
    }

    override fun solvePart2Actual() {
        // Arrange
        val expectedGearRatioSum = 467835
        val engineParts = mutableListOf<EnginePart>()
        val lines = getInput().toList()
        lines.forEachIndexed { index, line -> engineParts.addAll(EnginePart.getPartsFromSchematicLine(line, index, lines)) }

        // Act
        val validEngineParts = engineParts.filter { it.isAdjacentToGear }
            .groupBy { it.gearLocation }
            .filter { it.value.count() == 2 && it.key.isNotEmpty() }
            .map{
                object {
                    val gearID = it.key
                    val gearRatio = it.value[0].partNumber * it.value[1].partNumber
                }
            }
        val actualSum = validEngineParts
            .sumOf { it.gearRatio }

        // Assert
        assertEquals(expectedGearRatioSum, actualSum)
    }

    class EnginePart{
        var partNumber: Int = 0
        var lineNumber: Int = 0
        var isAdjacentToSymbol: Boolean = false
        var isAdjacentToGear: Boolean = false;
        var gearLocation: String = "";

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
                            val precedingChar = line[it.range.first - 1];
                            if (precedingChar != '.') {
                                ep.isAdjacentToSymbol = true
                                if (precedingChar == '*') // gear
                                {
                                    ep.isAdjacentToGear = true;
                                    ep.gearLocation = "${lineNumber}-${it.range.last - 1}";
                                }
                                return@map ep
                            }
                            possibleSymbolIndices.add(it.range.first - 1)
                        }
                        possibleSymbolIndices.addAll(it.range)
                        if (it.range.last < line.length - 1) {
                            val succeedingChar = line[it.range.last + 1];
                            if (line[it.range.last + 1] != '.'){
                                ep.isAdjacentToSymbol = true
                                if (succeedingChar == '*') // gear
                                {
                                    ep.isAdjacentToGear = true;
                                    ep.gearLocation = "${lineNumber}-${it.range.last + 1}";
                                }
                                return@map  ep
                            }
                            possibleSymbolIndices.add(it.range.last + 1)
                        }

                        if (lineNumber > 0){
                            val prevLineToCheck = lines[lineNumber - 1].substring(possibleSymbolIndices.first(), possibleSymbolIndices.last() + 1)
                            val symbolChar = prevLineToCheck.find { c-> validSymbolRegex.matches(c.toString()) }
                            if (symbolChar != null){
                                ep.isAdjacentToSymbol = true
                                if (symbolChar == '*') // gear
                                {
                                    ep.isAdjacentToGear = true;
                                    ep.gearLocation = "${lineNumber - 1}-${lines[lineNumber - 1].indexOf(symbolChar)}";
                                }
                                return@map ep
                            }
                        }

                        if (lineNumber < lines.count() - 1){
                            val nextLineToCheck = lines[lineNumber + 1].substring(possibleSymbolIndices.first(), possibleSymbolIndices.last() + 1)
                            val symbolChar = nextLineToCheck.find { c-> validSymbolRegex.matches(c.toString()) }
                            if (symbolChar != null){
                                ep.isAdjacentToSymbol = true
                                if (symbolChar == '*') // gear
                                {
                                    ep.isAdjacentToGear = true;
                                    ep.gearLocation = "${lineNumber + 1}-${lines[lineNumber + 1].indexOf(symbolChar)}";
                                }
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
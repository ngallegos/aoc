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
        val validEngineParts = engineParts.filter { it.isAdjacentToGear }.asSequence();
        val gears = validEngineParts.flatMap { it.gearLocations }.distinct();

        var gearRatioSum = 0;
        gears.forEach { gear ->
            val parts = validEngineParts.filter { it.gearLocations.contains(gear) }.toList()
            if (parts.count() == 2)
                gearRatioSum += parts[0].partNumber * parts[1].partNumber
        }

        // Assert
        assertEquals(expectedGearRatioSum, gearRatioSum)
    }

    override fun solvePart2Actual() {
        // Arrange
        val expectedGearRatioSum = 467835
        val engineParts = mutableListOf<EnginePart>()
        val lines = getInput().toList()
        lines.forEachIndexed { index, line -> engineParts.addAll(EnginePart.getPartsFromSchematicLine(line, index, lines)) }

        // Act
        val validEngineParts = engineParts.filter { it.isAdjacentToGear }.asSequence();
        val gears = validEngineParts.flatMap { it.gearLocations }.distinct();

        var gearRatioSum = 0;
        gears.forEach { gear ->
            val parts = validEngineParts.filter { it.gearLocations.contains(gear) }.toList()
            if (parts.count() == 2)
                gearRatioSum += parts[0].partNumber * parts[1].partNumber
        }

        // Assert
        assertEquals(expectedGearRatioSum, gearRatioSum)
    }

    class EnginePart{
        var partNumber: Int = 0
        var lineNumber: Int = 0
        var isAdjacentToSymbol: Boolean = false
        var isAdjacentToGear: Boolean = false
        var gearLocations: MutableList<String> = mutableListOf()

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
                            val precedingChar = line[it.range.first - 1]
                            if (precedingChar != '.') {
                                ep.isAdjacentToSymbol = true
                                if (precedingChar == '*') // gear
                                {
                                    ep.isAdjacentToGear = true
                                    ep.gearLocations.add("${lineNumber}-${it.range.last - 1}")
                                }
                            }
                            possibleSymbolIndices.add(it.range.first - 1)
                        }
                        possibleSymbolIndices.addAll(it.range)
                        if (it.range.last < line.length - 1) {
                            val succeedingChar = line[it.range.last + 1]
                            if (line[it.range.last + 1] != '.'){
                                ep.isAdjacentToSymbol = true
                                if (succeedingChar == '*') // gear
                                {
                                    ep.isAdjacentToGear = true
                                    ep.gearLocations.add("${lineNumber}-${it.range.last + 1}")
                                }
                                return@map  ep
                            }
                            possibleSymbolIndices.add(it.range.last + 1)
                        }

                        if (lineNumber > 0){
                            val prevLine = lines[lineNumber - 1]
                            val prevLineToCheck = prevLine.substring(possibleSymbolIndices.first(), possibleSymbolIndices.last() + 1)
                            prevLineToCheck.forEachIndexed{ index, c ->
                                if (validSymbolRegex.matches(c.toString())){
                                    ep.isAdjacentToSymbol = true;
                                    if (c == '*'){
                                        ep.isAdjacentToGear = true;
                                        ep.gearLocations.add("${lineNumber - 1}-${possibleSymbolIndices.first() + index}")
                                    }
                                }
                            }
                        }

                        if (lineNumber < lines.count() - 1){
                            val nextLine = lines[lineNumber + 1]
                            val nextLineToCheck = nextLine.substring(possibleSymbolIndices.first(), possibleSymbolIndices.last() + 1)
                            nextLineToCheck.forEachIndexed{ index, c ->
                                if (validSymbolRegex.matches(c.toString())){
                                    ep.isAdjacentToSymbol = true;
                                    if (c == '*'){
                                        ep.isAdjacentToGear = true;
                                        ep.gearLocations.add("${lineNumber + 1}-${possibleSymbolIndices.first() + index}")
                                    }
                                }
                            }
                        }

                        return@map ep
                    }.toList()
                return engineParts
            }
        }
    }
}
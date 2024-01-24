class Day03Tests : TestBase() {
    override fun SolvePart1_Sample() {
        // Arrange
        val engineParts = mutableListOf<EnginePart>();
        get_sample().forEachIndexed { index, line -> engineParts.addAll(EnginePart.GetPartsFromSchematicLine(line, index)) }

        // Act


        // Assert
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

    class EnginePart{
        public var PartNumber: Int = 0;
        public var LineNumber: Int = 0;
        public var PossibleSymbolIndices: MutableList<Int> = mutableListOf();
        companion object {
            fun GetPartsFromSchematicLine(line: String, lineNumber: Int): List<EnginePart>{
                val partNumberRegex = Regex("\\d+");
                val engineParts = partNumberRegex.findAll(line)
                    .filter { it.value.isNotEmpty() }
                    .map {
                        val ep = EnginePart();
                        ep.PartNumber = it.value.toInt();
                        ep.LineNumber = lineNumber;
                        if (it.range.first > 0)
                            ep.PossibleSymbolIndices.add(it.range.first - 1);
                        ep.PossibleSymbolIndices.addAll(it.range);
                        if (it.range.last < line.length - 1)
                            ep.PossibleSymbolIndices.add(it.range.last + 1);
                        return@map ep;
                    }.toList();
                return engineParts;
            }
        }
    }
}
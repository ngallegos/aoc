import org.opentest4j.TestAbortedException
import kotlin.test.Test
import kotlin.test.fail
import kotlin.time.measureTime

abstract class TestBase {
    protected abstract fun solvePart1Sample()
    protected abstract fun solvePart1Actual()
    protected abstract fun solvePart2Sample()
    protected abstract fun solvePart2Actual()

    @Test
    fun part1Sample() = execute("Part 1 sample") { solvePart1Sample() }

    @Test
    fun part1Actual() = execute("Part 1 actual") { solvePart1Actual() }

    @Test
    fun part2Sample() = execute("Part 2 sample") { solvePart2Sample() }

    @Test
    fun part2Actual() = execute("Part 2 actual") { solvePart2Actual() }

    private fun execute(label: String, solve: () -> Unit) {
        val timeTaken = measureTime {
            try {
                solve()
            } catch (e: TestAbortedException) {
                throw e
            } catch (e: Error){
                fail(e.message)
            } catch (e: Exception){
                fail(e.message)
            }
        }
        println("$label:\t${timeTaken}")
    }

    protected fun getSample(partNumber: Int = 1) : Iterable<String>
    {
        return getInput(partNumber = partNumber, type = "sample")
    }
    protected fun getInput(partNumber: Int = 1, type: String = "day"): Iterable<String>{
        val lines = mutableListOf<String>()
        val timeTaken = measureTime {
            val dayNumber = javaClass.name
                .replace("Day", "")
                .replace("Tests", "")
            lines.addAll(javaClass.getResource("${type}-${dayNumber}-${"%02d".format(partNumber)}.txt")
                ?.readText()
                ?.split("\n") ?: listOf<String>())
        }
        println("Input parsing:\t${timeTaken}")
        return lines
    }
}
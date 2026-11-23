import kotlin.test.Test
import kotlin.test.fail
import kotlin.time.measureTime

abstract class TestBase {
    protected abstract fun solvePart1Sample()
    protected abstract fun solvePart1Actual()
    protected abstract fun solvePart2Sample()
    protected abstract fun solvePart2Actual()

    @Test
    fun part1Sample(){
        val timeTaken = measureTime {
            try {
                solvePart1Sample()
            } catch (e: Error){
                fail(e.message)
            } catch (e: Exception){
                fail(e.message)
            }
        }
        println("Part 1 sample:\t${timeTaken}")
    }

    @Test
    fun part1Actual(){
        val timeTaken = measureTime {
            try {
                solvePart1Actual()
            } catch (e: Error){
                fail(e.message)
            } catch (e: Exception){
                fail(e.message)
            }
        }
        println("Part 1 actual:\t${timeTaken}")
    }

    @Test
    fun part2Sample(){
        val timeTaken = measureTime {
            try {
                solvePart2Sample()
            } catch (e: Error){
                fail(e.message)
            } catch (e: Exception){
                fail(e.message)
            }
        }
        println("Part 2 sample:\t${timeTaken}")
    }

    @Test
    fun part2Actual(){
        val timeTaken = measureTime {
            try {
                solvePart2Actual()
            } catch (e: Error){
                fail(e.message)
            } catch (e: Exception){
                fail(e.message)
            }
        }
        println("Part 2 actual:\t${timeTaken}")
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
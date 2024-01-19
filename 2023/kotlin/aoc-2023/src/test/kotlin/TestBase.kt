import kotlin.test.Test
import kotlin.test.assertFails
import kotlin.test.fail
import kotlin.time.measureTime

abstract class TestBase {
    protected abstract fun SolvePart1_Sample();
    protected abstract fun SolvePart1_Actual();
    protected abstract fun SolvePart2_Sample();
    protected abstract fun SolvePart2_Actual();

    @Test
    public fun Part1_Sample(){
        val timeTaken = measureTime {
            try {
                SolvePart1_Sample();
            } catch (e: Error){
                fail(e.message);
            } catch (e: Exception){
                fail(e.message);
            }
        }
        println("Part 1 sample:\t${timeTaken}");
    }

    @Test
    public fun Part1_Actual(){
        val timeTaken = measureTime {
            try {
                SolvePart1_Actual();
            } catch (e: Error){
                fail(e.message);
            } catch (e: Exception){
                fail(e.message);
            }
        }
        println("Part 1 actual:\t${timeTaken}");
    }

    @Test
    public fun Part2_Sample(){
        val timeTaken = measureTime {
            try {
                SolvePart2_Sample();
            } catch (e: Error){
                fail(e.message);
            } catch (e: Exception){
                fail(e.message);
            }
        }
        println("Part 2 sample:\t${timeTaken}");
    }

    @Test
    public fun Part2_Actual(){
        val timeTaken = measureTime {
            try {
                SolvePart2_Actual();
            } catch (e: Error){
                fail(e.message);
            } catch (e: Exception){
                fail(e.message);
            }
        }
        println("Part 2 actual:\t${timeTaken}");
    }
    protected fun get_sample(partNumber: Int = 1) : Iterable<String>
    {
        return get_input(partNumber = partNumber, type = "sample");
    }
    protected fun get_input(partNumber: Int = 1, type: String = "day"): Iterable<String>{
        val lines = mutableListOf<String>();
        val timeTaken = measureTime {
            val dayNumber = javaClass.name
                .replace("Day", "")
                .replace("Tests", "");
            lines.addAll(javaClass.getResource("${type}-${dayNumber}-${"%02d".format(partNumber)}.txt")
                ?.readText()
                ?.split("\n") ?: listOf<String>());
        };
        println("Input parsing:\t${timeTaken}")
        return lines;
    }
}
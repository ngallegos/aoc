using System.Security.Cryptography;
using System.Text;

namespace AOC2015.Tests;

public class Day04Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var secretKey = get_sample().First();

        // Act
        var (_, lowestNumber) = FindAdventCoinHash(secretKey, "00000");

        // Assert
        lowestNumber.ShouldBe(609043);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var secretKey = get_input().First();

        // Act
        var (_, lowestNumber) = FindAdventCoinHash(secretKey, "00000");

        // Assert
        lowestNumber.ShouldBe(282749);
    }

    [Ignore("No sample")]
    protected override void SolvePart2_Sample()
    {
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var secretKey = get_input().First();

        // Act
        var (_, lowestNumber) = FindAdventCoinHash(secretKey, "000000");

        // Assert
        lowestNumber.ShouldBe(9962624);
    }

    private (string hash, long lowestNumber) FindAdventCoinHash(string secretKey, string hashPrefix)
    {
        var currentNumber = 0;
        var hash = ComputeMD5Hash($"{secretKey}{currentNumber}");
        while (!hash.StartsWith(hashPrefix))
        {
            currentNumber++;
            hash = ComputeMD5Hash($"{secretKey}{currentNumber}");
        }

        return (hash, currentNumber);
    }
    private MD5 _md5 = MD5.Create();

    private string ComputeMD5Hash(string secretKey)
    {
        var hashBytes = _md5.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
        return string.Join("", hashBytes.Select(x => x.ToString("X2")));
    }
}

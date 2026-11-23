using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AOC2015.Modules;

public class Day04 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var (hash, lowestNumber) = FindAdventCoinHash("yzbqklnj", "00000");

        return new
        {
            hash,
            lowestNumber
        };
    }

    public override dynamic Part2()
    {
        var (hash, lowestNumber) = FindAdventCoinHash("yzbqklnj", "000000");

        return new
        {
            hash,
            lowestNumber
        };
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AOC2022.Modules;

public class Day13 : DayBase
{
    public override bool Completed { get; }
    public override dynamic Part1()
    {
        var sample = get_sample().ToList();
        var samplePacketPairs = new List<PacketPair>();
        for (int i = 0; i < sample.Count; i += 3)
        {
            samplePacketPairs.Add(new PacketPair(i / 3 + 1, sample[i], sample[i+1]));
        }
        var correctlyOrderedPairSumSample = samplePacketPairs.Where(x => x.OrderedCorrectly).Sum(x => x.PairNumber);
        
        var actual = get_input().ToList();
        var actualPacketPairs = new List<PacketPair>();
        for (int i = 0; i < actual.Count; i += 3)
        {
            actualPacketPairs.Add(new PacketPair(i / 3 + 1, actual[i], actual[i+1]));
        }
        var correctlyOrderedPairSumActual = actualPacketPairs.Where(x => x.OrderedCorrectly).Sum(x => x.PairNumber);

        return new
        {
            correctlyOrderedPairSumSample,
            correctlyOrderedPairSumActual
        };
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class PacketPair
    {
        public int PairNumber { get; }
        private JsonArray _leftPacket;
        private JsonArray _rightPacket;
        private bool _orderedCorrectly;

        public PacketPair(int pairNumber, string leftPacket, string rightPacket)
        {
            PairNumber = pairNumber;
            _leftPacket = JsonSerializer.Deserialize<JsonArray>(leftPacket);
            _rightPacket = JsonSerializer.Deserialize<JsonArray>(rightPacket);
            Console.WriteLine($"== Pair {pairNumber} ==");
            _orderedCorrectly = ValidateArray(_leftPacket, _rightPacket) ?? false;
            Console.WriteLine("\n");
        }

        private bool? ValidateArray(JsonArray left, JsonArray right, string indent = "")
        {
            Console.WriteLine($"{indent}- Compare {left.ToJsonString()} vs {right.ToJsonString()}");
            bool? valid = null;
            if (!left.Any() && right.Any()) {
                Console.WriteLine($"{indent}  - Left side ran out of items, so inputs are in the CORRECT ORDER");
                return true; // left side ran out of items, so CORRECT ORDER
            }
            for (int i = 0; i < left.Count; i++)
            {
                if (right.Count < i + 1)
                {
                    Console.WriteLine($"{indent}  - Right side ran out of items, so inputs are in the INCORRECT ORDER");
                    return false; // right side ran out of items, so INCORRECT ORDER
                }

                var lI = left[i];
                var rI = right[i];
                var lArray = lI as JsonArray;
                var rArray = rI as JsonArray;
                if (lArray != null && rArray != null)
                    valid = ValidateArray(lArray, rArray, $"{indent}  ");
                else if (lArray != null)
                {
                    Console.WriteLine($"{indent}  - Mixed types; convert right to [{rI.ToJsonString()}] and retry comparison");
                    valid = ValidateArray(lArray, new JsonArray(rI.GetValue<int>()), $"{indent}  ");
                }
                else if (rArray != null)
                {
                    Console.WriteLine($"{indent}  - Mixed types; convert left to [{lI.ToJsonString()}] and retry comparison");
                    valid = ValidateArray(new JsonArray(lI.GetValue<int>()), rArray,$"{indent}  ");
                }
                else
                {
                    var lInt = lI.GetValue<int>();
                    var rInt = rI.GetValue<int>();

                    Console.WriteLine($"{indent}  - Compare {lInt} vs {rInt}");
                    if (lInt != rInt)
                    {
                        var result = lInt < rInt;
                        if (result)
                            Console.WriteLine($"{indent}    - Left side is smaller, so inputs are in the CORRECT ORDER");
                        else
                            Console.WriteLine($"{indent}    - Right side is smaller, so inputs are in the INCORRECT ORDER");

                        valid = result;
                    }
                }

                if (valid.HasValue)
                    return valid;
            }

            if (left.Count != right.Count)
            {
                Console.WriteLine($"{indent}  - Left side ran out of items, so inputs are in the CORRECT ORDER");
                return true;
            }

            return valid;
        }

        public bool OrderedCorrectly => _orderedCorrectly;
    }
}
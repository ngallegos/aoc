using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AOC2022.Modules;

public class Day13 : DayBase
{
    public override bool Completed { get; }
    private static bool LogSteps = true;
    private readonly PacketProcessor _sampleProcessor;
    private readonly PacketProcessor _actualProcessor;

    public Day13()
    {
        _sampleProcessor = new PacketProcessor(get_sample("Part1").ToList());
        LogSteps = false;
        _actualProcessor = new PacketProcessor(get_input("Part1").ToList());
    }
    
    public override dynamic Part1()
    {
        return new
        {
            sample = _sampleProcessor.CorrectlyOrderedPairSum,
            actual = _actualProcessor.CorrectlyOrderedPairSum
        };
    }

    public override dynamic Part2()
    {
        return new
        {
            sample = _sampleProcessor.DecoderKey,
            actual = _actualProcessor.DecoderKey
        };
    }

    private class PacketProcessor
    {
        private List<PacketPair> _packetPairs = new List<PacketPair>();
        private List<string> _allPackets = new List<string>();

        public int CorrectlyOrderedPairSum => _packetPairs.Where(x => x.OrderedCorrectly)
            .Sum(x => x.PairNumber);

        private int _decoderKey;
        public int DecoderKey => _decoderKey;
        public List<string> OrderedPackets => _allPackets;

        public PacketProcessor(List<string> input)
        {
            for (int i = 0; i < input.Count; i += 3)
            {
                _packetPairs.Add(new PacketPair(i / 3 + 1, input[i], input[i+1]));
                _allPackets.Add(input[i]);
                _allPackets.Add(input[i+1]);
            }

            _allPackets.Add("[[2]]");
            _allPackets.Add("[[6]]");

            var comparer = new PacketComparer();
            _allPackets.Sort(comparer);
            _decoderKey = (_allPackets.FindIndex(x => x == "[[2]]") + 1) *
                          (_allPackets.FindIndex(x => x == "[[6]]") + 1);
        }
    }
    
    private class PacketComparer : IComparer<string>
    {
        public bool LogOutput { get; set; }
        public int Compare(string x, string y)
        {
            var left = JsonSerializer.Deserialize<JsonArray>(x);
            var right = JsonSerializer.Deserialize<JsonArray>(y);

            return Compare(left, right);
        }

        public void WriteLine(string line)
        {
            if (LogOutput)
                Console.WriteLine(line);
        }
        
        private int Compare(JsonArray left, JsonArray right, string indent = null)
        {
            WriteLine($"{indent}- Compare {left.ToJsonString()} vs {right.ToJsonString()}");
            var comparison = 0;
            if (!left.Any() && right.Any()) {
                WriteLine($"{indent}  - Left side ran out of items, so inputs are in the CORRECT ORDER");
                return -1; // left side ran out of items, so CORRECT ORDER
            }
            for (int i = 0; i < left.Count; i++)
            {
                if (right.Count < i + 1)
                {
                    WriteLine($"{indent}  - Right side ran out of items, so inputs are in the INCORRECT ORDER");
                    return 1; // right side ran out of items, so INCORRECT ORDER
                }

                var lI = left[i];
                var rI = right[i];
                var lArray = lI as JsonArray;
                var rArray = rI as JsonArray;
                if (lArray != null && rArray != null)
                    comparison = Compare(lArray, rArray, $"{indent}  ");
                else if (lArray != null)
                {
                    WriteLine($"{indent}  - Mixed types; convert right to [{rI.ToJsonString()}] and retry comparison");
                    comparison = Compare(lArray, new JsonArray(rI.GetValue<int>()), $"{indent}  ");
                }
                else if (rArray != null)
                {
                    WriteLine($"{indent}  - Mixed types; convert left to [{lI.ToJsonString()}] and retry comparison");
                    comparison = Compare(new JsonArray(lI.GetValue<int>()), rArray,$"{indent}  ");
                }
                else
                {
                    var lInt = lI.GetValue<int>();
                    var rInt = rI.GetValue<int>();

                    WriteLine($"{indent}  - Compare {lInt} vs {rInt}");
                    if (lInt != rInt)
                    {
                        var result = lInt < rInt;
                        if (result)
                            WriteLine($"{indent}    - Left side is smaller, so inputs are in the CORRECT ORDER");
                        else
                            WriteLine($"{indent}    - Right side is smaller, so inputs are in the INCORRECT ORDER");

                        comparison = result ? -1 : 1;
                    }
                }

                if (comparison != 0)
                    return comparison;
            }

            if (left.Count != right.Count)
            {
                WriteLine($"{indent}  - Left side ran out of items, so inputs are in the CORRECT ORDER");
                return -1;
            }

            return comparison;
        }
    }

    private class PacketPair
    {
        public int PairNumber { get; }
        private bool _orderedCorrectly;
        public bool OrderedCorrectly => _orderedCorrectly;

        public PacketPair(int pairNumber, string leftPacket, string rightPacket)
        {
            var comparer = new PacketComparer();
            comparer.LogOutput = LogSteps;
            PairNumber = pairNumber;
            comparer.WriteLine($"== Pair {pairNumber} ==");
            var result = comparer.Compare(leftPacket, rightPacket);
            _orderedCorrectly = result < 0;
            comparer.WriteLine("\n");
        }
    }
}
using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day05Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var almanacInput = get_sample().ToList();
        var almanac = new Almanac(almanacInput);
        
        almanac.LowestLocationNumber.ShouldBe(35L);
    }

    protected override void SolvePart2_Sample()
    {
        var almanacInput = get_sample().ToList();
        var almanac = new Almanac(almanacInput, false);
        
        almanac.LowestLocationNumber.ShouldBe(46);
    }

    protected override void SolvePart1_Actual()
    {
        var almanacInput = get_input().ToList();
        var almanac = new Almanac(almanacInput);
        
        almanac.LowestLocationNumber.ShouldBe(535088217L);
    }

    protected override void SolvePart2_Actual()
    {
        var almanacInput = get_input().ToList();
        var almanac = new Almanac(almanacInput, false);
        
        almanac.LowestLocationNumber.ShouldBe(46);
    }


    private class Almanac
    {
        private readonly List<ResourceMap> _resourceMaps = new ();
        public long LowestLocationNumber { get; private set; }

        public Almanac(List<string> almanac, bool part1 = true)
        {
            var seedValues = almanac[0].Replace("seeds: ", "").Split(' ')
                .Select(long.Parse).ToList();
            
            if (part1)
                _resourceMaps.Add(new ResourceMap("seed", seedValues.Select(x => (x,x)).ToList()));
            else
            {
                var seedRanges = new List<(long start, long end)>();
                for (int i = 0; i < seedValues.Count; i += 2)
                    seedRanges.Add((seedValues[i], seedValues[i] + seedValues[i+1] - 1));
                _resourceMaps.Add(new ResourceMap("seed", seedRanges));
            }
            
            Initialize(almanac);
        }
        
        private void Initialize(List<string> almanac)
        {
            var currentMappingBlock = new List<string>();
        
            foreach (var almanacEntry in almanac.Skip(1))
            {
                if (string.IsNullOrEmpty(almanacEntry))
                {
                    if (currentMappingBlock.Any())
                        _resourceMaps.Add(new ResourceMap(currentMappingBlock));
                    currentMappingBlock.Clear();
                    continue;
                }
                currentMappingBlock.Add(almanacEntry);
            }
            _resourceMaps.Add(new ResourceMap(currentMappingBlock));
            LowestLocationNumber = long.MaxValue;
        
            // helpful - looks similar to what I'm trying to do, just can't wrap my head around it yet
            // https://github.com/warriordog/advent-of-code-2023/blob/main/Solutions/Day05/Almanac.cs
            // foreach (var seed in _seeds)
            // {
            //     var currentLocation = FindMap(seed, "seed", "location");
            //     if (currentLocation < LowestLocationNumber)
            //         LowestLocationNumber = currentLocation;
            // }
        }
        
        
        private long FindMap(long source, string sourceResourceName, string destResourceName)
        {
            var map = _resourceMaps.First(x => x.Source == sourceResourceName);
            var destination = map.GetDestinationValue(source);
            if (map.Destination == destResourceName)
                return destination;
            return FindMap(destination, map.Destination, destResourceName);
        }
    }
    

    private class ResourceMap
    {
        private static Regex _mapNameRegex = new Regex(@"(?<source>\w+)-to-(?<dest>\w+)", RegexOptions.Compiled);
        public string Source { get; private set; }
        public string Destination { get; private set; }
        public List<(long start, long end)> DestinationMapping { get; private set; }
        public List<(long start, long end)> SourceMapping { get; private set; }

        public ResourceMap (string destName, List<(long start, long end)> destMap)
        {
            Source = string.Empty;
            Destination = destName;
            SourceMapping = new List<(long start, long end)>();
            DestinationMapping = new List<(long start, long end)>();
            DestinationMapping.AddRange(destMap);
        }
        public ResourceMap(List<string> definition)
        {
            var mapNameMatch = _mapNameRegex.Match(definition[0]);
            Source = mapNameMatch.Groups["source"].Value;
            Destination = mapNameMatch.Groups["dest"].Value;
            DestinationMapping = new List<(long start, long end)>();
            SourceMapping = new List<(long start, long end)>();
            foreach (var definitionLine in definition.Skip(1))
            {
                var definitionParts = definitionLine.Split(' ').Select(long.Parse).ToList();
                DestinationMapping.Add((definitionParts[0], definitionParts[0] + definitionParts[2] - 1));
                SourceMapping.Add((definitionParts[1], definitionParts[1] + definitionParts[2] - 1));
            }
        }

        public long GetDestinationValue(long sourceValue)
        {
            foreach(var item in SourceMapping.Select((m, i) => (m, i)))
            {
                var sourceMap = item.m;
                if (sourceValue >= sourceMap.start && sourceValue <= sourceMap.end)
                {
                    var destinationMap = DestinationMapping[item.i];
                    return destinationMap.start + (sourceValue - sourceMap.start);
                }
            }

            return sourceValue;
        }
        
        public bool ContainsDestinationValue(long destinationValue)
        {
            return DestinationMapping.Any(x => x.start <= destinationValue && x.end >= destinationValue);
        }
    }
}
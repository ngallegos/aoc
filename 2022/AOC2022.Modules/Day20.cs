using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day20 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        var sampleSequence = new Sequence(get_sample());

        sampleSequence.UnMix();
        
        return sampleSequence.ToString();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private class Sequence
    {
        private int[] _initial;
        private List<Node> _sequence;
        public Sequence(IEnumerable<string> numbers)
        {
            _initial = numbers.Select(int.Parse).ToArray();
            _sequence = _initial.Select((n, i) => new Node(n, i)).ToList();
        }

        public void UnMix()
        {
            Console.WriteLine(this.ToString());
            for(int i = 0; i < _initial.Length; i++)
            {
                MoveNumber(i);
                Console.WriteLine(this.ToString());
            }
        }

        public override string ToString()
        {
            return string.Join(", ", _sequence.Select(x => x.Value));
        }

        private void MoveNumber(int index)
        {
            var num = _sequence.Single(x => x.ID == index);
            var currentIndex = _sequence.IndexOf(num);

            _sequence.Remove(num);
            var lastIndex = _sequence.Count - 1;
            var direction = num.Value > 0 ? 1 : - 1;
            var adjustedDistance = direction * (Math.Abs(num.Value) % (lastIndex + 1));
            
            var newIndex = adjustedDistance + currentIndex;
            if (newIndex < 0)
                newIndex = lastIndex + newIndex + 1;
            else if (newIndex > lastIndex)
            {
                newIndex = newIndex - lastIndex - 1;
            }
            _sequence.Insert(newIndex, num);
        }
    }

    private class Node
    {
        public int Value { get; }
        public int ID { get; }

        public Node(int value, int index)
        {
            Value = value;
            ID = index;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace AOC2015.Modules;

public class Day03 : DayBase
{
    public override bool Ignore => true;
    public override dynamic Part1()
    {
        var instructions = get_input().First();
        var deliveryState = new PresentDeliveryState();
        foreach (var instruction in instructions)
        {
            deliveryState.Navigate(instruction);
        }

        return new { deliveryState };
    }

    public override dynamic Part2()
    {
        var instructions = get_input().First();
        var santaDeliveryState = new PresentDeliveryState();
        var roboSantaDeliveryState = new PresentDeliveryState();
        for (int i = 0; i < instructions.Length; i += 2)
        {
            santaDeliveryState.Navigate(instructions[i]);
            roboSantaDeliveryState.Navigate(instructions[i+1]);
        }

        var santaDeliveries = santaDeliveryState.GetCurrentDeliveries();
        var roboSantaDeliveries = roboSantaDeliveryState.GetCurrentDeliveries();
        var totalHousesDeliveredTo = santaDeliveries.UnionBy(roboSantaDeliveries, house => house.ToString())
            .DistinctBy(house => house.ToString())
            .ToList();
        
        return new
        {
            santaDeliveryState,
            roboSantaDeliveryState,
            totalHousesDeliveredTo = totalHousesDeliveredTo.Count
        };
    }

    private class PresentDeliveryState
    {
        private List<House> _deliveries = new List<House>();
        private House _currentDelivery;
        public int HousesDeliveredTo => _deliveries.Count;
        public int PresentsDelivered => _deliveries.Sum(x => x.PresentsReceived);
        public PresentDeliveryState()
        {
            _currentDelivery = new House(0, 0);
            _currentDelivery.ReceivePresent();
            _deliveries.Add(_currentDelivery);
        }

        public List<House> GetCurrentDeliveries()
        {
            return _deliveries;
        }

        public void Navigate(char instruction)
        {
            int x = _currentDelivery.X;
            int y = _currentDelivery.Y;
            switch (instruction)
            {
                case '<':
                    x--;
                    break;
                case '>':
                    x++;
                    break;
                case '^':
                    y--;
                    break;
                case 'v':
                    y++;
                    break;
            }

            _currentDelivery = _deliveries.FirstOrDefault(d => d.X == x && d.Y == y);
            if (_currentDelivery == null)
            {
                _currentDelivery = new House(x, y);
                _deliveries.Add(_currentDelivery);
            }
            _currentDelivery.ReceivePresent();
        }
    }

    private class House
    {
        private int _xLocation;
        private int _yLocation;
        private int _presentsReceived;
        public int X => _xLocation;
        public int Y => _yLocation;
        public int PresentsReceived => _presentsReceived;

        public House(int x, int y)
        {
            _xLocation = x;
            _yLocation = y;
            _presentsReceived = 0;
        }

        public void ReceivePresent()
        {
            _presentsReceived++;
        }

        public override string ToString()
        {
            return $"({_xLocation},{_yLocation})";
        }
    }
}
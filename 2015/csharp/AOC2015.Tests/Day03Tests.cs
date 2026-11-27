namespace AOC2015.Tests;

public class Day03Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var instructions = get_sample().First();

        // Act
        var housesDeliveredTo = DeliverAlone(instructions);

        // Assert
        housesDeliveredTo.ShouldBe(2);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var instructions = get_input().First();

        // Act
        var housesDeliveredTo = DeliverAlone(instructions);

        // Assert
        housesDeliveredTo.ShouldBe(2592);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var instructions = get_sample().First();

        // Act
        var housesDeliveredTo = DeliverWithRoboSanta(instructions);

        // Assert
        housesDeliveredTo.ShouldBe(11);
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var instructions = get_input().First();

        // Act
        var housesDeliveredTo = DeliverWithRoboSanta(instructions);

        // Assert
        housesDeliveredTo.ShouldBe(2360);
    }

    private static int DeliverAlone(string instructions)
    {
        var deliveryState = new PresentDeliveryState();
        foreach (var instruction in instructions)
        {
            deliveryState.Navigate(instruction);
        }

        return deliveryState.HousesDeliveredTo;
    }

    private static int DeliverWithRoboSanta(string instructions)
    {
        var santaDeliveryState = new PresentDeliveryState();
        var roboSantaDeliveryState = new PresentDeliveryState();
        for (var i = 0; i < instructions.Length; i += 2)
        {
            santaDeliveryState.Navigate(instructions[i]);
            roboSantaDeliveryState.Navigate(instructions[i + 1]);
        }

        return santaDeliveryState.GetCurrentDeliveries()
            .UnionBy(roboSantaDeliveryState.GetCurrentDeliveries(), house => house.ToString())
            .Count();
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

            var house = _deliveries.FirstOrDefault(d => d.X == x && d.Y == y);
            if (house == null)
            {
                house = new House(x, y);
                _deliveries.Add(house);
            }
            _currentDelivery = house;
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

namespace AOC2025.Tests;

public class Day08Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        // Arrange
        var junctionBoxes = get_sample((location) =>
            {
                var coords = location.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                return new JunctionBox(coords[0], coords[1], coords[2]);
            })
            .ToList();
        
        var circuits = ProcessJunctionBoxes(junctionBoxes, connectionsToMake: 10);
        
        
        // Act
        var result = circuits.OrderByDescending(x => x.Count).Take(3).Aggregate(1L, (accumulator, circuit) => accumulator * circuit.Count);
        
        // Assert
        result.ShouldBe(40L);
    }

    protected override void SolvePart1_Actual()
    {
        // Arrange
        var junctionBoxes = get_input((location) =>
            {
                var coords = location.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                return new JunctionBox(coords[0], coords[1], coords[2]);
            })
            .ToList();
        
        var circuits = ProcessJunctionBoxes(junctionBoxes, connectionsToMake: 1000);
        
        
        // Act
        var result = circuits.OrderByDescending(x => x.Count).Take(3).Aggregate(1L, (accumulator, circuit) => accumulator * circuit.Count);
        
        // Assert
        result.ShouldBe(352584L);
    }

    protected override void SolvePart2_Sample()
    {
        // Arrange
        var _ = get_sample().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    protected override void SolvePart2_Actual()
    {
        // Arrange
        var _ = get_input().ToList();
        
        // Act
        
        // Assert
        throw new System.NotImplementedException();
    }

    List<Circuit> ProcessJunctionBoxes(List<JunctionBox> junctionBoxes, int connectionsToMake = 10)
    {
        var allConnections = new List<JunctionBoxConnection>();
        for (var i = 0; i < junctionBoxes.Count; i++)
        {
            for (var j = i + 1; j < junctionBoxes.Count; j++)
            {
                var connection = new JunctionBoxConnection(junctionBoxes[i], junctionBoxes[j]);
                allConnections.Add(connection);
            }
        }

        allConnections = allConnections.OrderBy(x => x.Distance).ToList();
        
        
        var circuits = new List<Circuit>();

        for (var i = 0; i < connectionsToMake; i++)
        {
            var connection = allConnections[i];
            var existingCircuits = circuits
                .Where(c => c.Contains(connection.BoxA) || c.Contains(connection.BoxB))
                .ToList();
            if (existingCircuits.Any())
            {
                circuits.RemoveAll(c => c.Contains(connection.BoxA) || c.Contains(connection.BoxB));
                
                existingCircuits = new List<Circuit>
                {
                    new Circuit(existingCircuits.SelectMany(c => c)
                        .Append(connection.BoxA)
                        .Append(connection.BoxB)
                        .ToArray())
                };
            }
            else
            {
                existingCircuits = new List<Circuit> { new Circuit(connection.BoxA, connection.BoxB) };
            }
            
            circuits.AddRange(existingCircuits);
        }
        
        return circuits;
    }
    
    record JunctionBox(int X, int Y, int Z)
    {
        public double DistanceTo(JunctionBox box)
        {
            return Math.Sqrt(Math.Pow(X - box.X, 2) + Math.Pow(Y - box.Y, 2) + Math.Pow(Z - box.Z, 2));
        }

        public virtual bool Equals(JunctionBox? other)
        {
            if (other is null) return false;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
    
    record JunctionBoxConnection(JunctionBox BoxA, JunctionBox BoxB)
    {
        public double Distance => BoxA.DistanceTo(BoxB);
        public virtual bool Equals(JunctionBoxConnection? other)
        {
            if (other is null) return false;
            return (BoxA.Equals(other.BoxA) && BoxB.Equals(other.BoxB)) ||
                   (BoxA.Equals(other.BoxB) && BoxB.Equals(other.BoxA));
        }

        public override int GetHashCode()
        {
            var boxes = new[] { BoxA, BoxB }
                .OrderBy(b => b.GetHashCode()).ToArray();
            return HashCode.Combine(boxes[0], boxes[1]);
        }
    }

    class Circuit : HashSet<JunctionBox>
    {
        public Circuit(params JunctionBox[] junctionBoxes)
        {
            foreach (var box in junctionBoxes)
            {
                Add(box);
            }
        }
    }
}

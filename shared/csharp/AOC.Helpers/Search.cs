// ReSharper disable InconsistentNaming
namespace AOC.Helpers;

public class Search<T> where T : IEquatable<T>
{
    public static List<T>? BFS(T start, Func<T, List<T>> getAdjacent, 
        Func<T, bool> destinationReached)
    {
        var queue = new Queue<SearchNode<T>>();
        var checkedNodes = new HashSet<SearchNode<T>>();
        var startNode = new SearchNode<T>(start);
        checkedNodes.Add(startNode);
        queue.Enqueue(startNode);
        SearchNode<T> node;
        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            if (destinationReached(node.Item))
                return node.GetPath();
            var adjacentItems = getAdjacent(node.Item);

            foreach (var adjacent in adjacentItems)
            {
                var adjacentNode = new SearchNode<T>(adjacent);
                if (checkedNodes.Add(adjacentNode))
                {
                    adjacentNode.Parent = node;
                    queue.Enqueue(adjacentNode);
                }
            }
        };

        return null;
    }

    public static void DFS(T start, Func<T, List<T>> getChildren,
        Func<T, bool> destinationReached)
    {
        var stack = new Stack<SearchNode<T>>();
        var visitedNodes = new HashSet<SearchNode<T>>();
        stack.Push(new SearchNode<T>(start));
        while (stack.Count > 0)
        {
            var node = stack.Pop();
            
            if (destinationReached(node.Item))
                return;

            if (visitedNodes.Add(node))
            {
                var children = getChildren(node.Item);
                foreach (var child in children)
                {
                    var childNode = new SearchNode<T>(child);
                    childNode.Parent = node;
                    stack.Push(childNode);
                }
            }
        }
    }

    private class SearchNode<TN> : IEquatable<SearchNode<T>> where TN : IEquatable<T>
    {
        public TN Item { get; }
        public SearchNode<TN>? Parent { get; set; }

        public SearchNode(TN item)
        {
            Item = item;
        }

        public List<TN> GetPath()
        {
            var path = new List<TN>();
            var current = this;
            while (current != null)
            {
                path.Add(current.Item);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }

        public bool Equals(SearchNode<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Item.Equals(other.Item);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchNode<T>)obj);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }
    }
}
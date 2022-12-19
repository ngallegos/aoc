﻿using System;
using System.Collections.Generic;

namespace AOC2022.Modules.Shared;

public class BFS<T> where T : IEquatable<T>
{
    public static List<T> Search(T start, Func<T, List<T>> getAdjacent, 
        Func<T, bool> destinationReached)
    {
        var queue = new Queue<BFSNode<T>>();
        var checkedNodes = new HashSet<BFSNode<T>>();
        var startNode = new BFS<T>.BFSNode<T>(start);
        checkedNodes.Add(startNode);
        queue.Enqueue(startNode);
        BFSNode<T> node = null;
        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            if (destinationReached(node.Item))
                return node.GetPath();
            var adjacentItems = getAdjacent(node.Item);

            foreach (var adjacent in adjacentItems)
            {
                var adjacentNode = new BFS<T>.BFSNode<T>(adjacent);
                if (!checkedNodes.Contains(adjacentNode))
                {
                    checkedNodes.Add(adjacentNode);
                    adjacentNode.Parent = node;
                    queue.Enqueue(adjacentNode);
                }
            }
        };

        return null;
    }

    private class BFSNode<T> : IEquatable<BFSNode<T>> where T : IEquatable<T>
    {
        public T Item { get; }
        public BFSNode<T> Parent { get; set; }

        public BFSNode(T item)
        {
            Item = item;
        }

        public List<T> GetPath()
        {
            var path = new List<T>();
            var current = this;
            while (current != null)
            {
                path.Add(current.Item);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }

        public bool Equals(BFSNode<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Item.Equals(other.Item);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BFSNode<T>)obj);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }
    }
}
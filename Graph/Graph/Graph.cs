using System;
using System.Collections.Generic;

class GraphFunctionalityTest
{
    static void Main()
    {
        Graph<int> testGraph = new Graph<int>(false);
        testGraph.AddEdge(1, 2, true);
        testGraph.AddEdge(2, 3, true);
        testGraph.AddEdge(3, 1, true);
        testGraph.AddEdge(3, 4, true);
        testGraph.AddEdge(1, 4, true);

        List<int> path;
        int count = testGraph.ReturnShortestDistance(1, 4, out path);

        Console.WriteLine(testGraph.IsCyclic());
        foreach (int member in path)
        {
            Console.Write(member + " ");
        }
        Console.WriteLine();
        Console.WriteLine(count);
    }
}


class Graph<T> where T : IComparable<T>
{
    private Dictionary<Node<T>, List<Node<T>>> adjacencyLists;
    private Dictionary<T, Node<T>> allNodes;
    private MultiKeyDictionary<Node<T>, Node<T>, int> distances;
    private bool isWeighted;

    public Graph(bool isWeighted)
    {
        adjacencyLists = new Dictionary<Node<T>, List<Node<T>>>();
        allNodes = new Dictionary<T, Node<T>>();
        this.isWeighted = isWeighted;
        if (isWeighted)
        {
            distances = new MultiKeyDictionary<Node<T>, Node<T>, int>();
        }
    }

    #region Adding edges, retreiving edges information

    /// <summary>
    /// If you've set the graph as not weighted, your choice of distance between teh nodes will be ignored.
    /// </summary>
    /// <param name="firstNode"></param>
    /// <param name="secondNode"></param>
    /// <param name="isDirected"></param>
    /// <param name="distance"></param>
    public void AddEdge(T firstNode, T secondNode, bool isDirected, int distance)
    {
        AddEdge(firstNode, secondNode, isDirected);

        if (this.isWeighted)
        {
            Node<T> firstNodeShell = allNodes[firstNode];
            Node<T> secondNodeShell = allNodes[secondNode];

            distances.AddKey(firstNodeShell, secondNodeShell, distance);
        }
    }

    public void AddEdge(T firstNode, T secondNode, bool isDirected)
    {
        if (!allNodes.ContainsKey(firstNode))
        {
            allNodes.Add(firstNode, new Node<T>(firstNode));
        }

        if (!allNodes.ContainsKey(secondNode))
        {
            allNodes.Add(secondNode, new Node<T>(secondNode));
        }

        Node<T> firstNodeShell = allNodes[firstNode];
        Node<T> secondNodeShell = allNodes[secondNode];

        List<Node<T>> currentList;
        if (!adjacencyLists.TryGetValue(firstNodeShell, out currentList))
        {
            currentList = new List<Node<T>>();
            adjacencyLists.Add(firstNodeShell, currentList);
        }
        currentList.Add(secondNodeShell);

        if (!isDirected)
        {
            this.AddEdge(secondNode, firstNode, true);
        }

        if (this.isWeighted)
        {
            distances.AddKey(firstNodeShell, secondNodeShell, 1);
        }
    }

    public int GetDistanceBetweenAdjacentNodes(T fromNode, T toNode)
    {
        if (!this.isWeighted)
        {
            throw new InvalidOperationException("This is not a weighted graph!");
        }
        Node<T> firstNodeShell = allNodes[fromNode];
        Node<T> secondNodeShell = allNodes[toNode];
        int distance;
        try
        {
            distance = distances[firstNodeShell, secondNodeShell];
        }
        catch (KeyNotFoundException)
        {
            throw (new InvalidOperationException("There is no edge leading from the first to the second node!"));
        }
        return distance;
    }

    /// <summary>
    /// Returns all children of a node as a list of T elements. Returns an empty list if the node doesn't have children;
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<T> ReturnAllChildren(T node)
    {
        Node<T> nodeShell = allNodes[node];

        if (adjacencyLists.ContainsKey(nodeShell))
        {
            List<Node<T>> childrenNodes = adjacencyLists[nodeShell];
            List<T> childrenT = new List<T>();
            foreach (Node<T> currentNode in childrenNodes)
            {
                childrenT.Add(currentNode.CoreElement);
            }
            return childrenT;
        }
        else
        {
            return new List<T>();
        }
    }

    /// <summary>
    /// Returns all children of a node as Nodes for private use of the class. Returns an empty list if the node doesn't have children;
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private List<Node<T>> ReturnAllChildrenAsNodes(Node<T> node)
    {
        if (adjacencyLists.ContainsKey(node))
        {
            return adjacencyLists[node];
        }
        else
        {
            return new List<Node<T>>();
        }
    }

    #endregion

    #region Cyclicity
    /// <summary>
    /// Returns information on whether there are any cycles in the graph.
    /// </summary>
    /// <returns></returns>
    /// The method uses the below two private methods: DFSForCyclicity and ExploreForCyclicity. It makes a Depth first search.
    public bool IsCyclic()
    {
        return DFSForCyclicity();
    }


    private bool DFSForCyclicity()
    {
        HashSet<Node<T>> visited = new HashSet<Node<T>>();
        Dictionary<Node<T>, int> preVisitClock = new Dictionary<Node<T>, int>();
        Dictionary<Node<T>, int> postVisitClock = new Dictionary<Node<T>, int>();
        int time = 0;

        bool isCyclic = false;
        foreach (KeyValuePair<T, Node<T>> node in allNodes)
        {
            Node<T> currentNode = node.Value;

            if (!visited.Contains(currentNode))
            {
                if (ExploreForCyclicity(currentNode, ref visited, ref preVisitClock, ref postVisitClock, ref time))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool ExploreForCyclicity(Node<T> currentNode, ref HashSet<Node<T>> visited, ref Dictionary<Node<T>, int> preVisitClock, ref Dictionary<Node<T>, int> postVisitClock, ref int time)
    {
        visited.Add(currentNode);
        preVisitClock.Add(currentNode, time);
        time++;

        List<Node<T>> currentChildren = ReturnAllChildrenAsNodes(currentNode);
        foreach (Node<T> child in currentChildren)
        {
            if (!visited.Contains(child))
            {
                return ExploreForCyclicity(child, ref visited, ref preVisitClock, ref postVisitClock, ref time);
            }
            else if (!postVisitClock.ContainsKey(child))
            {
                return true;
            }
        }
        postVisitClock.Add(currentNode, time);
        time++;
        return false;
    }
    #endregion

    #region Distances between edges
    public int ReturnShortestDistance(T startNode, T endNode, out List<T> path)
    {
        if (isWeighted)
        {
            return ReturnShortestDistanceDijkstra(startNode, endNode, out path);
        }
        else
        {
            return ReturnShortestDistanceBFS(startNode, endNode, out path);
        }
    }

    /// <summary>
    /// Finds the shortest distance through BFS. To be used when the graph is not weighted;
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <param name="path"></param>
    /// <returns>Return -1 if the end node is not reachable. Otherwise returns the shortest distance between the start and the end node</returns>
    private int ReturnShortestDistanceBFS(T startNode, T endNode, out List<T> path)
    {
        Queue<T> BFSQueue = new Queue<T>();
        BFSQueue.Enqueue(startNode);
        T currentElement;
        List<T> currentChildren;
        bool found = false;
        Dictionary<T, T> previousNode = new Dictionary<T, T>();
        while (BFSQueue.Count != 0)
        {
            currentElement = BFSQueue.Dequeue();
            currentChildren = ReturnAllChildren(currentElement);
            foreach (T child in currentChildren)
            {
                if (!previousNode.ContainsKey(child))
                {
                    previousNode.Add(child, currentElement);
                }
                if (child.CompareTo(endNode) == 0)
                {
                    found = true;
                    break;
                }
                BFSQueue.Enqueue(child);
            }
            if (found)
            {
                break;
            }
        }

        if (found)
        {
            path = new List<T>();
            T nextNode = endNode;
            while (previousNode.ContainsKey(nextNode))
            {
                path.Add(nextNode);
                nextNode = previousNode[nextNode];
            }
            path.Add(nextNode);
            path.Reverse();
            return path.Count - 1;
        }
        else
        {
            path = new List<T>();
            return -1;
        }

    }

    
    private int ReturnShortestDistanceDijkstra(T startNode, T endNode, out List<T> path)
    {
        //TODO This one is not functional, needs to be fixed
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, int> bestDistances = new Dictionary<T, int>();
        bestDistances.Add(startNode, 0);

        List<T> currentChildren = this.ReturnAllChildren(startNode);
        foreach (T child in currentChildren)
        {
            if (!visited.Contains(child))
            {
                if (!bestDistances.ContainsKey(child)) 
                {
                    bestDistances.Add(child, bestDistances[startNode] + 1); //fix 1 later!!!
                }
                else if (bestDistances[startNode] + 1 < bestDistances[child])//fix 1 later!!!
                {
                    bestDistances[child] = bestDistances[startNode] + 1; //fix 1 later!!!
                }
            }
        }

    }
    //have to make the priority queue work with a custom comparer
    #endregion


    private class Node<K> : IComparable<Node<K>>
        where K : IComparable<K>
    {
        private K coreElement;
        private int dijkstraValue;


        public Node(K element)
        {
            this.coreElement = element;
        }

        public int DijkstraValue
        {
            get { return this.dijkstraValue; }
            set { this.dijkstraValue = value; }
        }

        public K CoreElement
        {
            get { return this.coreElement; }
            set { this.coreElement = value; }
        }

        public int CompareTo(Node<K> objectToCompare)
        {
            if (this.coreElement.CompareTo(objectToCompare.CoreElement) < 0)
            {
                return -1;
            }
            else if (this.coreElement.CompareTo(objectToCompare.CoreElement) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override int GetHashCode()
        {
            return this.coreElement.GetHashCode();
        }
    }

}

public class MultiKeyDictionary<Q, M, F>
        where Q : IComparable<Q>
{
    private Dictionary<Q, Dictionary<M, F>> mainDictionary;

    public MultiKeyDictionary()
    {
        mainDictionary = new Dictionary<Q, Dictionary<M, F>>();
    }

    public void AddKey(Q firstKey, M secondKey, F value)
    {
        Dictionary<M, F> secondaryDictionary;
        if (!mainDictionary.TryGetValue(firstKey, out secondaryDictionary))
        {
            secondaryDictionary = new Dictionary<M, F>();
            mainDictionary.Add(firstKey, secondaryDictionary);
        }
        secondaryDictionary.Add(secondKey, value);
    }

    public F this[Q firstKey, M secondKey]
    {
        get { return this.mainDictionary[firstKey][secondKey]; }
    }

    public bool ContainsKeys(Q firstKey, M secondKey)
    {
        Dictionary<M, F> secondaryDictionary;
        if (!mainDictionary.TryGetValue(firstKey, out secondaryDictionary))
        {
            return false;
        }
        else
        {
            if (!secondaryDictionary.ContainsKey(secondKey))
            {
                return false;
            }
        }
        return true;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager
{
    public class Graph
    {
        public bool wasSaved;

        public int nodeID;
        public double minWeight;
        public List<Node> nodes = new List<Node>();

        /// <summary>
        /// Saves a wrgf file containing the graph at the location specified
        /// </summary>
        /// <param name="path">Location to save the file at</param>
        /// <param name="autosave">If this is set to true, the graph's saved property is not updated</param>
        public void Save(string path, bool autosave=false)
        {
            // This will be used to detect whether the file is of the correct format
            string data = "GRAPH FILE\n";

            data += minWeight + "\n" + nodeID + "\n";
            foreach (Node n in nodes)
            {
                data += n.location.ToString() + n.name + ":";
                foreach (Arc a in n.connections)
                {
                    data += a.ID + "," + a.GetName() + "," + a.GetDestination(n).name + "," + a.GetWeight();
                    data += "\n";
                }
            }
            data += "END";
            try
            {
                File.WriteAllText(path, data);
                if (!autosave)
                {
                    wasSaved = true;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Graph not saved, reason: " + e.Message);
            }
        }

        public double[,] Floyds()
        {
            double[,] leastDistances = new double[nodes.Count, nodes.Count];
            // Initialise values to infinity
            for (int i = 0; i < nodes.Count * nodes.Count; i++)
            {
                leastDistances[i/nodes.Count, i % nodes.Count] = double.PositiveInfinity;
            }

            // Set up immediate distances (directly connected nodes)
            for (int i = 0; i < nodes.Count; i++)
            {
                foreach (Arc a in nodes[i].connections)
                {
                    int gridX = i;
                    int gridY = nodes.IndexOf(a.GetDestination(nodes[i]));
                    if (a.GetWeight() < leastDistances[gridX, gridY])
                    {
                        // Not using Math.Min here for future development so that a routing table can be added in future more easily
                        leastDistances[gridX, gridY] = a.GetWeight();
                    }
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int x = 0; x < nodes.Count; x++)
                {
                    for (int y = 0; y < nodes.Count; y++)
                    {
                        // Check connections to each node, excluding node i and the diagonal (node connecting to itself)
                        if (x != y && x != i && y != i)
                        {
                            if (leastDistances[y, i] + leastDistances[i, x] < leastDistances[y, x])
                            {
                                leastDistances[y, x] = leastDistances[y, i] + leastDistances[i, x];
                            }
                        }
                    }
                }
            }

            return leastDistances;
        }

        public List<Arc> Prims()
        {
            List<Arc> MST = new List<Arc>();
            List<Node> joinedNodes = new List<Node>();

            // Arbitrary start node
            Node start = nodes[0];
            joinedNodes.Add(start);

            while (joinedNodes.Count < nodes.Count)
            {
                double min = double.PositiveInfinity;
                Arc minArc = null;
                Node minNode = null;
                foreach (Node n in joinedNodes)
                {
                    for (int i = 0; i < n.connections.Count; i++)
                    {
                        // If the connection is the smallest we've seen so far, and it does not connect to a node already on the tree, select it
                        if (n.connections[i].GetWeight() < min && !joinedNodes.Contains(n.connections[i].GetDestination(n)))
                        {
                            min = n.connections[i].GetWeight();
                            minArc = n.connections[i];
                            minNode = n;
                        }
                    }
                }
                if (minArc == null)
                {
                    // Graph has unconnected nodes, so a MST is not possible
                    return null;
                }
                MST.Add(minArc);
                joinedNodes.Add(minArc.GetDestination(minNode));
            }

            return MST;
        }

        public List<Node> Dijkstra(Node start, Node end)
        {
            int count = nodes.Count;
            // Distance is the distance from the start
            double[] distance = new double[count];
            // Store the node before the one at the index position on the shortest route
            Node[] previous = new Node[count];
            // Set to true when all routes to neighbours have been checked
            bool[] explored = new bool[count];

            int i = 0;
            foreach (Node n in nodes)
            {
                if (n == start)
                {
                    distance[i] = 0;
                }
                else
                {
                    distance[i] = double.PositiveInfinity;
                }
                // Previous is already null for all values so no need to set it here
                explored[i] = false;
                i++;
            }

            // Do this while there are still unexplored nodes
            while (explored.Contains(false))
            {
                int smallestIndex = -1;
                double smallestValue = double.PositiveInfinity;
                for (int j = 0; j < count; j++)
                {
                    if (explored[j] == false && distance[j] < smallestValue)
                    {
                        smallestIndex = j;
                        smallestValue = distance[j];
                    }
                }

                // This means we have some nodes which are unreachable from the start point, and we have explored all reachable nodes
                if (smallestIndex == -1)
                {
                    // First, check if the end is reachable
                    if (distance[nodes.IndexOf(end)] == double.PositiveInfinity)
                    {
                        // No possible route
                        return null;
                    }
                    else
                    { 
                        for (int j = 0; j < count; j++)
                        {
                            return Backtrack(previous, end);
                        }
                    }
                }

                explored[smallestIndex] = true;
                foreach (Arc a in nodes[smallestIndex].connections)
                {
                    Node neighbour = a.GetDestination(nodes[smallestIndex]);
                    int indexOfNeighbour = nodes.IndexOf(neighbour);
                    if (explored[indexOfNeighbour] == false)
                    {
                        double newDist = distance[smallestIndex] + a.GetWeight();
                        if (newDist < distance[indexOfNeighbour])
                        {
                            distance[indexOfNeighbour] = newDist;
                            previous[indexOfNeighbour] = nodes[smallestIndex];
                        }
                    }
                }
            }

            return Backtrack(previous, end);
        }

        private List<Node> Backtrack(Node[] previous, Node end)
        {
            List<Node> route = new List<Node>();
            int pos = nodes.IndexOf(end);
            route.Add(end);

            while (previous[pos] != null)
            {
                route.Add(previous[pos]);
                pos = nodes.IndexOf(previous[pos]);
            }

            // Reverse returns nothing, but alters the list.
            route.Reverse();
            return route;
        }

        public List<Node> AStar(Node start, Node end)
        {
            // For any node in the open set, cameFrom at the same index is the node before it on the shortest path 
            var cameFrom = new List<Node>();

            // gScore is the cost of the cheapest path from start to the node
            var gScore = new List<double>();

            // fScore is our current best guess at how cheap a path could be
            var fScore = new List<double>();

            List<bool> explored = new List<bool>();

            foreach (Node n in nodes)
            {
                cameFrom.Add(null);
                gScore.Add(double.PositiveInfinity);
                fScore.Add(double.PositiveInfinity);
                explored.Add(false);
            }
            int startPos = nodes.IndexOf(start);
            gScore[startPos] = 0f;
            fScore[startPos] = Heuristic(start, end);

            while (explored.Contains(false))
            {
                // The node with the lowest fScore
                int currentPos = -1;
                double min = double.PositiveInfinity;
                for (int i = 0; i < fScore.Count; i++)
                {
                    if (fScore[i] < min && explored[i] == false)
                    {
                        currentPos = i;
                        min = fScore[i];
                    }
                }

                // This means we have some nodes which are unreachable from the start point, and we have explored all reachable nodes
                if (currentPos == -1)
                {
                    // First, check if the end is reachable
                    if (gScore[nodes.IndexOf(end)] == double.PositiveInfinity)
                    {
                        // No possible route
                        return null;
                    }
                    else
                    {
                        for (int j = 0; j < fScore.Count; j++)
                        {
                            return Backtrack(cameFrom.ToArray(), end);
                        }
                    }
                }

                Node currentNode = nodes[currentPos];
                if (currentNode == end)
                {
                    List<Node> path = new List<Node>();
                    while (currentNode != null)
                    {
                        path.Add(currentNode);
                        currentNode = cameFrom[nodes.IndexOf(currentNode)];
                    }
                    path.Reverse();
                    return path;
                }
                else
                {
                    explored[currentPos] = true;

                    for (int i = 0; i < currentNode.connections.Count; i++)
                    {
                        int neighbourPos = nodes.IndexOf(currentNode.connections[i].GetDestination(currentNode));
                        double temp_gScore = gScore[currentPos] + currentNode.connections[i].GetWeight();
                        if (temp_gScore < gScore[neighbourPos])
                        {
                            // This path is better than any previous one
                            cameFrom[neighbourPos] = currentNode;
                            gScore[neighbourPos] = temp_gScore;
                            fScore[neighbourPos] = temp_gScore + Heuristic(nodes[neighbourPos], end);

                            if (explored[neighbourPos] == true)
                            {
                                explored[neighbourPos] = false;
                            }
                        }
                    }
                }
            }
            // Failure
            return null;
        }

        public double Heuristic(Node current, Node goal)
        {
            // Good heuristic goes here.

            // Using min edge weight for now
            return minWeight;
        }
    }
}

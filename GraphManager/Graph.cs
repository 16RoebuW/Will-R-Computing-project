using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphManager
{
    public class Graph
    {
        public bool wasSaved;

        public int nodeID;
        // Initialise to a very large number so that any new value is smaller than it
        public double minWeight = double.MaxValue;
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
            // Store the simple attributes
            data += minWeight + "\n" + nodeID + "\n";
            // Store the nodes
            foreach (Node n in nodes)
            {
                data += n.location.ToString() + n.name + @"[:-~-:]";
                foreach (Arc a in n.connections)
                {
                    data += a.ID + @"[,-,]" + a.GetName() + @"[,-,]" + a.GetDestination(n).name + @"[,-,]" + a.GetWeight();
                    data += "\n";
                }
            }
            data += "END";
            // The data has been created, try to store it in a file but validate that the file exists
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

        /// <summary>
        /// Standard implementation of Floyd'a algorithm on the graph. 
        /// </summary>
        /// <returns>The least distance matrix arranged in the order nodes are stored</returns>
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

        /// <summary>
        /// Standard implementation of Prim's algorithm on the graph
        /// </summary>
        /// <returns>A list of the arcs included in the MST. Returns null if no MST exists</returns>
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

        /// <summary>
        /// Standard implementation of Dijkstra's algorithm on the graph
        /// </summary>
        /// <param name="start">Node to begin the route from</param>
        /// <param name="end">Destination node</param>
        /// <returns>List of nodes in order from start to end where possible, null otherwise</returns>
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

                // This means we have some nodes which are unreachable from the start point and we have explored all reachable nodes
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
                        return Backtrack(previous, end);
                    }
                }

                // Explore the node with the next smallest distance
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

        // Private function used inside Dijkstra and A* to backtrack through the previous nodes
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

        /// <summary>
        /// Standard implementation of the A* algorithm on the graph
        /// </summary>
        /// <param name="start">Node to begin the route from</param>
        /// <param name="end">Destination node</param>
        /// <returns>List of nodes in order from start to end, returns null when no route is possible</returns>
        public List<Node> AStar(Node start, Node end)
        {
            // For any node in the open set, cameFrom at the same index is the node before it on the shortest path 
            var cameFrom = new List<Node>();

            // gScore is the cost of the cheapest path from start to the node
            var gScore = new List<double>();

            // fScore is our current best guess at how cheap a path could be
            var fScore = new List<double>();

            List<bool> explored = new List<bool>();

            // Initialise values
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
                        return Backtrack(cameFrom.ToArray(), end);
                    }
                }

                Node currentNode = nodes[currentPos];
                if (currentNode == end)
                {
                    // If the end is reached, return the route
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
                        // Work out if this route is better
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

        // Placeholder heuristic function for A*
        public double Heuristic(Node current, Node goal)
        {
            // Good heuristic goes here.

            // Using min edge weight for now
            if (minWeight != double.MaxValue)
            {
                return minWeight;
            }
            else
            {
                // This will only occur if weights have been left unset, meaning they will have their default value of 0
                return 0;
            }
        }

        /// <summary>
        /// Brute force implementation of travelling saleseman - VERY SLOW
        /// </summary>
        /// <param name="start">Node to start and end at</param>
        /// <returns>Hamiltonian cycle or empty list if none is possible</returns>
        public List<Node> TravellingSalesman(Node start)
        {
            List<Node> minPath = null;
            List<Node> path = new List<Node>();
            List<int> nodeIDs = new List<int>();
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            // Create the initial path
            int count = 0;
            foreach (Node n in nodes)
            {
	            if (n != start)
                {
		            path.Add(n);
                    nodeIDs.Add(count);
                    count++;
	            }
            }

            double shortest = double.PositiveInfinity;
            stopwatch.Start();
            for (int i = 0; i <= 120; i++)
            {
                TestPermutation(ref path, ref minPath, ref shortest, start);
                bool nextIsPossible = FindNextPermutation(ref nodeIDs, ref path);
                if (!nextIsPossible)
                {
                    if (shortest == double.PositiveInfinity)
                    {
                        // This means that there is no possible route
                        return new List<Node>();
                    }
                    else
                    {
                        return minPath;
                    }
                }
            }
            stopwatch.Stop();

            double factorial = 1;
            for (int i = 1; i < nodes.Count; i++ )
            {
                factorial *= i;
            }

            // Subtract 1 for the time that has already elapsed (1 is multiplied by already elapsed time)
            
            double timeMs = Convert.ToDouble((factorial / 120f) - 1) * Convert.ToDouble(stopwatch.ElapsedMilliseconds);

            
            System.Windows.Forms.DialogResult answer;
            // This means we have an overflow so time must be > 29000 years in ms (922337203685477ms)
            if (timeMs >= TimeSpan.MaxValue.TotalMilliseconds)
            {
                answer = System.Windows.Forms.MessageBox.Show("For this network, this algorithm will take more than 29 thousand years to complete, are you sure you want to continue?", "Time warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            }
            else
            {
                TimeSpan estTime = TimeSpan.FromMilliseconds(timeMs);

                // Display remaining time and ask user whether to continue
                answer = System.Windows.Forms.MessageBox.Show("On your machine, this algorithm may take just over " + estTime.Days + " days, " + estTime.Hours + " hours, " + estTime.Minutes + " minutes, and " + estTime.Seconds + " seconds to complete, are you sure you want to continue?", "Time warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            if (answer == System.Windows.Forms.DialogResult.No)
            {
                return new List<Node>();
            }

            do
            {
                TestPermutation(ref path, ref minPath, ref shortest, start);
            } while (FindNextPermutation(ref nodeIDs, ref path));

	        if (shortest == double.PositiveInfinity)
            {
		        // This means that there is no possible route
		        return new List<Node>();
            }
	        else
            {
		        return minPath;
	        }
                
        }

        // Find the next permutation of the int array. Adapted from https://www.geeksforgeeks.org/traveling-salesman-problem-tsp-implementation/
        public bool FindNextPermutation(ref List<int> data, ref List<Node> nodes)
        {
            // If the given dataset is empty or contains only one element next_permutation is not possible
            if (data.Count <= 1)
                return false;
            int last = data.Count - 2;

            // find the longest non-increasing suffix and find the pivot
            while (last >= 0)
            {
                if (data[last] < data[last + 1])
                    break;
                last--;
            }

            // If there is no increasing pair there is no higher order permutation
            if (last < 0)
                return false;
            int nextGreater = data.Count - 1;

            // Find the rightmost successor to the pivot
            for (int i = data.Count - 1; i > last; i--)
            {
                if (data[i] > data[last])
                {
                    nextGreater = i;
                    break;
                }
            }

            // Swap the successor and the pivot
            int temp = data[nextGreater];
            Node tempN = nodes[nextGreater];
            data[nextGreater] = data[last];
            nodes[nextGreater] = nodes[last];
            data[last] = temp;
            nodes[last] = tempN;

            // Reverse the suffix
            int left = last + 1;
            int right = data.Count - 1;
            while (left < right)
            {
                temp = data[left];
                tempN = nodes[left];
                data[left] = data[right];
                // ++ updates the variable AFTER it has been used
                nodes[left++] = nodes[right];
                data[right] = temp;
                nodes[right--] = tempN;
            }

            // Return true as the next_permutation is done
            return true;
        }

        private void TestPermutation(ref List<Node> path, ref List<Node> minPath, ref double shortest, Node start)
        {
            double currentPathWeight = -1;

            //Find current path weight
            try
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    currentPathWeight += path[i].GetArcBetween(path[i + 1]).GetWeight();
                }
                currentPathWeight += path[0].GetArcBetween(start).GetWeight();
                currentPathWeight += path[path.Count - 1].GetArcBetween(start).GetWeight();

                // Update shortest length
                if (currentPathWeight < shortest)
                {
                    shortest = currentPathWeight;
                    // .ToList sets the list by value rather than by reference
                    minPath = path.ToList();
                }
            }
            catch { }
        }

    }
}
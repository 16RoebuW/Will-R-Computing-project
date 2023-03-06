using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphManager
{
    // This class represents the individual datapoints on the graph, storing the connections between them
    public class Node
    {
        public string name;
        public List<Arc> connections = new List<Arc>();
        public System.Drawing.Point location;

        /// <summary>
        /// Node constructor, validates the name given as a parameter
        /// </summary>
        /// <param name="parentGraph">The graph that this node is a part of</param>
        /// <param name="name">The name of the node (leave blank for automatic)</param>
        /// <param name="location">The position of the node initially</param>
        public Node(Graph parentGraph, string name, System.Drawing.Point location)
        {
            parentGraph.wasSaved = false;
            if (name == "")
            {
                // Automatically assigns nodes with names A-Z, but if there are more than 26 they are named A1-Z1, then A2-Z2 and so on
                if (parentGraph.nodeID > 25)
                {
                    this.name = (char)((parentGraph.nodeID % 26) + 65) + (parentGraph.nodeID / 26).ToString();
                }
                else
                {
                    this.name = ((char)(parentGraph.nodeID + 65)).ToString();
                }
                parentGraph.nodeID++;

                // If this name already exists, add more text to its name indicating this
                int copies = 0;
                string letter = this.name;
                string copyName = this.name;
                while (parentGraph.nodes.Contains(this))
                {
                    copies++;
                    copyName = letter + " (" + copies + ")";
                    this.name = copyName;
                }
            }
            else
            {
                // Ensures no duplicate names exist on the graph, incrementing them as 'name (1)', 'name (2)' etc
                int copies = 0;
                string copyName = name;
                this.name = copyName;
                while (parentGraph.nodes.Contains(this))
                {
                    copies++;
                    copyName = name + " (" + copies + ")";
                    this.name = copyName;
                }
            }
            // Range check for position (short hand for if location < 0, location = 0)
            this.location.X = Math.Max(0, location.X);
            this.location.Y = Math.Max(0, location.Y);
        }

        // I have overriden this function to change the behaviour of .Contains so that it returns true if a node is found with the same name as the parameter
        // This post guided me to this solution: https://stackoverflow.com/questions/1076350/modify-list-contains-behavior
        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                return ((Node)obj).name == name;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        /// <summary>
        /// Join this node to another, updating the other node. This procedure is not validated
        /// </summary>
        /// <param name="n">Destination node</param>
        /// <param name="weight">Connection weight</param>
        public void JoinTo(Node n, string name, double weight, ref int IDCount)
        {
            Arc connection = new Arc(name, n, weight, ref IDCount);
            connection.between[0] = this;
            connections.Add(connection);
            n.connections.Add(connection);
        }

        /// <summary>
        /// Given an input node, find the arc connecting this node to the input (returns null on a failure)
        /// </summary>
        /// <param name="other">The node on the other side of this arc</param>
        /// <returns></returns>
        public Arc GetArcBetween(Node other)
        {
            foreach (Arc a in connections)
            {
                if (a.between.Contains(other))
                {
                    return a;
                }
            }
            return null;
        }
    }
}

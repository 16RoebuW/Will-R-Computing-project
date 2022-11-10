using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphManager
{
    public class Node
    {
        public string name;
        public List<Arc> connections = new List<Arc>();
        public System.Drawing.Point location;

        [JsonConstructor]
        public Node(string name, System.Drawing.Point location)
        {
            this.name = name;
            this.location = location;
        }
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
            connection.between.Add(this);
            connections.Add(connection);
            n.connections.Add(connection);
        }

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

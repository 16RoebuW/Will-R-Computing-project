using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager
{
    class Node
    {
        public string name;
        public List<Arc> connections;

        public Node(Graph parentGraph, string name)
        {
            int copies = 0;
            if (parentGraph.nodes.Contains("bob"))
            {
                
            }
        }

        /// <summary>
        /// Join this node to another, updating the other node. This procedure is not validated
        /// </summary>
        /// <param name="n">Destination node</param>
        /// <param name="weight">Connection weight</param>
        public void JoinTo(Node n, double weight)
        {
            Arc outgoing = new Arc("", n, weight);
            Arc incoming = new Arc("", this, weight);
            connections.Add(outgoing);
            n.connections.Add(incoming);
        }
    }
}

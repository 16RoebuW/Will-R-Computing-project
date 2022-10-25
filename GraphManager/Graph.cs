using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager
{
    public class Graph
    {
        private bool wasSaved;

        public int nodeID;
        public double minWeight;
        public List<Node> nodes = new List<Node>();
    }
}

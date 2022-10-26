using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphManager
{
    public class Arc
    {
        private string name;
        private double weight;

        public int ID;
        public Node[] between = new Node[2];

        public Arc(string name, Node destination, double weight, ref int IDCount)
        {
            this.name = name;
            between[1] = destination;
            this.weight = weight;
            ID = IDCount;
            IDCount++;
        }

        /// <summary>
        /// Validates string given as name, then sets it if it passes
        /// </summary>
        /// <param name="newName">Desired name</param>
        public void SetName(string newName)
        {
            // Length check
            if (newName.Length < 31)
            {
                name = newName;
            }
        }

        /// <summary>
        /// Returns the name of the arc as a string
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Validates double given as weight, then sets it if it passes
        /// </summary>
        /// <param name="newWeight">Desired weight</param>
        public void SetWeight(double val)
        {
            // Range check
            if (val >= 0)
            {
                weight = val;                
            }
        }

        /// <summary>
        /// Returns the weight of the arc as a double
        /// </summary>
        /// <returns></returns>
        public double GetWeight()
        {
            return weight;
        }

        /// <summary>
        /// Returns the node that can be travelled to from the input via this arc, similar to the old "destination" value
        /// </summary>
        /// <param name="start">Start node</param>
        /// <returns></returns>
        public Node GetDestination(Node start)
        {
            // Efficiency step
            Node firstItem = between[0];
            if (firstItem == start)
            {
                return between[1];
            }
            else
            {
                return firstItem;
            }
        }
    }
}

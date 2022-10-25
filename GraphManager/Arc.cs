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

        public Node destination;

        public Arc(string name, Node destination, double weight)
        {
            this.name = name;
            this.destination = destination;
            this.weight = weight;
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
    }
}

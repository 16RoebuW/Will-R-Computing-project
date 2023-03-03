namespace GraphManager
{
    // Arc class, representing connections between nodes. A reference is stored by each node
    public class Arc
    {
        private string name;
        private double weight;

        public int ID;
        public Node[] between = new Node[2];
        public bool highlighted = false;

        /// <summary>
        /// Arc constructor, does not validate weight, should be used and then the arc should be added to the base node's list of connections
        /// </summary>
        /// <param name="name">Name of the arc (duplicates allowed)</param>
        /// <param name="destination">The node on the other side of the arc, not the one that stores this arc in a list</param>
        /// <param name="weight">Weight of the arc</param>
        /// <param name="IDCount">Counter to be incremented</param>
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
            // Pick the one that isn't the input
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

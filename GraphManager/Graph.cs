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
    }
}

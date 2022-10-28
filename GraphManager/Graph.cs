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

        public void Save(string path, bool autosave)
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

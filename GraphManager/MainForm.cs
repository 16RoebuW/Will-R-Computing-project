using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GraphManager
{
    public partial class mainForm : Form
    {
        public Graph activeGraph = new Graph();
        public Arc activeEdge;
        public Node activeNode;
        public int IDCount = 0;


        public mainForm()
        {
            InitializeComponent();
            // Code to stop drawings flickering
            // Source: https://stackoverflow.com/questions/8046560/how-to-stop-flickering-c-sharp-winforms
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, this, new object[] { true });
        }

        private void ProgramLoaded(object sender, EventArgs e)
        {
            // While this code seems to do nothing, it actually has the effect of applying the saved options to the main form when the program starts
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.Show();
            optionsForm.Close();          
        }

        private void OpenOptions(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.Show();
        }

        private void HandleEdgeClick(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;
            if (rdbCreate.Checked)
            {
                // Create mode
            }   
            else if (rdbEdit.Checked)
            {
                // Edit mode
                activeEdge = FindArcFromID(Convert.ToInt32(btnSender.Name));
                EdgeEditDialogue editWindow = new EdgeEditDialogue();
                editWindow.ShowDialog(this);          
            }
            else if (rdbDelete.Checked)
            {
                // Delete mode

                activeEdge = FindArcFromID(Convert.ToInt32(btnSender.Name));

                activeEdge.between[0].connections.Remove(activeEdge);
                activeEdge.between[0] = null;
                activeEdge.between[1].connections.Remove(activeEdge);
                activeEdge.between[1] = null;
                // Hopefully won't result in a memory leak

                Controls.Remove(btnSender);
                activeEdge = null;
                DisplayGraph(activeGraph);
            }
            else
            {
                // This should never happen, but if it does, ignore it.
            }

        }

        private void HandleNodeClick(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;
            activeNode = FindNodeWithName(activeGraph.nodes, btnSender.Text);

            if (rdbCreate.Checked)
            {
                // Create mode

                if (statusLabel.Text.Contains("Select the node to connect to:"))
                {
                    string[] parsedText = statusLabel.Text.Split(':');
                    activeNode.JoinTo(FindNodeWithName(activeGraph.nodes, parsedText[1]), "", 0, ref IDCount);
                    statusLabel.Text = "";
                    DisplayGraph(activeGraph);
                }
                else
                { 
                    statusLabel.Text = "Select the node to connect to:" + activeNode.name;
                }
            }
            else if (rdbEdit.Checked)
            {
                // Edit mode

                NodeEditDialogue editWindow = new NodeEditDialogue();
                editWindow.ShowDialog(this);
            }
            else if (rdbDelete.Checked)
            {
                // Delete mode

            }
            else
            {
                // This should never happen, but if it does, ignore it.
            }
        }

        /// <summary>
        /// Returns a node from a list of nodes that has the name specified. Returns null and displays a message if not found
        /// </summary>
        /// <param name="nodes">The list to be searched</param>
        /// <param name="name">The name the node should have</param>
        /// <returns></returns>
        public Node FindNodeWithName(List<Node> nodes, string name)
        {
            foreach (Node node in nodes)
            {
                if (node.name == name)
                {
                    return node;
                }
            }
            MessageBox.Show("NODE NOT FOUND");
            return null;
        }

        /// <summary>
        /// Takes an integer input and returns the arc with that ID
        /// </summary>
        /// <param name="id">The ID the arc should have</param>
        /// <returns></returns>
        private Arc FindArcFromID(int id)
        {
            // O(N^2) complexity here is bad, should be improved in future, perhaps with indexing
            foreach (Node n in activeGraph.nodes)
            {
                foreach (Arc a in n.connections)
                {
                    if (a.ID == id)
                    {
                        return a;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Changes the fonts of some controls
        /// </summary>
        /// <param name="font">The font to be changed to</param>
        public void ChangeFonts(Font font)
        {
            cbxAlgorithmSelect.Font = font;
            statusLabel.Font = font;
        }

        /// <summary>
        /// Clears all graph related controls and draws a new graph using paint and buttons
        /// </summary>
        /// <param name="input">The graph that should be drawn</param>
        public void DisplayGraph(Graph input)
        {
            // Clear existing graph
            for (int i = 0; i < this.Controls.Count; i++)
            {
                Control c = this.Controls[i];
                if ((string)c.Tag == "Graph Part" || (string)c.Tag == "Edge Label")
                {
                    this.Controls.Remove(c);
                    i--;
                }
            }

            // Draws lines between each node
            activeGraph = input;
            this.Invalidate();

            foreach (Node n in input.nodes)
            {
                // Create button to represent node
                Button btnNode = new Button
                {
                    Location = n.location,
                    Text = n.name,
                    Tag = "Graph Part",
                    Size = new Size(234, 45)
                };
                btnNode.Click += new EventHandler(HandleNodeClick);
                Controls.Add(btnNode);

                // Draw arcs to each connected node 
                foreach (Arc a in n.connections)
                {
                    // Prevents two buttons per arc by checking if one already exists
                    // (ContainsKey checks for a control with the given name)
                    if (!Controls.ContainsKey(a.ID.ToString()))
                    {
                        Button btnArc = new Button
                        {
                            // Centerpoint between nodes
                            Location = new Point((a.between[0].location.X + a.between[1].location.X) / 2,
                                                    (a.between[0].location.Y + a.between[1].location.Y) / 2),
                            Tag = "Edge Label",
                            Size = new Size(75, 75),
                            Name = a.ID.ToString()
                        };

                        // When this button is clicked, call the HandleEdgeClick procedure
                        btnArc.Click += new EventHandler(HandleEdgeClick);

                        if (a.GetWeight() == 0)
                        {
                            if (a.GetName() == "")
                            {
                                btnArc.Text = "_";
                            }
                            else
                            {
                                btnArc.Text = a.GetName();
                            }
                        }
                        else if (a.GetName() == "")
                        {
                            btnArc.Text = "Weight = " + a.GetWeight();
                        }
                        else
                        {
                            btnArc.Text = a.GetName() + ", weight = " + a.GetWeight();
                        }

                        Controls.Add(btnArc);
                    }
                }
            }           
        }

        // Called on "this.Invalidate()", clears the canvas each time
        protected override void OnPaint(PaintEventArgs e)
        {
            // Source: https://stackoverflow.com/questions/47239557/c-sharp-how-do-i-draw-a-line-between-two-objects-on-a-windows-form

            base.OnPaint(e);

            using (var blackPen = new Pen(Color.Black, 3))
            {
                foreach (Node n in activeGraph.nodes)
                {
                    foreach (Arc a in n.connections)
                    {
                        e.Graphics.DrawLine(blackPen, a.between[0].location, a.between[1].location);
                    }
                }
            }
        }

        /// <summary>
        /// Changes various properties of an the active edge
        /// </summary>
        /// <param name="mode">0 = Set name and weight, 1 = Set name, 2 = Set weight, 3/other = None</param>
        /// <param name="newName">The name that should be set or not</param>
        /// <param name="newWeight">The weight that should be set or not</param>
        public void EditEdge(int mode, string newName, double newWeight)
        {
            switch (mode)
            {
                case 0:
                    // Change both
                    activeEdge.SetName(newName);
                    activeEdge.SetWeight(newWeight);
                    break;
                case 1:
                    // Change name
                    activeEdge.SetName(newName);
                    break;
                case 2:
                    // Change weight
                    activeEdge.SetWeight(newWeight);
                    break;
                case 3:
                    // Do nothing
                    break;
            }
            DisplayGraph(activeGraph);
            activeEdge = null;
        }

        /// <summary>
        /// Sets the active node's name to the string
        /// </summary>
        /// <param name="newName">The name that should be set</param>
        public void EditNode(string newName)
        {
            activeNode.name = newName;
            DisplayGraph(activeGraph);
            activeNode = null;
        }

        private void btnAlgRun_Click(object sender, EventArgs e)
        {
            // !!Testing code!!
            Graph graph = new Graph();
            graph.nodes.Add(new Node(graph, "Cottonshopeburnfoot", new Point(500, 500)));
            graph.nodes.Add(new Node(graph, "", new Point(900, 400)));
            graph.nodes.Add(new Node(graph, "", new Point(200, 200)));
            graph.nodes[0].JoinTo(graph.nodes[1], "M2", 5, ref IDCount);
            graph.nodes[1].JoinTo(graph.nodes[2], "", 0, ref IDCount);
            DisplayGraph(graph);
        }
    }
}

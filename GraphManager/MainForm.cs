using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
        public float zoomLevel = 1;
        public float prevZoomLevel = 1;


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
            statusLabel.Text = "";
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
            activeNode = FindNodeWithName(activeGraph.nodes, btnSender.Name);

            if (rdbCreate.Checked)
            {
                // Create mode

                if (statusLabel.Text.Contains("Select the node to connect to:"))
                {
                    string[] parsedText = statusLabel.Text.Split(':');
                    Node destination = FindNodeWithName(activeGraph.nodes, parsedText[1]);
                    if (activeNode == destination)
                    {
                        statusLabel.Text = "Edge creation cancelled, you cannot join a node to itself";                        
                    }
                    else
                    {
                        activeNode.JoinTo(destination, "", 0, ref IDCount);
                        statusLabel.Text = "";
                        DisplayGraph(activeGraph);
                    }
                    
                }
                else
                { 
                    statusLabel.Text = "Select the node to connect to:" + activeNode.name;
                }
            }
            else if (rdbEdit.Checked)
            {
                // Edit mode
                statusLabel.Text = "";
                NodeEditDialogue editWindow = new NodeEditDialogue();
                editWindow.ShowDialog(this);
            }
            else if (rdbDelete.Checked)
            {
                // Delete mode
                statusLabel.Text = "";
                activeNode = FindNodeWithName(activeGraph.nodes, btnSender.Name);
                // Delete all connections to this node
                for (int i = 0; i < activeNode.connections.Count; i++)
                {
                    // Gets the node at the other end of the arc from the deleted node
                    Node destination = activeNode.connections[i].GetDestination(activeNode);
                    // Removes the arc from the destination node's list
                    destination.connections.Remove(activeNode.connections[i]);
                }
                activeGraph.nodes.Remove(activeNode);
                // Refresh display
                DisplayGraph(activeGraph);
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
                    Name = n.name,
                    Tag = "Graph Part",
                    Size = new Size((int)(234f * zoomLevel), (int)(45f * zoomLevel)),
                };
                if (zoomLevel <= 0.5)
                {
                    btnNode.Text = ShortenText(btnNode.Name);
                }
                else
                {
                    btnNode.Text = btnNode.Name;
                }

                btnNode.Click += new EventHandler(HandleNodeClick);
                btnNode.LocationChanged += new EventHandler(HandleNodeDrag);
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
                            Size = new Size((int)(75f * zoomLevel), (int)(75f * zoomLevel)),
                            Name = a.ID.ToString()
                        };

                        // When this button is clicked, call the HandleEdgeClick procedure
                        btnArc.Click += new EventHandler(HandleEdgeClick);

                        if (zoomLevel > 0.5)
                        {
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
                        }
                        else
                        {
                            if (a.GetWeight() == 0)
                            {
                                if (a.GetName() == "")
                                {
                                    btnArc.Text = "_";
                                }
                                else
                                {
                                    btnArc.Text = ShortenText(a.GetName());
                                }
                            }
                            else if (a.GetName() == "")
                            {
                                btnArc.Text = "W: " + a.GetWeight();
                            }
                            else
                            {
                                btnArc.Text = ShortenText(a.GetName()) + ", W: " + a.GetWeight();
                            }
                        }
                        Controls.Add(btnArc);
                    }
                }
            }           
        }

        private void HandleNodeDrag(object sender, EventArgs e)
        {           
            Button btn = sender as Button;
            // Keeps the location inside the bounds of the form
            // 79 is the height of the two toolbars at the top
            // 36 is the height of the flow layout panel at the bottom
            // Location is the top left of the button, so I need to subtract its width/height from the min limits
            btn.Location = new Point(Math.Min(this.Width - btn.Width, Math.Max(0, btn.Location.X)),
                                        Math.Min(this.Height - 36 - statusStrip.Size.Height - btn.Height, Math.Max(79, btn.Location.Y)));
            btn.BringToFront();
            activeNode = FindNodeWithName(activeGraph.nodes, btn.Name);
            activeNode.location = btn.Location;
            this.Invalidate(); 
        }

        // Called on "this.Invalidate()", clears the canvas each time
        protected override void OnPaint(PaintEventArgs e)
        {
            // Source: https://stackoverflow.com/questions/47239557/c-sharp-how-do-i-draw-a-line-between-two-objects-on-a-windows-form

            base.OnPaint(e);

            using (var blackPen = new Pen(Color.Black, 3 * zoomLevel * zoomLevel))
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
                    if (newWeight < 0)
                    {
                        MessageBox.Show("Weight must be positive, value not changed");
                    }
                    else
                    {
                        activeEdge.SetWeight(newWeight);
                    }
                    break;
                case 1:
                    // Change name
                    activeEdge.SetName(newName);
                    break;
                case 2:
                    // Change weight
                    if (newWeight < 0)
                    {
                        MessageBox.Show("Weight must be positive, value not changed");
                    }
                    else
                    {
                        activeEdge.SetWeight(newWeight);
                    }
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
            // To ensure no identical names are created, we must destroy the existing node and replace it with one with the new name
            // This way, the validation inside the node constructor can be carried out
            // First, store all attributes of the node (except name)
            List<Arc> oldConnections = activeNode.connections;
            Point oldLocation = activeNode.location;

            activeGraph.nodes.Remove(activeNode);

            activeGraph.nodes.Add(new Node(activeGraph, newName, oldLocation));
            activeGraph.nodes.Last().connections = oldConnections;

            DisplayGraph(activeGraph);
            activeNode = null;
        }

        private void btnAlgRun_Click(object sender, EventArgs e)
        {
            // !!Testing code!!
            // Graph A
            Graph graph = new Graph();
            graph.nodes.Add(new Node(graph, "Cammeringham Hill", new Point(20, 100)));
            graph.nodes.Add(new Node(graph, "Álta", new Point(300, 300)));
            graph.nodes.Add(new Node(graph, "B", new Point(700, 500)));
            graph.nodes.Add(new Node(graph, "Barton-upon-Humber", new Point(500, 700)));
            graph.nodes[0].JoinTo(graph.nodes[1], "", 8, ref IDCount);
            graph.nodes[0].JoinTo(graph.nodes[2], "", 3, ref IDCount);
            graph.nodes[0].JoinTo(graph.nodes[3], "", 5, ref IDCount);
            graph.nodes[1].JoinTo(graph.nodes[2], "", 1, ref IDCount);
            graph.nodes[1].JoinTo(graph.nodes[3], "", 2, ref IDCount);
            graph.nodes[2].JoinTo(graph.nodes[3], "", 9, ref IDCount);

            // Graph B
            Graph graph1 = new Graph();
            graph1.nodes.Add(new Node(graph1, "Riga", new Point(100, 100)));
            graph1.nodes.Add(new Node(graph1, "Dreilini", new Point(400, 200)));
            graph1.nodes.Add(new Node(graph1, "Valdlauči", new Point(200, 500)));
            graph1.nodes.Add(new Node(graph1, "Mārupe", new Point(20, 300)));
            graph1.nodes.Add(new Node(graph1, "Kekava", new Point(250, 800)));
            graph1.nodes.Add(new Node(graph1, "Saurieši", new Point(600, 300)));
            graph1.nodes[0].JoinTo(graph1.nodes[1], "", 8, ref IDCount);
            graph1.nodes[0].JoinTo(graph1.nodes[2], "", 3, ref IDCount);
            graph1.nodes[0].JoinTo(graph1.nodes[3], "", 4, ref IDCount);
            graph1.nodes[1].JoinTo(graph1.nodes[2], "", 1, ref IDCount);
            graph1.nodes[1].JoinTo(graph1.nodes[5], "", 6, ref IDCount);
            graph1.nodes[2].JoinTo(graph1.nodes[3], "", 2, ref IDCount);
            graph1.nodes[2].JoinTo(graph1.nodes[4], "", 5, ref IDCount);

            // Graph C
            Graph graph2 = new Graph();
            graph2.nodes.Add(new Node(graph2, "St Louis", new Point(500, 100)));
            graph2.nodes.Add(new Node(graph2, "Dallas", new Point(20, 200)));
            graph2.nodes.Add(new Node(graph2, "Memphis", new Point(700, 200)));
            graph2.nodes.Add(new Node(graph2, "Austin", new Point(15, 400)));
            graph2.nodes.Add(new Node(graph2, "Houston", new Point(550, 300)));
            graph2.nodes.Add(new Node(graph2, "New Orleans", new Point(750, 500)));
            graph2.nodes.Add(new Node(graph2, "San Antonio", new Point(300, 700)));
            graph2.nodes[0].JoinTo(graph2.nodes[1], "", 4, ref IDCount);
            graph2.nodes[1].JoinTo(graph2.nodes[3], "", 53, ref IDCount);
            graph2.nodes[4].JoinTo(graph2.nodes[5], "", 34, ref IDCount);
            graph2.nodes[5].JoinTo(graph2.nodes[6], "", 105, ref IDCount);
          

            DisplayGraph(graph2);
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.G)
            {
                Cursor = Cursors.SizeAll;

                foreach (Control c in this.Controls)
                {
                    // Remove all click events temporarily
                    if ((string)c.Tag == "Graph Part" && c is Button)
                    {
                        c.Click -= new EventHandler(HandleNodeClick);
                        // Using the Control.Draggable NuGet package
                        c.Draggable(true);
                    }
                }
            }
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.G)
            {
                Cursor = Cursors.Default;

                foreach (Control c in this.Controls)
                {
                    // Restore event handlers
                    if ((string)c.Tag == "Graph Part" && c is Button)
                    {
                        c.Click += new EventHandler(HandleNodeClick);
                        c.Draggable(false);
                    }
                }

                // Refresh graph after movement is done
                DisplayGraph(activeGraph);
            }
        }

        private void BackClicked(object sender, MouseEventArgs e)
        {
            if (rdbCreate.Checked)
            {
                activeGraph.nodes.Add(new Node(activeGraph, "", e.Location));
                DisplayGraph(activeGraph);
            }
        }

        private void ZoomLvlChanged(object sender, EventArgs e)
        {
            zoomLevel = (225-trbZoom.Value) / 100f;
            foreach (Control c in this.Controls)
            {
                if ((string)c.Tag == "Graph Part" || (string)c.Tag == "Edge Label")
                {
                    SizeF ratio = new SizeF(zoomLevel / prevZoomLevel, zoomLevel / prevZoomLevel);
                    Point oldLocation = c.Location;
                    c.Scale(ratio);
                    // Because for some reason this scales the controls about (0,0)
                    c.Location = oldLocation;

                    if (zoomLevel <= 0.5 && prevZoomLevel > 0.5)
                    {
                        if ((string)c.Tag == "Graph Part")
                        {
                            c.Text = ShortenText(c.Text);
                        }
                        else
                        {
                            activeEdge = FindArcFromID(Convert.ToInt32(c.Name));
                            if (activeEdge.GetWeight() == 0)
                            {
                                if (activeEdge.GetName() == "")
                                {
                                    c.Text = "_";
                                }
                                else
                                {
                                    c.Text = activeEdge.GetName();
                                }
                            }
                            else if (activeEdge.GetName() == "")
                            {
                                c.Text = "W: " + activeEdge.GetWeight();
                            }
                            else
                            {
                                c.Text = ShortenText(activeEdge.GetName()) + ", W: " + activeEdge.GetWeight();
                            }
                            
                        }
                    }
                    else if (zoomLevel > 0.5 && prevZoomLevel <= 0.5)
                    {
                        
                        if ((string)c.Tag == "Graph Part")
                        {
                            c.Text = c.Name;
                        }
                        else
                        {
                            activeEdge = FindArcFromID(Convert.ToInt32(c.Name));
                            if (activeEdge.GetWeight() == 0)
                            {
                                if (activeEdge.GetName() == "")
                                {
                                    c.Text = "_";
                                }
                                else
                                {
                                    c.Text = activeEdge.GetName();
                                }
                            }
                            else if (activeEdge.GetName() == "")
                            {
                                c.Text = "Weight = " + activeEdge.GetWeight();
                            }
                            else
                            {
                                c.Text = activeEdge.GetName() + ", weight = " + activeEdge.GetWeight();
                            }
                        }
                    }
                }
            }
            prevZoomLevel = zoomLevel;
        }

        private string ShortenText(string input)
        {
            string[] words = input.Split(' ','-');
            string output = "";
            foreach (string word in words)
            {
                if (word.Length > 0)
                {                    
                    output += word[0].ToString().ToUpper();
                }
            }
            return output;
        }
    }
}

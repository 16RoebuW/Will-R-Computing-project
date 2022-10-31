using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.RegularExpressions;

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
        public bool themeIsDark = false;
        public bool textIsLarge = false;


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

            // Check what happened last time
            try
            {
                string lastData = File.ReadAllText(Application.StartupPath + "\\AutosaveData.txt");
                if (lastData == "False")
                {
                    MessageBox.Show("The last graph loaded was not saved when the program closed. To recover lost work, the autosave can be found at: " + Application.StartupPath + "\\autosave.wrgf");
                }
            }
            catch { }
            autosaveTimer.Start();
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
        /// Returns a node from a list of nodes that has the name specified. Returns null if not found
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
            if (font.Size > cbxAlgorithmSelect.Font.Size)
            {
                textIsLarge = true;
            }
            else
            {
                textIsLarge = false;
            }
            cbxAlgorithmSelect.Font = font;
            statusLabel.Font = font;
            foreach (Control c in this.Controls)
            {
                if ((string)c.Tag == "Graph Part" || (string)c.Tag == "Edge Label")
                {
                    c.Font = font;
                }
            }
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

            Color backColour = SystemColors.Control;
            Color textColour = SystemColors.ControlText;
            if (themeIsDark)
            {
                backColour = SystemColors.ControlDarkDark;
                textColour = SystemColors.ControlLight;
            }

            Font font = new Font("Segoe UI", 9, FontStyle.Regular);
            if (textIsLarge)
            {
                font = new Font("Arial", 16, FontStyle.Bold);
            }

            foreach (Node n in input.nodes)
            {
                // Create button to represent node
                Button btnNode = new Button
                {
                    Location = n.location,
                    Name = n.name,
                    Tag = "Graph Part",
                    BackColor = backColour,
                    ForeColor = textColour,
                    Font = font,
                    MaximumSize = new Size((int)(300f * zoomLevel), (int)(70f * zoomLevel)),
                    MinimumSize = new Size((int)(0f * zoomLevel), (int)(70f * zoomLevel)),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
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
                            BackColor = backColour,
                            ForeColor = textColour,
                            Font = font,
                            MaximumSize = new Size((int)(300f * zoomLevel), (int)(70f * zoomLevel)),
                            MinimumSize = new Size((int)(0f * zoomLevel), (int)(70f * zoomLevel)),
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowAndShrink,
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
            activeGraph.wasSaved = false;
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
                        if (a.highlighted)
                        {
                            blackPen.Color = Color.Red;
                            blackPen.Width *= 1.25f;
                        }
                        e.Graphics.DrawLine(blackPen, a.between[0].location, a.between[1].location);
                        blackPen.Color = Color.Black;
                        blackPen.Width = 3 * zoomLevel * zoomLevel;
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
            activeGraph.wasSaved = false;
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
                        activeGraph.minWeight = Math.Min(activeGraph.minWeight, newWeight);
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
                        activeGraph.minWeight = Math.Min(activeGraph.minWeight, newWeight);
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

        /// <summary>
        /// Acronymises the input text
        /// </summary>
        /// <param name="input">Long text</param>
        /// <returns></returns>
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

        private void SaveClicked(object sender, EventArgs e)
        {            
            SaveFileDialog fileDialogue = new SaveFileDialog();
            fileDialogue.Filter = "WR Graph File|*.wrgf";
            fileDialogue.Title = "Save a graph";
            fileDialogue.AddExtension = true;
            DialogResult dialogueResult = fileDialogue.ShowDialog();

            if (dialogueResult == DialogResult.OK)
            {
                string path = fileDialogue.FileName;
                activeGraph.Save(path, false);
            }
        }

        private void LoadClicked(object sender, EventArgs e)
        {
            if (activeGraph.nodes.Count > 0 && !activeGraph.wasSaved)
            {
                DialogResult userAnswer = MessageBox.Show("Are you sure you want to overwrite the current graph?","Overwrite?",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userAnswer == DialogResult.No)
                {
                    return;
                }
            }
            OpenFileDialog fileDialogue = new OpenFileDialog();
            fileDialogue.Filter = "WR Graph File|*.wrgf";
            fileDialogue.Title = "Load a graph";
            DialogResult dialogueResult = fileDialogue.ShowDialog();

            if (dialogueResult == DialogResult.OK)
            {
                string path = fileDialogue.FileName;
                LoadGraph(path);
            }
            
        }

        /// <summary>
        /// Parses and validates wrgf files then displays them
        /// </summary>
        /// <param name="path">Location of the file</param>
        private void LoadGraph(string path)
        {           
            Graph graph = new Graph();
            string[] fileLines = File.ReadAllLines(path);
            string wholeFile = File.ReadAllText(path);
            if (fileLines[0] != "GRAPH FILE")
            {
                statusLabel.Text = "The file specified is not in the required format or is corrupted";
                return;
            }
            else
            {
                graph.minWeight = Convert.ToDouble(fileLines[1]);
                graph.nodeID = Convert.ToInt32(fileLines[2]);                

                // Regex to capture nodes
                string regex = @"{X=([0-9]+),Y=([0-9]+)}([^:]+)";               
                foreach (Match m in Regex.Matches(wholeFile, regex, RegexOptions.Multiline))
                {
                    Point location = new Point(Convert.ToInt32(m.Groups[1].Value), Convert.ToInt32(m.Groups[2].Value));
                    graph.nodes.Add(new Node(graph, m.Groups[3].Value, location));
                }

                // Now to find the arcs
                List<int> createdArcs = new List<int>();
                string[] colonSplit = wholeFile.Split(':');
                for (int i = 1; i < colonSplit.Length; i++)
                {
                    string[] arcs = colonSplit[i].Split('\n');
                    // The last item in this list will be a node or end of file, so we don't parse it here
                    for (int j = 0; j < arcs.Length - 1; j++)
                    {
                        string[] data = arcs[j].Split(',');
                        int tempId = Convert.ToInt32(data[0]);
                        if (!createdArcs.Contains(tempId))
                        {
                            createdArcs.Add(tempId);
                            Node destination = FindNodeWithName(graph.nodes, data[2]);
                            graph.nodes[i - 1].JoinTo(destination, data[1], Convert.ToDouble(data[3]), ref tempId);
                            IDCount = Math.Max(tempId, IDCount);                          
                        }
                    }
                }

                activeGraph = graph;
                graph.wasSaved = true;
                DisplayGraph(activeGraph);
            }
        }

        private void TimeTick(object sender, EventArgs e)
        {
            // Called every 5 minutes
            activeGraph.Save(Application.StartupPath + "\\autosave.wrgf", true);
        }

        private void ProgramClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(Application.StartupPath + "\\AutosaveData.txt", activeGraph.wasSaved.ToString());
            if (activeGraph.nodes.Count > 0 && !activeGraph.wasSaved)
            {
                DialogResult userAnswer = MessageBox.Show("Current graph is not saved, would you like to save?", "Save graph?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userAnswer == DialogResult.Yes)
                {
                    File.WriteAllText(Application.StartupPath + "\\AutosaveData.txt", "True");
                    SaveClicked(new object(), new EventArgs());
                }
                else if (userAnswer == DialogResult.No)
                {
                    File.WriteAllText(Application.StartupPath + "\\AutosaveData.txt", "True");
                }
            }
        }

        private void AlgorithmChosen(object sender, EventArgs e)
        {
            AlgorithmInputDialogue algorithmInput = new AlgorithmInputDialogue();
            algorithmInput.selectedAlgorithm = cbxAlgorithmSelect.Text;
            algorithmInput.ShowDialog(this);
        }

        /// <summary>
        /// Called from AlgorithmInputDialogue, parses, validates and carries out algorithms then displays the output
        /// </summary>
        /// <param name="selectedAlgorithm">The algorithm's name in the combo box</param>
        /// <param name="start">The name of the start node (Where applicable)</param>
        /// <param name="end">The name of the destination node (Where applicable)</param>
        public void RunAlgorithm(string selectedAlgorithm, string start, string end)
        {
            ClearHighlight(activeGraph);
            Node from = null;
            Node to = null;

            switch (selectedAlgorithm)
            {
                case "Shortest path (Accurate)":
                    from = FindNodeWithName(activeGraph.nodes, start);
                    to = FindNodeWithName(activeGraph.nodes, end);
                    if (from == null)
                    {
                        MessageBox.Show("Error, \"" + start + "\" does not exist on this graph");
                    }
                    else if (to == null)
                    {
                        MessageBox.Show("Error, \"" + end + "\" does not exist on this graph");
                    }
                    else
                    {
                        List<Node> route = activeGraph.Dijkstra(from, to);
                        if (route != null)
                        {
                            double sum = 0;
                            for (int i = 0; i < route.Count - 1; i++)
                            {
                                Arc arc = route[i].GetArcBetween(route[i + 1]);
                                arc.highlighted = true;
                                sum += arc.GetWeight();
                            }
                            DisplayGraph(activeGraph);
                            statusLabel.Text = "Route found, total weight = " + sum;
                        }
                        else
                        {
                            statusLabel.Text = "Error: no possible route found";
                        }
                    }
                    break;
                case "Shortest path (Fast)":
                    from = FindNodeWithName(activeGraph.nodes, start);
                    to = FindNodeWithName(activeGraph.nodes, end);

                    if (from == null)
                    {
                        MessageBox.Show("Error, \"" + start + "\" does not exist on this graph");
                    }
                    else if (to == null)
                    {
                        MessageBox.Show("Error, \"" + end + "\" does not exist on this graph");
                    }
                    else
                    {
                        List<Node> route = activeGraph.AStar(from, to);
                        if (route != null)
                        {
                            double sum = 0;
                            for (int i = 0; i < route.Count - 1; i++)
                            {
                                Arc arc = route[i].GetArcBetween(route[i + 1]);
                                arc.highlighted = true;
                                sum += arc.GetWeight();
                            }
                            DisplayGraph(activeGraph);
                            statusLabel.Text = "Route found, total weight = " + sum;
                        }
                        else
                        {
                            statusLabel.Text = "Error: no possible route found";
                        }
                    }
                    break;
                case "Compute all min distances":
                    double[,] minDistances = activeGraph.Floyds();
                    LeastDistancesDisplay outputDisplay = new LeastDistancesDisplay();
                    outputDisplay.distances = minDistances;
                    outputDisplay.nodes = activeGraph.nodes;
                    outputDisplay.Show();

                    break;
                case "Find shortest tour of all nodes":
                    break;
                case "Find minimum network containing all nodes (MST)":
                    List<Arc> MST = activeGraph.Prims();
                    if (MST != null)
                    {
                        foreach (Arc a in MST)
                        {
                            a.highlighted = true;
                        }
                        DisplayGraph(activeGraph);
                    }
                    else
                    {
                        statusLabel.Text = "Error: graph has unconnected nodes so there are no possible networks containing all nodes";
                    }
                    break;
            }
        }

        public void ClearHighlight(Graph graph)
        {
            foreach (Node n in graph.nodes)
            {
                foreach (Arc a in n.connections)
                {
                    a.highlighted = false;
                }
            }
        }
    }
}

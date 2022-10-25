using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphManager
{
    public partial class mainForm : Form
    {
        public Graph activeGraph = new Graph();

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

        /// <summary>
        /// Changes the fonts of some controls
        /// </summary>
        /// <param name="font">The font to be changed to</param>
        public void ChangeFonts(Font font)
        {
            cbxAlgorithmSelect.Font = font;
            statusLabel.Font = font;
        }

        public void DisplayGraph(Graph input)
        {
            //Draws lines between each node
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
                Controls.Add(btnNode);

                // Draw arcs to each connected node 
                // Each will be drawn twice due to the to/from nature of connections
                foreach (Arc a in n.connections)
                {
                    Button btnArc = new Button
                    {                       
                        // Centerpoint between nodes
                        Location = new Point((btnNode.Location.X + a.destination.location.X) / 2,
                                                (btnNode.Location.Y + a.destination.location.Y) / 2),
                        Tag = "Edge Label",
                        Size = new Size(75,75)
                    };

                    if (a.GetWeight() == 0)
                    {
                        btnArc.Text = "_";
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
                        e.Graphics.DrawLine(blackPen, n.location, a.destination.location);
                    }
                }
            }
        }

        private void btnAlgRun_Click(object sender, EventArgs e)
        {
            // !!Testing code!!
            Graph graph = new Graph();
            graph.nodes.Add(new Node(graph, "Cottonshopeburnfoot", new Point(500, 500)));
            graph.nodes.Add(new Node(graph, "", new Point(900, 400)));
            graph.nodes.Add(new Node(graph, "", new Point(200, 200)));
            graph.nodes[0].JoinTo(graph.nodes[1], "M2", 5);
            graph.nodes[1].JoinTo(graph.nodes[2], "", 0);
            DisplayGraph(graph);
        }
    }
}

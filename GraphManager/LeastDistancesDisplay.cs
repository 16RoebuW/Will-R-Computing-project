using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GraphManager
{
    public partial class LeastDistancesDisplay : Form
    {
        public double[,] distances;
        public List<Node> nodes;

        public LeastDistancesDisplay()
        {
            InitializeComponent();
        }

        private void DisplayOpened(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable("Min Distances");
            dataTable.Columns.Add(new DataColumn("Name:"));

            // Add a column for each node
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                dataTable.Columns.Add(new DataColumn(nodes[i].name, typeof(double)));
            }

            // Add the data from the table
            for (int j = 0; j < distances.GetLength(0); j++)
            {
                DataRow row = dataTable.NewRow();
                row["Name:"] = nodes[j].name;
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    row[nodes[i].name] = distances[i, j];
                }

                dataTable.Rows.Add(row);
            }

            // Style
            dataGridView.DataSource = dataTable;
            dataGridView.Font = new Font("Arial", 16, FontStyle.Bold);
            dataGridView.AutoSize = true;
            dataGridView.Refresh();

            // So that it displays on top of the initial form
            this.TopMost = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            for (int i = 0; i < distances.GetLength(0); i++)
            {
                dataTable.Columns.Add(new DataColumn(nodes[i].name));
            }

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

            dataGridView.DataSource = dataTable;
            dataGridView.Font = new Font("Arial", 16, FontStyle.Bold);
            dataGridView.AutoSize = true;
            dataGridView.Refresh();

            this.TopMost = true;
        }
    }
}

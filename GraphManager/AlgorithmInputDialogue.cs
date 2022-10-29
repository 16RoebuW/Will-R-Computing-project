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
    public partial class AlgorithmInputDialogue : Form
    {
        public string selectedAlgorithm;

        public AlgorithmInputDialogue()
        {
            InitializeComponent();
        }

        private void CloseDialogue(object sender, EventArgs e)
        {
            ((mainForm)Owner).RunAlgorithm(selectedAlgorithm, tbxStart.Text, tbxEnd.Text);
            this.Close();
        }

        private void FormOpened(object sender, EventArgs e)
        {
            switch (selectedAlgorithm)
            {
                case "Shortest path (Accurate)":
                    break;
                case "Shortest path (Fast)":
                    break;
                case "Find shortest tour of all nodes":
                    tbxEnd.Enabled = false;
                    break;
                default:
                    ((mainForm)Owner).RunAlgorithm(selectedAlgorithm, tbxStart.Text, tbxEnd.Text);
                    this.Close();
                    break;
            }
        }
    }
}

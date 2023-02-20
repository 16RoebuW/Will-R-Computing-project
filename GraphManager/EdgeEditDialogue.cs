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
    public partial class EdgeEditDialogue : Form
    {
        public EdgeEditDialogue()
        {
            InitializeComponent();
        }

        // Called when the form closes
        private void CloseDialogue(object sender, EventArgs e)
        {
            if (tbxName.Text == "" && tbxWeight.Text == "")
            {
                // No changes
                ((MainForm)Owner).activeEdge = null;
                Close();
            }
            else if(tbxName.Text == "")
            {
                // Weight change only
                // Type check
                try
                {
                    double weight = Convert.ToDouble(tbxWeight.Text);
                    ((MainForm)Owner).EditEdge(2, "", weight);
                    Close();
                }
                catch
                {
                    MessageBox.Show("Weight must be a number");
                }
            }
            else if(tbxWeight.Text == "")
            {
                // Name change only
                ((MainForm)Owner).EditEdge(1, tbxName.Text, -1);
                Close();
            }
            else
            {
                // Name and weight change
                // Type check
                try
                {
                    double weight = Convert.ToDouble(tbxWeight.Text);
                    ((MainForm)Owner).EditEdge(0, tbxName.Text, weight);
                    Close();
                }
                catch
                {
                    MessageBox.Show("Weight must be a number");
                }
            }

        }
    }
}

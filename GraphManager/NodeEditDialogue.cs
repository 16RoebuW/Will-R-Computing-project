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
    public partial class NodeEditDialogue : Form
    {
        public NodeEditDialogue()
        {
            InitializeComponent();
        }

        // Called when the close button is clicked
        private void CloseDialogue(object sender, EventArgs e)
        {
            if (tbxName.Text == "")
            {
                // Only change the data if there has been an input (Presence check)
                ((MainForm)Owner).activeEdge = null;
                Close();
            }
            else
            {
                ((MainForm)Owner).EditNode(tbxName.Text);
                Close();
            }
        }

        // Called when data in the text box is changed
        private void Name_TextChanged(object sender, EventArgs e)
        {
            // This warning does not prevent the user from using the name, as a length of 30 is acceptable and the textbox already limits the length of inputs.
            // The main purpose is to explain to users why they can't type anymore to eliminate confusion
            if (tbxName.Text.Length == 30)
            {
                MessageBox.Show("Max name length is 30");
            }
        }
    }
}

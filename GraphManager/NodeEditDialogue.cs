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

        private void CloseDialogue(object sender, EventArgs e)
        {
            if (tbxName.Text == "")
            {
                ((MainForm)Owner).activeEdge = null;
                Close();
            }
            else
            {
                ((MainForm)Owner).EditNode(tbxName.Text);
                Close();
            }
        }

        private void Name_TextChanged(object sender, EventArgs e)
        {
            if (tbxName.Text.Length == 30)
            {
                MessageBox.Show("Max name length is 30");
            }
        }
    }
}

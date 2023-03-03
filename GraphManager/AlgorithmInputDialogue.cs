using System;
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

        // Called when the done button is clicked (either by the mouse or the enter key)
        private void CloseDialogue(object sender, EventArgs e)
        {
            ((MainForm)Owner).RunAlgorithm(selectedAlgorithm, tbxStart.Text, tbxEnd.Text);
            this.Close();
        }

        // Called when the form is opened and displays the correct boxes or runs the algorithm depending on the dropdown box selection
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
                    // In the case that no inputs are required, run the algorithm straight away
                    ((MainForm)Owner).RunAlgorithm(selectedAlgorithm, tbxStart.Text, tbxEnd.Text);
                    this.Close();
                    break;
            }
        }
    }
}

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
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void openOptions(object sender, EventArgs e)
        {
            OptionsForm optionsBox = new OptionsForm();
            optionsBox.Show();
            
        }
    }
}

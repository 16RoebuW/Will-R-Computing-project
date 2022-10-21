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
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void closeOptions(object sender, EventArgs e)
        {
            this.Close();
        }

        private void itemChecked(object sender, ItemCheckEventArgs e)
        {
            //Called before the item is actually checked/unchecked
            switch (optionsBox.SelectedIndex)
            {
                case 1:
                    // Dark theme

                    // Default colours:
                    Color backColour = SystemColors.Control;
                    Color textColour = SystemColors.ControlText;
                    if (e.CurrentValue == CheckState.Unchecked)
                    {
                        // Dark colours:
                        backColour = SystemColors.ControlDarkDark;
                        textColour = SystemColors.ControlLight;
                    }

                    foreach (Form f in Application.OpenForms)
                    {
                        f.BackColor = backColour;
                        foreach (Control c in f.Controls)
                        {
                            if (c.Name == "darkThemeIndicator")
                            {
                                c.BackColor = textColour;
                            }
                            else
                            {                            
                                c.BackColor = backColour;
                                c.ForeColor = textColour;
                            }
                            foreach (Control nestedControl in c.Controls)
                            {
                                nestedControl.BackColor = backColour;
                                nestedControl.ForeColor = textColour;
                            }
                        }
                    }

                    break;
                    
            }
            
        }

        public List<Control> GetAllControls(Form f)
        {
            List<Control> resultList = new List<Control>();
            foreach (Control c in f.Controls)
            {
                resultList.Add(c);
            }
            return resultList;
        }
    }
}

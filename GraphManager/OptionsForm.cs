using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphManager
{
    public partial class OptionsForm : Form
    {
        string cfgFilePath = Application.StartupPath + "\\options.cfg";
        bool themeIsDark = false;
        bool textIsLarge = false;

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void CloseOptions(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ItemChecked(object sender, ItemCheckEventArgs e)
        {
            // Called before the item is actually checked/unchecked
            switch (optionsBox.SelectedIndex)
            {
                case 0:
                    // Large text

                    // Default font
                    Font font = new Font("Segoe UI", 9, FontStyle.Regular);
                    textIsLarge = false;
                    if (e.CurrentValue == CheckState.Unchecked)
                    {
                        // Large font
                        font = new Font("Arial", 16, FontStyle.Bold);
                        textIsLarge = true;
                    }

                    foreach (Form f in Application.OpenForms)
                    {
                        // Checks if any open form is a mainForm, if so, change its fonts
                        try
                        {
                            ((mainForm)f).ChangeFonts(font);
                        }
                        catch (Exception error)
                        { }
                    }

                        break;
                case 1:
                    // Dark theme

                    // Default colours:
                    Color backColour = SystemColors.Control;
                    Color textColour = SystemColors.ControlText;
                    themeIsDark = false;
                    if (e.CurrentValue == CheckState.Unchecked)
                    {
                        // Dark colours:
                        backColour = SystemColors.ControlDarkDark;
                        textColour = SystemColors.ControlLight;
                        themeIsDark = true;
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
                                SetControlColours(c, backColour, textColour);
                            }
                            foreach (Control nestedControl in c.Controls)
                            {
                                SetControlColours(nestedControl, backColour, textColour);
                            }
                        }
                    }
                    break;
            }

            WriteOpsToFile(cfgFilePath);
        }

        /// <summary>
        /// Saves options to a config file
        /// </summary>
        /// <param name="cfgFilePath">The location of the file</param>
        private void WriteOpsToFile(string cfgFilePath)
        {
            string data = "";
            for (int i = 0; i < optionsBox.Items.Count; i++)
            {
                // The condition after the '^' (XOR operator) is necessary due to the fact this is called before the item is actually checked/unchecked
                if (optionsBox.GetItemCheckState(i) == CheckState.Checked ^ optionsBox.SelectedIndex == i)
                {
                    data += "1";
                }
                else
                {
                    data += "0";
                }
                data += "\n";
            }
            File.WriteAllText(cfgFilePath, data);
        }

        /// <summary>
        /// Sets the foreColor and backColor of a control
        /// </summary>
        /// <param name="control">The control to have its colours changed</param>
        /// <param name="backColour">The desired back colour</param>
        /// <param name="textColour">The desired fore/text colour</param>
        private void SetControlColours(Control control, Color backColour, Color textColour)
        {
            // This is a list of all controls that must have a white back colour
            if ((control.Name == "cbxAlgorithmSelect" || control.Name == "optionsBox" || control.Name == "") && !themeIsDark)
            {
                control.BackColor = SystemColors.Window;
            }
            else
            {
                control.BackColor = backColour;
            }
            control.ForeColor = textColour;
        }

        private void OptionsLoaded(object sender, EventArgs e)
        {
            // When this form opens, check the boxes for the options that were saved in the file
            string[] storedOps = File.ReadAllLines(cfgFilePath);
            for (int i = 0; i < storedOps.Length; i++)
            {
                if (storedOps[i] == "1")
                {
                    // Have to mimic a user clicking the boxes so that changes are handled correctly
                    optionsBox.SelectedIndex = i;
                    ItemChecked(optionsBox, new ItemCheckEventArgs(i, CheckState.Checked, CheckState.Unchecked));
                    optionsBox.SetItemCheckState(i, CheckState.Checked);
                }
            }
        }
    }
}

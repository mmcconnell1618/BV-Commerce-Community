﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BVSoftware.Commerce.MigrationWindows
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            LoadRadioButtons();
        }

        private void LoadRadioButtons()
        {
            if (Properties.Settings.Default.SourceType == Migration.MigrationSourceType.BVC2004)
            {
                this.rb2004.Checked = true;
            }
            else
            {
                this.rb5.Checked = true;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void SaveSettings()
        {
            if (this.rb2004.Checked)
            {
                Properties.Settings.Default.SourceType = Migration.MigrationSourceType.BVC2004;
            }
            else
            {
                Properties.Settings.Default.SourceType = Migration.MigrationSourceType.BV5;
            }

            Properties.Settings.Default.Save();
        }

        private void btnBrowseForImagesRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = this.ImagesRootFolderField.Text;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ImagesRootFolderField.Text = fd.SelectedPath;
            }
        }
    }
}

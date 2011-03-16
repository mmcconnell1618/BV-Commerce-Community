﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BVSoftware.Commerce.Migration;

namespace BVSoftware.Commerce.MigrationWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version " + Application.ProductVersion;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OptionsForm s = new OptionsForm();
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MigrationSettings settings = new MigrationSettings();
                LoadSettingsFromSaved(settings);

                WorkerForm worker = new WorkerForm();
                worker.Show();
                worker.Focus();
                worker.StartMigration(settings);                
            }
            else
            {
                this.Focus();
            }
        }

        private void LoadSettingsFromSaved(MigrationSettings s)
        {
            s.ApiKey = Properties.Settings.Default.ApiKey;
            s.ClearAffiliates = Properties.Settings.Default.ClearAffiliates;
            s.ClearCategories = Properties.Settings.Default.ClearCategories;
            s.ClearOrders = Properties.Settings.Default.ClearOrders;
            s.ClearProducts = Properties.Settings.Default.ClearProducts;
            s.DestinationServiceRootUrl = Properties.Settings.Default.DestinationRootUrl;
            s.ImportAffiliates = Properties.Settings.Default.ImportAffiliates;
            s.ImportCategories = Properties.Settings.Default.ImportCategories;
            s.ImportOrders = Properties.Settings.Default.ImportOrders;
            s.ImportOtherSettings = Properties.Settings.Default.ImportOther;
            s.ImportProducts = Properties.Settings.Default.ImportProducts;
            s.ImportUsers = Properties.Settings.Default.ImportUsers;
            if (Properties.Settings.Default.SingleOrderOn)
            {
                s.SingleOrderImport = Properties.Settings.Default.SingleOrderImport;
            }
            if (Properties.Settings.Default.SingleSkuOn)
            {
                s.SingleSkuImport = Properties.Settings.Default.SingleSkuImport;
            }
            s.SourceType = Properties.Settings.Default.SourceType;
            s.SQLDatabase = Properties.Settings.Default.SQLDatabase;
            s.SQLServer = Properties.Settings.Default.SQLServer;
            s.SQLUsername = Properties.Settings.Default.SQLUsername;
            s.SQLPassword = Properties.Settings.Default.SQLPassword;
            s.UseMetricUnits = Properties.Settings.Default.UseMetricUnits;

            s.ImagesRootFolder = Properties.Settings.Default.ImagesRootFolder;

            s.PrepArgs();
        }       
    }
}
using System;
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
    public partial class WorkerForm : Form
    {
        private MigrationSettings _Settings { get; set; }

        public WorkerForm()
        {            
            InitializeComponent();
            _Settings = null;
        }

        public delegate void WorkerCancelRequested();
        public event WorkerCancelRequested CancelRequested;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.backgroundWorker1.CancelAsync();
        }

        public void StartMigration(MigrationSettings data)
        {            
            _Settings = data;
            this.OutputField.Text = string.Empty;
            this.backgroundWorker1.RunWorkerAsync(data);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MigrationSettings settings = (MigrationSettings)e.Argument;
            MigrationService svc = new MigrationService(settings);
            svc.ProgressReport += new MigrationService.ProgressReportDelegate(svc_ProgressReport);
            svc.StartMigration();            
        }

        void svc_ProgressReport(string message)
        {            
            this.backgroundWorker1.ReportProgress(0, message);            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.OutputField.AppendText((string)e.UserState + System.Environment.NewLine);            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.btnClose.Enabled = true;

            // Dump Log File
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string logPath = System.IO.Path.Combine(appPath, "Log.txt");
            System.IO.File.WriteAllText(logPath, this.OutputField.Text);            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}

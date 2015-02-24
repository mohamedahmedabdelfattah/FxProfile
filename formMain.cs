using System;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace FxProfile
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }

        string FxProfilePath = Environment.ExpandEnvironmentVariables("%APPDATA%\\Mozilla\\Firefox");
        string FxProfilePathAlt = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Mozilla\Firefox";
        
        private void folderPath()
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                FxProfilePath = folderBrowserDialog1.SelectedPath;
            }
            else
                FxProfilePath = FxProfilePathAlt;
        }

        private void btnBrowseFolders_Click(object sender, EventArgs e)
        {
            folderPath();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            saveFileDialog1.Filter = "FxProfile files (*.fxbu)|*.fxbu";

            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                if ((File.Exists(saveFileDialog1.FileName)))
                {
                    File.Delete(saveFileDialog1.FileName);
                }

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            openFileDialog1.Filter = "FxProfile files (*.fxbu)|*.fxbu";

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                backgroundWorker2.RunWorkerAsync();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:mohamed.aabdelfattah@tedata.net?subject=FxProfile "+label2.Text);
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            btnBackup.Enabled = false;
            btnRestore.Enabled = false;
            timer1.Enabled = true;

            try
            {
                ZipFile.CreateFromDirectory(FxProfilePath, saveFileDialog1.FileName);
                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                progressBar1.Value = 0;
                MessageBox.Show(ex.ToString());

                if ((File.Exists(saveFileDialog1.FileName)))
                {
                    File.Delete(saveFileDialog1.FileName);
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            timer1.Enabled = false;

            if (progressBar1.Value == 100)
                MessageBox.Show("Backup created successfully");
            else
                progressBar1.Value = 0;

            btnBackup.Enabled = true;
            btnRestore.Enabled = true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void formMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'f')
                folderPath();
            else if (e.KeyChar == 'p')
            {   
                //Test
                MessageBox.Show(FxProfilePath);
                MessageBox.Show(FxProfilePathAlt);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value != 100)
                progressBar1.Value += 1;

        }

        private void formMain_Load(object sender, EventArgs e)
        {
            this.Focus();
            richTextBox1.Focus();
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            btnBackup.Enabled = false;
            btnRestore.Enabled = false;
            timer1.Enabled = true;

            try
            {
                if (!Directory.Exists(FxProfilePath))
                {
                    Directory.CreateDirectory(FxProfilePath);
                }

                ZipFile.ExtractToDirectory(openFileDialog1.FileName, FxProfilePath);
                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                progressBar1.Value = 0;
                MessageBox.Show(ex.ToString());
            }

        }

        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            timer1.Enabled = false;

            if(progressBar1.Value == 100)
                MessageBox.Show("Data restored successfully");
            else
                progressBar1.Value = 0;

            btnBackup.Enabled = true;
            btnRestore.Enabled = true;
        }
    }
}
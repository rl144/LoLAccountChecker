#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PVPNetConnect;

#endregion

namespace LoL_Account_Checker
{
    public partial class Form1 : Form
    {
        private List<AccountData> _accountList;

        public Form1()
        {
            InitializeComponent();

            regionsComboBox.SelectedIndex = 0;
        }

        private void InputFileTextBoxDClick(object sender, MouseEventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                inputFileTextBox.Text = ofd.FileName;
            }
        }

        private void OutputFileTextBoxDClick(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = "output" + (ExportHTML.Checked ? ".html" : ".txt"),
                Filter = ExportHTML.Checked ? "HTML (.html)|" : "Text file (.txt)|"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                outputFileTextBox.Text = sfd.FileName;
            }
        }

        private void CheckAcccountsClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputFileTextBox.Text))
            {
                MessageBox.Show(@"Account's file missing.");
                return;
            }

            if (string.IsNullOrEmpty(outputFileTextBox.Text))
            {
                MessageBox.Show(@"Output file missing.");
                return;
            }


            if (!File.Exists(inputFileTextBox.Text))
            {
                MessageBox.Show(@"Account's file does not exist.");
                return;
            }

            button1.Enabled = false;

            var region = (Region) regionsComboBox.SelectedIndex;

            _accountList = new List<AccountData>();

            var bw = new BackgroundWorker { WorkerReportsProgress = true };

            bw.DoWork += (o, args) =>
            {
                try
                {
                    var b = o as BackgroundWorker;

                    var sr = new StreamReader(inputFileTextBox.Text);

                    var totalLines = sr.ReadToEnd().Split(new[] { '\n' }).Count();

                    var lineCount = 0;

                    sr.DiscardBufferedData();
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    sr.BaseStream.Position = 0;

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var accountData = line.Split(new[] { ':' });

                        if (accountData.Count() < 2)
                        {
                            continue;
                        }

                        var username = accountData[0];
                        var password = accountData[1];

                        var client = new Client(region, username, password);

                        while (true)
                        {
                            if (client.Completed)
                            {
                                break;
                            }

                            Thread.Sleep(1000);
                        }

                        _accountList.Add(client.Data);
                        client.Disconnect();

                        Console.WriteLine(@"[{0:HH:mm}] <{1}> Completed!", DateTime.Now, client.Data.Username);

                        lineCount++;

                        if (b != null)
                        {
                            b.ReportProgress((lineCount * 100) / totalLines);
                        }
                    }

                    if (ExportHTML.Checked)
                    {
                        Export.ExportAsHTML(outputFileTextBox.Text, _accountList, ExportErrors.Checked);
                    }
                    else
                    {
                        Export.ExportAsText(outputFileTextBox.Text, _accountList, ExportErrors.Checked);
                    }

                    sr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            };

            bw.ProgressChanged += (o, args) => { toolStripProgressBar1.Value = args.ProgressPercentage; };

            bw.RunWorkerCompleted += (o, args) =>
            {
                button1.Enabled = true;

                if (
                    MessageBox.Show(
                        "Finished!\nWanna see the results?", @"LoL Account Checker", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    var file = outputFileTextBox.Text;

                    Process.Start(file);
                }
            };

            bw.RunWorkerAsync();
        }

        private void InputFileOnDragDrop(object sender, DragEventArgs e)
        {
            var path = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            var b = new StringBuilder();
            foreach (var c in path)
            {
                b.Append(c);
            }

            inputFileTextBox.Text = b.ToString();
        }

        private void InputFileOnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void ExportHTML_OnChangeChecked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(outputFileTextBox.Text))
            {
                return;
            }

            var newExtension = ExportHTML.Checked ? ".html" : ".txt";

            var newFile = Path.ChangeExtension(outputFileTextBox.Text, newExtension);

            if (newFile == outputFileTextBox.Text)
            {
                return;
            }


            if (File.Exists(newFile))
            {
                if (
                    MessageBox.Show(
                        "There is already a file with the same name of the \"Output\" file.\nDo you wanna overwrite?",
                        @"LoL Account Checker", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    ExportHTML.Checked = !ExportHTML.Checked;
                    return;
                }
            }

            outputFileTextBox.Text = newFile;
        }
    }
}
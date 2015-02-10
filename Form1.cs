#region

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PVPNetConnect;

#endregion

namespace LoL_Account_Checker
{
    internal delegate void SetControlsEnabled(bool value);

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            regionsComboBox.SelectedIndex = 0;
            Checker.OnFinishedChecking += Checker_OnFinishedChecking;
            Checker.OnNewAccount += Checker_OnNewAccount;

            toolStripSpacer1.Spring = true;
            toolStripSpacer1.Text = string.Empty;

            toolStripNewAccount.Text = string.Empty;
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

            ChangeControlsStatus(false);

            var region = (Region) regionsComboBox.SelectedIndex;
            var logins = Utils.GetLogins(inputFileTextBox.Text);

            toolStripProgressBar1.Value = 0;
            toolStripNewAccount.Text = string.Empty;

            var thread =
                new Thread(
                    () =>
                        Checker.Start(
                            logins, region, (int) numericUpDown1.Value, outputFileTextBox.Text, ExportHTML.Checked));

            thread.Start();
        }

        private void InputFileOnDragDrop(object sender, DragEventArgs e)
        {
            if (!inputFileTextBox.Enabled)
            {
                return;
            }

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

        private void ChangeControlsStatus(bool value)
        {
            if (InvokeRequired)
            {
                var d = new SetControlsEnabled(ChangeControlsStatus);
                Invoke(d, new object[] { value });
                return;
            }

            // Input file
            inputLabel.Enabled = value;
            inputFileTextBox.Enabled = value;

            // Ouput file
            outputLabel.Enabled = value;
            outputFileTextBox.Enabled = value;

            // Regions
            regionLabel.Enabled = value;
            regionsComboBox.Enabled = value;

            // Config group
            ExportErrors.Enabled = value;
            ExportHTML.Enabled = value;
            numericUpDown1.Enabled = value;


            // Checker Button
            button1.Enabled = value;
        }

        private void Checker_OnNewAccount(AccountData account)
        {
            if (InvokeRequired)
            {
                var d = new NewAccountChecked(Checker_OnNewAccount);
                Invoke(d, new object[] { account });
                return;
            }

            toolStripProgressBar1.Value = Checker.Percentage;
            toolStripNewAccount.Text = string.Format("{0}: {1}", account.Username, account.Result);
        }

        private void Checker_OnFinishedChecking(EventArgs args)
        {
            if (InvokeRequired)
            {
                var d = new FinishedChecking(Checker_OnFinishedChecking);
                Invoke(d, new object[] { args });
                return;
            }

            toolStripProgressBar1.Value = 100;
            toolStripNewAccount.Text = @"Finished!";

            // Export
            if (ExportHTML.Checked)
            {
                Utils.ExportAsHtml(outputFileTextBox.Text, Checker.Accounts, ExportErrors.Checked);
            }
            else
            {
                Utils.ExportAsText(outputFileTextBox.Text, Checker.Accounts, ExportErrors.Checked);
            }


            if (MessageBox.Show("Finished!\nWanna see the results?", @"LoL Account Checker", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                var file = outputFileTextBox.Text;

                Process.Start(file);
            }

            // Enable controlls
            ChangeControlsStatus(true);
        }

        private void donateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var msgBox = MessageBox.Show(
                "If you wanna pay me a beer you can do it via paypal.\n" +
                "For other methods, send me a private message on http://nulled.io or http://joduska.me \n" +
                "Wanna donate via paypal?", "Donate", MessageBoxButtons.YesNo);

            if (msgBox == DialogResult.Yes)
            {
                Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CHEV6LWPMHUMW");
            }
        }
    }
}
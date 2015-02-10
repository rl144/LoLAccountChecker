namespace LoL_Account_Checker
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputLabel = new System.Windows.Forms.Label();
            this.inputFileTextBox = new System.Windows.Forms.TextBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.outputFileTextBox = new System.Windows.Forms.TextBox();
            this.regionLabel = new System.Windows.Forms.Label();
            this.regionsComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSpacer1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripNewAccount = new System.Windows.Forms.ToolStripStatusLabel();
            this.ExportErrors = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ExportHTML = new System.Windows.Forms.CheckBox();
            this.donateLink = new System.Windows.Forms.LinkLabel();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(10, 18);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(55, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Accounts:";
            // 
            // inputFileTextBox
            // 
            this.inputFileTextBox.AllowDrop = true;
            this.inputFileTextBox.Location = new System.Drawing.Point(71, 15);
            this.inputFileTextBox.Name = "inputFileTextBox";
            this.inputFileTextBox.Size = new System.Drawing.Size(208, 20);
            this.inputFileTextBox.TabIndex = 1;
            this.inputFileTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputFileOnDragDrop);
            this.inputFileTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputFileOnDragEnter);
            this.inputFileTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InputFileTextBoxDClick);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(23, 44);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(42, 13);
            this.outputLabel.TabIndex = 2;
            this.outputLabel.Text = "Output:";
            // 
            // outputFileTextBox
            // 
            this.outputFileTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.outputFileTextBox.Location = new System.Drawing.Point(71, 41);
            this.outputFileTextBox.Name = "outputFileTextBox";
            this.outputFileTextBox.Size = new System.Drawing.Size(208, 20);
            this.outputFileTextBox.TabIndex = 1;
            this.outputFileTextBox.DoubleClick += new System.EventHandler(this.OutputFileTextBoxDClick);
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.Location = new System.Drawing.Point(21, 70);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(44, 13);
            this.regionLabel.TabIndex = 3;
            this.regionLabel.Text = "Region:";
            // 
            // regionsComboBox
            // 
            this.regionsComboBox.DisplayMember = "1";
            this.regionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionsComboBox.FormattingEnabled = true;
            this.regionsComboBox.Items.AddRange(new object[] {
            "NA",
            "EUW",
            "EUN",
            "KR",
            "BR",
            "TR",
            "RU",
            "LA1",
            "LA2",
            "PBE",
            "SG",
            "MY",
            "SGMY",
            "TW",
            "TH",
            "PH",
            "VN",
            "OCE",
            "LAN",
            "LAS"});
            this.regionsComboBox.Location = new System.Drawing.Point(71, 67);
            this.regionsComboBox.Name = "regionsComboBox";
            this.regionsComboBox.Size = new System.Drawing.Size(208, 21);
            this.regionsComboBox.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(214, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CheckAcccountsClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripSpacer1,
            this.toolStripNewAccount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 214);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(291, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripSpacer1
            // 
            this.toolStripSpacer1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripSpacer1.Name = "toolStripSpacer1";
            this.toolStripSpacer1.Size = new System.Drawing.Size(93, 17);
            this.toolStripSpacer1.Text = "toolStripSpacer1";
            // 
            // toolStripNewAccount
            // 
            this.toolStripNewAccount.Name = "toolStripNewAccount";
            this.toolStripNewAccount.Size = new System.Drawing.Size(0, 17);
            this.toolStripNewAccount.Text = "toolStripSpacer1";
            // 
            // ExportErrors
            // 
            this.ExportErrors.AutoSize = true;
            this.ExportErrors.Location = new System.Drawing.Point(6, 19);
            this.ExportErrors.Name = "ExportErrors";
            this.ExportErrors.Size = new System.Drawing.Size(85, 17);
            this.ExportErrors.TabIndex = 7;
            this.ExportErrors.Text = "Export errors";
            this.ExportErrors.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.ExportHTML);
            this.groupBox1.Controls.Add(this.ExportErrors);
            this.groupBox1.Location = new System.Drawing.Point(12, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 68);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Config";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(164, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Threads:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(219, 18);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(42, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ExportHTML
            // 
            this.ExportHTML.AutoSize = true;
            this.ExportHTML.Location = new System.Drawing.Point(6, 42);
            this.ExportHTML.Name = "ExportHTML";
            this.ExportHTML.Size = new System.Drawing.Size(103, 17);
            this.ExportHTML.TabIndex = 8;
            this.ExportHTML.Text = "Export as HTML";
            this.ExportHTML.UseVisualStyleBackColor = true;
            this.ExportHTML.CheckedChanged += new System.EventHandler(this.ExportHTML_OnChangeChecked);
            // 
            // donateLink
            // 
            this.donateLink.AutoSize = true;
            this.donateLink.Location = new System.Drawing.Point(237, 198);
            this.donateLink.Name = "donateLink";
            this.donateLink.Size = new System.Drawing.Size(42, 13);
            this.donateLink.TabIndex = 9;
            this.donateLink.TabStop = true;
            this.donateLink.Text = "Donate";
            this.donateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.donateLink_LinkClicked);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 236);
            this.Controls.Add(this.donateLink);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.regionsComboBox);
            this.Controls.Add(this.regionLabel);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.outputFileTextBox);
            this.Controls.Add(this.inputFileTextBox);
            this.Controls.Add(this.inputLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "LoL Account Checker";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.TextBox inputFileTextBox;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.TextBox outputFileTextBox;
        private System.Windows.Forms.Label regionLabel;
        private System.Windows.Forms.ComboBox regionsComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox ExportErrors;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ExportHTML;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSpacer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNewAccount;
        private System.Windows.Forms.LinkLabel donateLink;
    }
}


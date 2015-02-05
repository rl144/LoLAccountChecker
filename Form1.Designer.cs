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
            this.label1 = new System.Windows.Forms.Label();
            this.inputFileTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.outputFileTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.regionsComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.ExportErrors = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExportHTML = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Accounts:";
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output:";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Region:";
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
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 207);
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
            this.groupBox1.Controls.Add(this.ExportHTML);
            this.groupBox1.Controls.Add(this.ExportErrors);
            this.groupBox1.Location = new System.Drawing.Point(12, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 68);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Config";
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
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 229);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.regionsComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outputFileTextBox);
            this.Controls.Add(this.inputFileTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "LoL Account Checker";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputFileTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputFileTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox regionsComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox ExportErrors;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ExportHTML;
    }
}


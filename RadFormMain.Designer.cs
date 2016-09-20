
using System.Windows.Forms;

namespace CSV2QFX
{
    partial class RadFormMain
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
            this.radLabel1 = new System.Windows.Forms.Label();
            this.radTextBoxCSVPath = new System.Windows.Forms.TextBox();
            this.radGroupBoxDrop = new System.Windows.Forms.GroupBox();
            this.radLabelLastRunDate = new System.Windows.Forms.Label();
            this.radButtonLaunchQuicken = new System.Windows.Forms.Button();
            this.radLabel5 = new System.Windows.Forms.Label();
            this.radLabelManualInputDetails = new System.Windows.Forms.Label();
            this.radLabel3 = new System.Windows.Forms.Label();
            this.radButtonManualInput = new System.Windows.Forms.Button();
            this.radDropDownListAccountNo = new System.Windows.Forms.ComboBox();
            this.radLabel2 = new System.Windows.Forms.Label();
            this.radButtonConvert = new System.Windows.Forms.Button();
            this.radLabelPreviousRunDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radGroupBoxDrop.SuspendLayout();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(5, 21);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(86, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "CSV to convert :";
            // 
            // radTextBoxCSVPath
            // 
            this.radTextBoxCSVPath.AllowDrop = true;
            this.radTextBoxCSVPath.Location = new System.Drawing.Point(93, 21);
            this.radTextBoxCSVPath.Name = "radTextBoxCSVPath";
            this.radTextBoxCSVPath.Size = new System.Drawing.Size(458, 20);
            this.radTextBoxCSVPath.TabIndex = 1;
            this.radTextBoxCSVPath.TabStop = false;
            // 
            // radGroupBoxDrop
            // 
            this.radGroupBoxDrop.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxDrop.AllowDrop = true;
            this.radGroupBoxDrop.Controls.Add(this.radLabelPreviousRunDate);
            this.radGroupBoxDrop.Controls.Add(this.label2);
            this.radGroupBoxDrop.Controls.Add(this.radLabelLastRunDate);
            this.radGroupBoxDrop.Controls.Add(this.radButtonLaunchQuicken);
            this.radGroupBoxDrop.Controls.Add(this.radLabel5);
            this.radGroupBoxDrop.Controls.Add(this.radLabelManualInputDetails);
            this.radGroupBoxDrop.Controls.Add(this.radLabel3);
            this.radGroupBoxDrop.Controls.Add(this.radButtonManualInput);
            this.radGroupBoxDrop.Controls.Add(this.radDropDownListAccountNo);
            this.radGroupBoxDrop.Controls.Add(this.radLabel2);
            this.radGroupBoxDrop.Controls.Add(this.radButtonConvert);
            this.radGroupBoxDrop.Controls.Add(this.radLabel1);
            this.radGroupBoxDrop.Controls.Add(this.radTextBoxCSVPath);
            this.radGroupBoxDrop.Location = new System.Drawing.Point(13, 13);
            this.radGroupBoxDrop.Name = "radGroupBoxDrop";
            this.radGroupBoxDrop.Size = new System.Drawing.Size(571, 175);
            this.radGroupBoxDrop.TabIndex = 2;
            this.radGroupBoxDrop.TabStop = false;
            this.radGroupBoxDrop.Text = "Convert and Import";
            this.radGroupBoxDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.radGroupBoxDrop_DragEnter);
            this.radGroupBoxDrop.DragOver += new System.Windows.Forms.DragEventHandler(this.radGroupBoxDrop_DragOver);
            // 
            // radLabelLastRunDate
            // 
            this.radLabelLastRunDate.ForeColor = System.Drawing.Color.DarkGreen;
            this.radLabelLastRunDate.Location = new System.Drawing.Point(445, 110);
            this.radLabelLastRunDate.Name = "radLabelLastRunDate";
            this.radLabelLastRunDate.Size = new System.Drawing.Size(106, 18);
            this.radLabelLastRunDate.TabIndex = 9;
            this.radLabelLastRunDate.Text = "N/A";
            this.radLabelLastRunDate.Click += new System.EventHandler(this.radLabelLastRunDate_Click);
            // 
            // radButtonLaunchQuicken
            // 
            this.radButtonLaunchQuicken.Location = new System.Drawing.Point(9, 136);
            this.radButtonLaunchQuicken.Name = "radButtonLaunchQuicken";
            this.radButtonLaunchQuicken.Size = new System.Drawing.Size(110, 24);
            this.radButtonLaunchQuicken.TabIndex = 6;
            this.radButtonLaunchQuicken.Text = "Launch Quicken";
            this.radButtonLaunchQuicken.Click += new System.EventHandler(this.radButtonLaunchQuicken_Click);
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(343, 110);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(78, 18);
            this.radLabel5.TabIndex = 8;
            this.radLabel5.Text = "Last run date : ";
            // 
            // radLabelManualInputDetails
            // 
            this.radLabelManualInputDetails.ForeColor = System.Drawing.Color.DarkGreen;
            this.radLabelManualInputDetails.Location = new System.Drawing.Point(93, 92);
            this.radLabelManualInputDetails.Name = "radLabelManualInputDetails";
            this.radLabelManualInputDetails.Size = new System.Drawing.Size(235, 18);
            this.radLabelManualInputDetails.TabIndex = 7;
            this.radLabelManualInputDetails.Text = "N/A";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(5, 92);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(81, 18);
            this.radLabel3.TabIndex = 6;
            this.radLabel3.Text = "Manual input : ";
            // 
            // radButtonManualInput
            // 
            this.radButtonManualInput.Location = new System.Drawing.Point(164, 136);
            this.radButtonManualInput.Name = "radButtonManualInput";
            this.radButtonManualInput.Size = new System.Drawing.Size(110, 24);
            this.radButtonManualInput.TabIndex = 5;
            this.radButtonManualInput.Text = "Manual Input";
            this.radButtonManualInput.Click += new System.EventHandler(this.radButtonManualInput_Click);
            // 
            // radDropDownListAccountNo
            // 
            this.radDropDownListAccountNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.radDropDownListAccountNo.Location = new System.Drawing.Point(93, 54);
            this.radDropDownListAccountNo.Name = "radDropDownListAccountNo";
            this.radDropDownListAccountNo.Size = new System.Drawing.Size(235, 21);
            this.radDropDownListAccountNo.TabIndex = 4;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(5, 54);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(53, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "Account :";
            // 
            // radButtonConvert
            // 
            this.radButtonConvert.Location = new System.Drawing.Point(325, 136);
            this.radButtonConvert.Name = "radButtonConvert";
            this.radButtonConvert.Size = new System.Drawing.Size(226, 24);
            this.radButtonConvert.TabIndex = 2;
            this.radButtonConvert.Text = "Convert to QFX && Import in Quicken";
            this.radButtonConvert.Click += new System.EventHandler(this.radButtonConvert_Click);
            // 
            // radLabelPreviousRunDate
            // 
            this.radLabelPreviousRunDate.ForeColor = System.Drawing.Color.DarkGreen;
            this.radLabelPreviousRunDate.Location = new System.Drawing.Point(445, 92);
            this.radLabelPreviousRunDate.Name = "radLabelPreviousRunDate";
            this.radLabelPreviousRunDate.Size = new System.Drawing.Size(103, 18);
            this.radLabelPreviousRunDate.TabIndex = 11;
            this.radLabelPreviousRunDate.Text = "N/A";
            this.radLabelPreviousRunDate.Click += new System.EventHandler(this.radLabelPreviousRunDate_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(322, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Previous run date : ";
            // 
            // RadFormMain
            // 
            this.AcceptButton = this.radButtonManualInput;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 201);
            this.Controls.Add(this.radGroupBoxDrop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "RadFormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CSV2QFX";
            this.Load += new System.EventHandler(this.RadFormMain_Load);
            this.radGroupBoxDrop.ResumeLayout(false);
            this.radGroupBoxDrop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label radLabel1;
        private TextBox radTextBoxCSVPath;
        private GroupBox radGroupBoxDrop;
        private Button radButtonConvert;
        private Label radLabel2;
        private ComboBox radDropDownListAccountNo;
        private Button radButtonManualInput;
        private Label radLabel3;
        private Label radLabelManualInputDetails;
        private Button radButtonLaunchQuicken;
        private Label radLabelLastRunDate;
        private Label radLabel5;
        private Label radLabelPreviousRunDate;
        private Label label2;
    }
}

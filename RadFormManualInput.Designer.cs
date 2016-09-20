
using System.Windows.Forms;

namespace CSV2QFX
{
    partial class RadFormManualInput
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
            this.radGroupBox1 = new System.Windows.Forms.GroupBox();
            this.radTextBoxManualInput = new System.Windows.Forms.TextBox();
            this.radButtonOK = new System.Windows.Forms.Button();
            this.radGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.radTextBoxManualInput);
            this.radGroupBox1.Location = new System.Drawing.Point(13, 13);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(489, 288);
            this.radGroupBox1.TabIndex = 0;
            this.radGroupBox1.TabStop = false;
            this.radGroupBox1.Text = "Manual Input";
            // 
            // radTextBoxManualInput
            // 
            this.radTextBoxManualInput.AcceptsReturn = true;
            this.radTextBoxManualInput.Location = new System.Drawing.Point(6, 22);
            this.radTextBoxManualInput.Multiline = true;
            this.radTextBoxManualInput.Name = "radTextBoxManualInput";
            this.radTextBoxManualInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.radTextBoxManualInput.Size = new System.Drawing.Size(478, 261);
            this.radTextBoxManualInput.TabIndex = 0;
            this.radTextBoxManualInput.TabStop = false;
            this.radTextBoxManualInput.TextChanged += new System.EventHandler(this.radTextBoxManualInput_TextChanged);
            // 
            // radButtonOK
            // 
            this.radButtonOK.Location = new System.Drawing.Point(387, 307);
            this.radButtonOK.Name = "radButtonOK";
            this.radButtonOK.Size = new System.Drawing.Size(110, 24);
            this.radButtonOK.TabIndex = 1;
            this.radButtonOK.Text = "OK";
            this.radButtonOK.Click += new System.EventHandler(this.radButtonOK_Click);
            // 
            // RadFormManualInput
            // 
            this.AcceptButton = this.radButtonOK;
            this.ClientSize = new System.Drawing.Size(511, 337);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.radButtonOK);
            this.Name = "RadFormManualInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manual Input";
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox radGroupBox1;
        private TextBox radTextBoxManualInput;
        private Button radButtonOK;
    }
}

using System.Globalization;
using CSV2QFX.Stub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSV2QFX
{
    public partial class RadFormManualInput : Form
    {
        public RadFormManualInput()
        {
            InitializeComponent();

            //radTextBoxManualInput.Text = UtilityQfx.GetInstance.ManualInputContent;
        }

        private void radButtonOK_Click(object sender, EventArgs e)
        {
            UtilityQfx.GetInstance.ManualInputContent = radTextBoxManualInput.Text;
            
            var manualInputDetails = String.Format("Number of lines to convert : {0}",
                UtilityQfx.GetInstance.ManualInputContent.Split(
                new[] { UtilityQfx.CsvNewLine }, StringSplitOptions.RemoveEmptyEntries).Length);

            var ownerForm = Owner as RadFormMain;
            if (ownerForm != null)
            {
                ownerForm.UpdateManualInputDetails(manualInputDetails);
            }

            this.Close();
        }

        private void radTextBoxManualInput_TextChanged(object sender, EventArgs e)
        {
            // replace , in numbers first
            radTextBoxManualInput.Text = radTextBoxManualInput.Text.Replace(
                UtilityQfx.DigitSeparator.ToString(CultureInfo.InvariantCulture), String.Empty);
            
            // replace all tabs by ,
            radTextBoxManualInput.Text = radTextBoxManualInput.Text.Replace("\t", 
                UtilityQfx.ManualCsvSeparator.ToString(CultureInfo.InvariantCulture));
            
            // set cursor position at the end
            radTextBoxManualInput.SelectionStart = radTextBoxManualInput.Text.Length;
            radTextBoxManualInput.SelectionLength = 0;
        }

        private void RadFormManualInput_Load(object sender, EventArgs e)
        {
            radTextBoxManualInput.MaxLength = int.MaxValue;
        }
    }
}

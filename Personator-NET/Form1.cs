using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Personator_NET
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            String RESTRequest = "";
            String Actions = "";
            String Columns = "";
            String Options = "";

            // *************************************************************************************
            // Set the License String in the Request
            // *************************************************************************************
            RESTRequest += @"&id=" + Uri.EscapeDataString(txtLicense.Text);


            // *************************************************************************************
            // Set the Actions in the Request
            // *************************************************************************************
            if (chkActionCheck.Checked)
                Actions += "Check,";
            if (chkActionVerify.Checked)
                Actions += "Verify,";
            if (chkActionAppend.Checked)
                Actions += "Append,";
            if (chkActionMove.Checked)
                Actions += "Move,";

            RESTRequest += @"&act=" + Actions;


            // *************************************************************************************
            // Set the Columns in the Request
            // *************************************************************************************
            foreach (object itemChecked in chkListBoxColumns.CheckedItems)
            {
                Columns += itemChecked.ToString() + ",";
            }

            foreach (object itemChecked in chkListBoxGroups.CheckedItems)
            {
                Columns += itemChecked.ToString() + ",";
            }
            
            RESTRequest += @"&cols=" + Columns;


            // *************************************************************************************
            // Set the Options in the Request
            // *************************************************************************************
            if (chkAdvancedAddressCorrection.Checked)
                Options += "AdvancedAddressCorrection:on;";

            switch (cmbCentricHint.SelectedIndex)
            {
                case 0: Options += "CentricHint:Auto;"; break;
                case 1: Options += "CentricHint:Address;"; break;
                case 2: Options += "CentricHint:Phone;"; break;
                case 3: Options += "CentricHint:Email;"; break;
                case 4: Options += "CentricHint:SSN;"; break;
                default:
                    break;
            }

            switch (cmbAppend.SelectedIndex)
            {
                case 0: Options += "Append:Blank;"; break;
                case 1: Options += "Append:CheckError;"; break;
                case 2: Options += "Append:Always;"; break;
                default:
                    break;
            }

            // Set Options
            RESTRequest += @"&opt=" + Options;



            // *************************************************************************************
            // Set the Input Parameters
            // *************************************************************************************
            RESTRequest += @"&full=" + Uri.EscapeDataString(txtNameIn.Text);
            RESTRequest += @"&comp=" + Uri.EscapeDataString(txtCompanyIn.Text);
            RESTRequest += @"&a1=" + Uri.EscapeDataString(txtAddress1In.Text);
            RESTRequest += @"&a2=" + Uri.EscapeDataString(txtAddress2In.Text);
            
            RESTRequest += @"&city=" + Uri.EscapeDataString(txtCityIn.Text);
            RESTRequest += @"&state=" + Uri.EscapeDataString(txtStateIn.Text);
            RESTRequest += @"&postal=" + Uri.EscapeDataString(txtPostalCodeIn.Text);
            
            RESTRequest += @"&email=" + Uri.EscapeDataString(txtEmailIn.Text);
            RESTRequest += @"&phone=" + Uri.EscapeDataString(txtPhoneIn.Text);

            RESTRequest += @"&phone=" + Uri.EscapeDataString(txtPhoneIn.Text);

            /*
             * Other Possible Personator Inputs
             * 
             *                         
            RESTRequest += @"&ss=" + Uri.EscapeDataString(txtSSNIn.Text);
            RESTRequest += @"&mak=" + Uri.EscapeDataString(txtMAKIn.Text);
            RESTRequest += @"&ip=" + Uri.EscapeDataString(txtIPIn.Text);

            RESTRequest += @"&bday=" + Uri.EscapeDataString(txtBirthDayIn.Text);
            RESTRequest += @"&bmo=" + Uri.EscapeDataString(txtBirthMonthIn.Text);
            RESTRequest += @"&byr=" + Uri.EscapeDataString(txtBirthYearIn.Text);
            */

            // Set JSON Response Protocol
            RESTRequest += @"&format=json";

            // Build the final REST String Query
            RESTRequest = @"https://personator.melissadata.net/v3/WEB/ContactVerify" + @"/doContactVerify?t=" + RESTRequest;

            // Output the REST Query
            txtRESTRequest.Text = RESTRequest;


            // *************************************************************************************
            // Submit to the Web Service. 
            // Make sure to set a retry block in case of any timeouts
            // *************************************************************************************
            Boolean Success = false;
            Int16 RetryCounter = 0;
            Stream ResponseReaderFile = null;
            do
            {
                try
                {
                    HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(RESTRequest);
                    WebResponse Response = WebRequestObject.GetResponse();
                    ResponseReaderFile = Response.GetResponseStream();
                    Success = true;
                }
                catch (Exception ex)
                {
                    RetryCounter++;
                    MessageBox.Show("Exception: " + ex.Message);
                    return;
                }
            } while ((Success != true) && (RetryCounter < 5));


            // *************************************************************************************
            // Output Formatted JSON String
            // *************************************************************************************
            StreamReader Reader = new StreamReader(ResponseReaderFile, Encoding.UTF8);
            String ResponseString = Reader.ReadToEnd();

            txtResponse.Text = JValue.Parse(ResponseString).ToString(Newtonsoft.Json.Formatting.Indented);


        }


        // *************************************************************************************
        // Clear the Input Strings
        // *************************************************************************************
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAddress1In.Text = "";
            txtAddress2In.Text = "";
            txtCityIn.Text = "";
            txtCompanyIn.Text = "";
            txtEmailIn.Text = "";
            txtNameIn.Text = "";
            txtPhoneIn.Text = "";
            txtPostalCodeIn.Text = "";
            txtStateIn.Text = "";
        }


        // *************************************************************************************
        // Check all Group Boxes
        // *************************************************************************************
        private void chkBoxAllGroups_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListBoxGroups.Items.Count; i++)
            {
                chkListBoxGroups.SetItemChecked(i, chkBoxAllGroups.Checked);
            }            
        }


        // *************************************************************************************
        // Wiki Link
        // *************************************************************************************
        private void lnkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://wiki.melissadata.com/index.php?title=Personator");
        }


        // *************************************************************************************
        // Check all Column Boxes
        // *************************************************************************************
        private void chkBoxColumns_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListBoxColumns.Items.Count; i++)
            {
                chkListBoxColumns.SetItemChecked(i, chkBoxColumns.Checked);
            }
        }
    }
}

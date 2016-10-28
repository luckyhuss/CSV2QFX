using System.Configuration;
using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSV2QFX.Stub;

//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSV2QFX
{
    public partial class RadFormMain : Form
    {
        private bool _isManual;
        private readonly string _pathQfx = ConfigurationManager.AppSettings["PathQfx"];
        private readonly string _pathFuellyLog = ConfigurationManager.AppSettings["PathFuellyLog"];
        private const string _fuelly = "FUELLY";        
        private Dictionary<string, string> _settings = new Dictionary<string, string>();
        
        public RadFormMain()
        {
            InitializeComponent();
        }

        private void radButtonConvert_Click(object sender, EventArgs e)
        {
            // write header information
            var sbQfx = new StringBuilder();
            const string dateFormatQfx = "yyyyMMdd";
            var dateToday = DateTime.Today.ToString(dateFormatQfx);

            if (radDropDownListAccountNo.SelectedIndex == -1) return;
            if (!_isManual && String.IsNullOrWhiteSpace(radTextBoxCSVPath.Text)) return;
                        
            var accountDetail = radDropDownListAccountNo.SelectedItem as AccountItem;
            // Account no.
            if (accountDetail == null) return;
            
            var bankId = accountDetail.BankId;
            var acctNo = accountDetail.AcctNo;
            var acctType = accountDetail.AcctType;

            // if fuelly CSV generating
            var isFuelly = bankId.Equals(_fuelly, StringComparison.InvariantCultureIgnoreCase);

            #region QFX Header
            sbQfx.AppendLine("OFXHEADER:100");
            sbQfx.AppendLine("DATA:OFXSGML");
            sbQfx.AppendLine("VERSION:102");
            sbQfx.AppendLine("SECURITY:NONE");
            sbQfx.AppendLine("ENCODING:USASCII");
            sbQfx.AppendLine("CHARSET:1252");
            sbQfx.AppendLine("COMPRESSION:NONE");
            sbQfx.AppendLine("OLDFILEUID:NONE");
            sbQfx.AppendLine("NEWFILEUID:NONE");
            sbQfx.AppendLine();

            // write OFX body
            sbQfx.AppendLine("<OFX>");

            // SIGNONMSGSRSV1
            sbQfx.AppendLine("	<SIGNONMSGSRSV1>");
            sbQfx.AppendLine("		<SONRS>");
            sbQfx.AppendLine("			<STATUS>");
            sbQfx.AppendLine("				<CODE>0");
            sbQfx.AppendLine("				<SEVERITY>INFO");
            sbQfx.AppendLine("				<MESSAGE>OK");
            sbQfx.AppendLine("			</STATUS>");
            sbQfx.AppendLine(
                String.Format("			<DTSERVER>{0}", dateToday));
            sbQfx.AppendLine("			<LANGUAGE>ENG");
            sbQfx.AppendLine("			<INTU.BID>1270");
            sbQfx.AppendLine("		</SONRS>");
            sbQfx.AppendLine("	</SIGNONMSGSRSV1>");

            // BANKMSGSRSV1
            sbQfx.AppendLine("	<BANKMSGSRSV1>");
            sbQfx.AppendLine("		<STMTTRNRS>");
            sbQfx.AppendLine(
                String.Format("			<TRNUID>{0}", dateToday));
            sbQfx.AppendLine("			<STATUS>");
            sbQfx.AppendLine("				<CODE>0");
            sbQfx.AppendLine("				<SEVERITY>INFO");
            sbQfx.AppendLine("				<MESSAGE>OK");
            sbQfx.AppendLine("			</STATUS>");

            // STMTRS
            sbQfx.AppendLine("			<STMTRS>");
            sbQfx.AppendLine("				<CURDEF>USD");

            // BANKACCTFROM
            sbQfx.AppendLine("				<BANKACCTFROM>");
            sbQfx.AppendLine(
                String.Format("					<BANKID>{0}", bankId));
            sbQfx.AppendLine(
                String.Format("					<ACCTID>{0}{1}{0}", bankId, acctNo));
            sbQfx.AppendLine(
                String.Format("					<ACCTTYPE>{0}", acctType));
            sbQfx.AppendLine("				</BANKACCTFROM>");

            // BANKTRANLIST
            sbQfx.AppendLine("				<BANKTRANLIST>");
            sbQfx.AppendLine(
                String.Format("					<DTSTART>{0}", dateToday));
            sbQfx.AppendLine(
                String.Format("					<DTEND>{0}", dateToday));
            #endregion

            // TRANSACTIONS
            // READ CSV
            var csvImport = _isManual ?
                UtilityQfx.GetInstance.ManualInputContent :
                File.ReadAllText(radTextBoxCSVPath.Text, Encoding.Default);

            // replace all digit separator in CSV
            Regex amount = new Regex(@"""-*\d+,\d+[.]\d{2} *""", RegexOptions.Multiline);

            foreach(Match match in amount.Matches(csvImport))
            {
                // replace , by empty and <space> by empty
                csvImport = csvImport.Replace(match.Value, 
                    match.Value.Replace(UtilityQfx.DigitSeparator.ToString(), string.Empty).Replace(' '.ToString(), string.Empty));
            }

            // replace all " by empty
            csvImport = csvImport.Replace('"'.ToString(), string.Empty);

            var csvLines = csvImport.Split(
                new[] { UtilityQfx.CsvNewLine }, StringSplitOptions.RemoveEmptyEntries);

            // reset StringBuilder for FuellyCSV
            if (isFuelly)
            {
                sbQfx = new StringBuilder();

                // CSV header
                sbQfx.AppendLine("odometer,litres,price,fuelup_date,city_percentage,tags,notes,missed_fuelup,partial_fuelup");
            }

            foreach (var csvLine in csvLines)
            {
                // if (String.IsNullOrWhiteSpace(csvLine)) continue; // no more required

                var csvValues = csvLine.Replace("\r", string.Empty).Split(_isManual ? UtilityQfx.ManualCsvSeparator : UtilityQfx.CsvSeparator);
                if (csvValues.Length <= 2 && !isFuelly) continue;

                // shared variables within the scope of "switch"
                string trnType;
                string trnAmt;
                DateTime dtposted;

                switch (bankId)
                {
                    #region SBM
                    case "SBM":

                        #region CREDITLINE
                        if (acctType.Equals("CREDITLINE"))
                        {
                            // Transaction Number,Transaction Date,Posted Date,,Amount in Billing Currency,Original Currency,Original Amount,Nature Of Transaction,Transaction Remarks
                            // 100782751480,28-09-2016,28-09-2016,,-20,000.00",MUR,-20,000.00, Payments to account,Payment
                            // 283730284   ,26-09-2016,28-09-2016,,518.77  ,MUR,518.77  ,205,SHOPRITE LTD QUATRE BORNES ZAF
                            // FITID format : yyyyMMdd_<Ref no.>_<1st word of desc>_<TRNAMT>_<TRNTYPE>

                            trnType = csvValues[4].Contains("-") ? "CREDIT" : "DEBIT";
                            trnAmt = csvValues[4].Contains("-")
                                         ? csvValues[4].Replace("-", string.Empty)
                                         : String.Concat("-", csvValues[4]);

                            // check if not header
                            if (!DateTime.TryParse(csvValues[1], out dtposted)) continue;

                            // valid line
                            // STMTTRN
                            sbQfx.AppendLine("					<STMTTRN>");
                            sbQfx.AppendLine(
                                String.Format("						<TRNTYPE>{0}", trnType));
                            sbQfx.AppendLine(
                                String.Format("						<DTPOSTED>{0}", dtposted.ToString(dateFormatQfx)));
                            sbQfx.AppendLine(
                                String.Format("						<TRNAMT>{0}", trnAmt.Trim()));
                            sbQfx.AppendLine(
                                String.Format("						<FITID>{0}_{1}_{2}_{3}_{4}",
                                              dtposted.ToString(dateFormatQfx),
                                              String.IsNullOrWhiteSpace(csvValues[0]) ? "NIL" : csvValues[0].Trim(),
                                              csvValues[8].Replace(" ", String.Empty).ToUpper().ToUpper(),
                                              trnAmt.Trim(),
                                              trnType));
                            sbQfx.AppendLine(
                                String.Format("						<MEMO>{0}", RenameAccountNameMemo(csvValues[3])));
                            sbQfx.AppendLine("					</STMTTRN>");
                        }
                        #endregion

                        #region CASA ACCOUNTS
                        else
                        {
                            //Instrument ID, Transaction Date,Value Date, Source Branch Code,, Transaction Remarks,Debit Amount, Credit Amount,Running Balance                            
                            //,04-10-2016,04-10-2016,PORT LOUIS,, NANDO'S NON SBM POS,705.00,,5645.35
                            // FITID format : yyyyMMdd_<Source branch.>_<1st word of desc>_<TRNAMT>_<TRNTYPE>

                            trnType = String.IsNullOrWhiteSpace(csvValues[6]) ? "CREDIT" : "DEBIT";
                            trnAmt = String.IsNullOrWhiteSpace(csvValues[6])
                                         ? csvValues[7]
                                         : String.Concat("-", csvValues[6]);

                            if (trnType.Equals("DEBIT") && trnAmt.Contains("--"))
                            {
                                // return from bank to the account ( POS failed etc.)
                                // therefore treat as a credit to the account
                                trnType = "CREDIT";
                                trnAmt = trnAmt.Replace("--", String.Empty);
                            }

                            // check if not header
                            if (!DateTime.TryParse(csvValues[1], out dtposted)) continue;

                            // valid line
                            // STMTTRN
                            sbQfx.AppendLine("					<STMTTRN>");
                            sbQfx.AppendLine(
                                String.Format("						<TRNTYPE>{0}", trnType));
                            sbQfx.AppendLine(
                                String.Format("						<DTPOSTED>{0}", dtposted.ToString(dateFormatQfx)));
                            sbQfx.AppendLine(
                                String.Format("						<TRNAMT>{0}", trnAmt.Trim()));
                            sbQfx.AppendLine(
                                String.Format("						<FITID>{0}_{1}_{2}_{3}_{4}",
                                              dtposted.ToString(dateFormatQfx),
                                              String.IsNullOrWhiteSpace(csvValues[3]) ? "NIL" : csvValues[3].Replace(" ", String.Empty).ToUpper(),
                                              csvValues[5].Replace(" ", String.Empty).ToUpper(),
                                              trnAmt.Trim(),
                                              trnType));
                            sbQfx.AppendLine(
                                String.Format("						<MEMO>{0}", RenameAccountNameMemo(csvValues[5])));
                            sbQfx.AppendLine("					</STMTTRN>");
                        }
                        #endregion
                        break;
                    #endregion
                    
                    #region MCB
                    case "MCB":
                        // Transaction Date,Value Date,Reference,Description,Money out,Money in,Balance
                        // 24-Jun-2016,24-Jun-2016,FT161767FHSQ\BNK,Debit Card Purchase WINNERS ST PAUL,956.81,0.00,40764.39
                        // FITID format : yyyyMMdd_<Ref>_<1st word of desc>_<TRNAMT>_<TRNTYPE>

                        trnType = "0.00".Equals(csvValues[4]) ? "CREDIT" : "DEBIT";
                        trnAmt = "0.00".Equals(csvValues[4]) ? csvValues[5] : String.Concat("-", csvValues[4]);

                        // check if not header
                        if (!DateTime.TryParse(csvValues[0], out dtposted)) continue;

                        // valid line
                        // STMTTRN
                        sbQfx.AppendLine("					<STMTTRN>");
                        sbQfx.AppendLine(
                            String.Format("						<TRNTYPE>{0}", trnType));
                        sbQfx.AppendLine(
                            String.Format("						<DTPOSTED>{0}", dtposted.ToString(dateFormatQfx)));
                        sbQfx.AppendLine(
                            String.Format("						<TRNAMT>{0}", trnAmt.Trim()));
                        sbQfx.AppendLine(
                            String.Format("						<FITID>{0}_{1}_{2}_{3}_{4}",
                                dtposted.ToString(dateFormatQfx),
                                String.IsNullOrWhiteSpace(csvValues[2]) ? "NIL" : csvValues[2].Replace(" ", String.Empty).ToUpper(),
                                csvValues[3].Replace(" ", String.Empty).ToUpper(),
                                trnAmt.Trim(),
                                trnType));
                        sbQfx.AppendLine(
                            String.Format("						<MEMO>{0}", RenameAccountNameMemo(csvValues[3])));
                        sbQfx.AppendLine("					</STMTTRN>");
                        break;
                    #endregion
                    
                    #region CIM
                    case "CIM":

                        //Post Date;Trans Date;Merchant ID;Transaction Type;Transaction Desc;Debit;Credit
                        //06-Jun-2010;05-Jun-2010;SH013;SHELL PINEVIEW SERVICE STATION;110;SALES;704.06;.00
                        //14-Jun-2010;13-Jun-2010;SH013;SHELL PINEVIEW SERVICE STATION;110;SALES;1017.85;.00
                        //20-Jun-2010;19-Jun-2010;SH013;SHELL PINEVIEW SERVICE STATION;110;SALES;528.85;.00
                        //27-Jun-2010;26-Jun-2010;SH013;SHELL PINEVIEW SERVICE STATION;110;SALES;1037.31;.00
                        //30-Jun-2010;30-Jun-2010; ; ;500;P_no: 14618345 Amt: 3162.77; ;3162.77
                        //25-Jul-2010;24-Jul-2010;SH013;SHELL PINEVIEW SERVICE STATION;110;SALES;1050.19;.00
                        // FITID format : yyyyMMdd_<Merchant_ID>_<1st word of Type>_<TRNAMT>_<TRNTYPE>

                        trnType = String.IsNullOrWhiteSpace(csvValues[6]) || csvValues[6].Equals(".00") ? "CREDIT" : "DEBIT";
                        trnAmt = String.IsNullOrWhiteSpace(csvValues[6]) || csvValues[6].Equals(".00")
                                     ? csvValues[7]
                                     : String.Concat("-", csvValues[6]);

                        // check if not header
                        if (!DateTime.TryParse(csvValues[1], out dtposted)) continue;

                        // valid line
                        // STMTTRN
                        sbQfx.AppendLine("					<STMTTRN>");
                        sbQfx.AppendLine(
                            String.Format("						<TRNTYPE>{0}", trnType));
                        sbQfx.AppendLine(
                            String.Format("						<DTPOSTED>{0}", dtposted.ToString(dateFormatQfx)));
                        sbQfx.AppendLine(
                            String.Format("						<TRNAMT>{0}", trnAmt));
                        sbQfx.AppendLine(
                            String.Format("						<FITID>{0}_{1}_{2}_{3}_{4}",
                                        dtposted.ToString(dateFormatQfx),
                                        String.IsNullOrWhiteSpace(csvValues[2]) ? "NIL" : csvValues[2],
                                        csvValues[3].Split(' ')[0].ToUpper(),
                                        trnAmt,
                                        trnType));
                        sbQfx.AppendLine(
                            String.Format("						<MEMO>{0}",
                                RenameAccountNameMemo(
                                    String.IsNullOrWhiteSpace(csvValues[3]) ? "PAYMENT" : csvValues[3])));
                        sbQfx.AppendLine("					</STMTTRN>");
                        break;
                    #endregion
                    
                    #region FUELLY
                    case _fuelly:

                        // output format
                        //odometer,litres,price,fuelup_date,city_percentage,tags,notes,missed_fuelup,partial_fuelup
                        //83849,32.37,9.30,2013-01-16,80,"","SHELL EBENE",0,0
                        //84216,28.13,9.30,2013-01-26 14:47:41,80,"","SHELL PINEVIEW",0,0
                        //84668,33.06,9.30,2013-02-09 14:03:55,80,"","SHELL EAU COULEE",0,0

                        string shellPDFSeparator = " ";

                        // input format
                        //08/12/2013 17:56:43 SHELL PINEVIEW UX 26.97 1,409.29 94959 13/59834 09/12/2013
                        //or
                        //23/03/2013 6:56:49 PM SHELL PINEVIEW UX 29.92 1,563.16 86280 33/79193 25/03/2013

                        // Foxit Reader fix @ 30/06/2014 (time is at the end instead 2nd)
                        //17/05/2014 SHELL ST JEAN UX 1,650.00 73/49852  101764 31.58 19/05/2014 19:11:07
                        // UPDATE @ 21/04/2015
                        // Foxit Reader 7.0.6 is normal (time is at 2nd position)
                        //16/01/2015 5:44:42 PM SHELL ST PAUL UX 2,875.0062.57 111 98/12249 19/01/2015

                        // replace all " " by ,
                        var lineValue = csvLine.Replace(shellPDFSeparator,
                            UtilityQfx.ManualCsvSeparator.ToString(CultureInfo.InvariantCulture));

                        var lineValues = lineValue.Replace("\r", String.Empty).Split(UtilityQfx.ManualCsvSeparator);
                        if (lineValues.Length <= 1) continue;

                        var indexOfUX = Array.FindIndex(lineValues, value => value.Contains("UX"));

                        // fuelup_date
                        var datetime = lineValues[0];

                        bool foxit = false;
                        if (lineValues[1].Contains(":"))
                        {
                            // PDF text copied from Adobe Acrobat Reader
                            datetime = String.Format("{0} {1}", lineValues[0], lineValues[1]);
                        }
                        else if (lineValues[lineValues.Length - 1].Contains(":"))
                        {
                            // PDF text copied from Foxit Reader
                            datetime = String.Format("{0} {1}", lineValues[0], lineValues[lineValues.Length - 1]);
                            foxit = true;
                        }

                        bool ampm = false;
                        var fuelup_date = "error";

                        if (lineValues[2].Contains("AM") || lineValues[2].Contains("PM"))
                        {
                            ampm = true;
                            datetime = String.Format("{0} {1}", datetime, lineValues[2]);

                            // 23/03/2013 6:56:49 PM -> 2013-03-27 06:56:49
                            fuelup_date =
                                DateTime.ParseExact(datetime, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).
                                ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            // 08/12/2013 17:56:43 -> 2013-12-08 17:56:43
                            fuelup_date =
                                DateTime.ParseExact(datetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).
                                ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        // notes
                        string notes = "";
                        int counter = 2;
                        if (foxit)
                        {
                            // PDF text copied from Foxit Reader
                            counter = 1;
                        }

                        if (ampm) counter = 3;
                        for (int i = counter; i < indexOfUX; i++)
                        {
                            notes += String.Concat(lineValues[i], shellPDFSeparator);
                        }
                        notes = notes.Trim();

                        string litres = String.Empty;
                        decimal totalPrice = 0;
                        decimal pricePerLitre;
                        string odometer;

                        if (!foxit)
                        {
                            // PDF text copied from Adobe Acrobat Reader

                            // if new FoxIt
                            int countDot = lineValues[indexOfUX + 1].Count(dot => dot == '.');
                            if (countDot > 1)
                            {
                                // then format other indexed value = <price><litres>
                                //2,418.8152.64
                                Regex floatValues = new Regex(@"[0-9]*\.?[0-9]{2}", RegexOptions.IgnoreCase);
                                var matches = floatValues.Matches(lineValues[indexOfUX + 1]);

                                if (matches.Count > 1)
                                {
                                    // litres
                                    litres = matches[1].Value;

                                    // total price
                                    totalPrice = Convert.ToDecimal(matches[0].Value);

                                    indexOfUX--;
                                }
                            }
                            else
                            {
                                // litres
                                litres = lineValues[indexOfUX + 1];

                                // total price
                                totalPrice = Convert.ToDecimal(lineValues[indexOfUX + 2]);
                            }

                            // price per litre
                            pricePerLitre = totalPrice / Convert.ToDecimal(litres);

                            // 30/06/2014 - Fuelly has fixed the currency bug I reported in May 2014
                            //var denominator = Convert.ToInt32(Math.Floor(price / 10)) * 10;
                            //price = price - denominator;

                            // odometer
                            odometer = lineValues[indexOfUX + 3];
                        }
                        else
                        {
                            // PDF text copied from Foxit Reader
                            // litres
                            litres = lineValues[indexOfUX + 5];

                            // price
                            pricePerLitre = Convert.ToDecimal(lineValues[indexOfUX + 1]) / Convert.ToDecimal(litres);

                            // 30/06/2014 - Fuelly has fixed the currency bug I reported in May 2014
                            //var denominator = Convert.ToInt32(Math.Floor(price / 10)) * 10;
                            //price = price - denominator;

                            // odometer
                            odometer = lineValues[indexOfUX + 4];
                        }

                        // city_percentage                        
                        var city_percentage = 75;

                        var csvLineToOutput = String.Format("{0},{1},{2:f2},{3},{4},{5},\"{6}\",{7},{8}",
                            odometer,
                            litres,
                            pricePerLitre,
                            fuelup_date,
                            city_percentage,
                            "",
                            notes,
                            0,
                            0);

                        sbQfx.AppendLine(csvLineToOutput);

                        break;
                        #endregion                    
                } // end switch bankId
            } // end foreach

            if (!isFuelly)
            {
                // write footer information
                sbQfx.AppendLine("				</BANKTRANLIST>");
                sbQfx.AppendLine("				<LEDGERBAL>");
                sbQfx.AppendLine("					<BALAMT>0.00");
                sbQfx.AppendLine(
                    String.Format("					<DTASOF>{0}", dateToday));
                sbQfx.AppendLine("				</LEDGERBAL>");
                sbQfx.AppendLine("				<AVAILBAL>");
                sbQfx.AppendLine("					<BALAMT>0.00");
                sbQfx.AppendLine(
                    String.Format("					<DTASOF>{0}", dateToday));
                sbQfx.AppendLine("				</AVAILBAL>");
                sbQfx.AppendLine("			</STMTRS>");
                sbQfx.AppendLine("		</STMTTRNRS>");
                sbQfx.AppendLine("	</BANKMSGSRSV1>");
                sbQfx.AppendLine("</OFX>");
            }

            if (String.IsNullOrWhiteSpace(radTextBoxCSVPath.Text) && !_isManual) return;
            
            var filenameQfx = Path.Combine(_pathQfx,
            String.Format("{0}_{1}.qfx", acctNo, dateToday));

            File.Delete(filenameQfx);

            using (var fs = File.OpenWrite(filenameQfx))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write(sbQfx.ToString());
                }
            }

            // import into Quicken
            Process.Start(filenameQfx);

            _isManual = false;

            MessageBox.Show(@"Import into Quicken has been launched.");

            #region Update settings.ini
            // update last run date
            var today = DateTime.Today.ToString("dd/MM/yyyy");
            if (!_settings["lastrundate"].Equals(today))
            {
                radLabelPreviousRunDate.Text = radLabelLastRunDate.Text;
                _settings["previousrundate"] = radLabelPreviousRunDate.Text;

                radLabelLastRunDate.Text = today;
                _settings["lastrundate"] = radLabelLastRunDate.Text;
            }

            // update settings.ini file
            WriteSettings();
            #endregion
        }

        private void WriteSettings()
        {
            StringBuilder sbData = new StringBuilder();
            foreach (var keyvalue in _settings)
            {
                sbData.AppendLine(String.Format("{0}={1}", keyvalue.Key, keyvalue.Value));
            }

            File.WriteAllText(ConfigurationManager.AppSettings["SettingsFile"], sbData.ToString(), Encoding.Default);
        }

        private static string RenameAccountNameMemo(string originalMemo)
        {
            return originalMemo;
            //return originalMemo.
            //    Replace("00110100207295", "ACC7295").
            //    Replace("03136200005044", "ACC5044").
            //    Replace("06336200010590", "ACC0590");
        }

        private void radGroupBoxDrop_DragEnter(object sender, DragEventArgs e)
        {
            var buffer = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            if (buffer != null)
            {
                radTextBoxCSVPath.Text = buffer.FirstOrDefault();
            }

            if (String.IsNullOrWhiteSpace(radTextBoxCSVPath.Text)) return;

            var fileName = Path.GetFileNameWithoutExtension(radTextBoxCSVPath.Text);
            var ext = Path.GetExtension(radTextBoxCSVPath.Text.Trim());

            if (fileName != null && ext.Equals(".csv", StringComparison.InvariantCultureIgnoreCase))
            {
                // load file content
                var fileContent = File.ReadAllText(radTextBoxCSVPath.Text, Encoding.Default);
                var acctNo = String.Empty;

                // parse content to get account number
                Regex accountNumberMCB = new Regex(@"(Account Number)\s(?<acc>\d+)", RegexOptions.Multiline);
                Regex accountNumberSBM = new Regex(@"(Transaction).*(?<acc>\d{14})", RegexOptions.Multiline);

                if (accountNumberMCB.IsMatch(fileContent))
                {
                    // MCB account found
                    var match = accountNumberMCB.Match(fileContent);
                    acctNo = match.Groups["acc"].Value;
                }
                else if (accountNumberSBM.IsMatch(fileContent))
                {
                    // SBM account found
                    var match = accountNumberSBM.Match(fileContent);
                    acctNo = match.Groups["acc"].Value;
                }

                if (!String.IsNullOrEmpty(acctNo))
                {
                    foreach (var item in radDropDownListAccountNo.Items)
                    {
                        if (item is AccountItem && (((AccountItem)item).AcctNo.Equals(acctNo)))
                        {
                            radDropDownListAccountNo.SelectedItem = item;
                            break;
                        }
                        else if (item is AccountItem && (((AccountItem)item).AcctNo.Equals("CREDITCARD01") && acctNo.Contains("7712")))
                        {
                            //  CREDITLINE
                            radDropDownListAccountNo.SelectedItem = item;
                        }
                    }
                }
                else
                {
                    // clear data
                    radDropDownListAccountNo.SelectedIndex = -1;
                }
            }
            else
            {
                // clear data
                radDropDownListAccountNo.SelectedIndex = -1;
            }
        }

        private void radGroupBoxDrop_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void radButtonManualInput_Click(object sender, EventArgs e)
        {
            // reset drop down list
            radDropDownListAccountNo.SelectedIndex = -1;
            
            var manualInput = new RadFormManualInput();
            manualInput.ShowDialog(this);
        }

        public void UpdateManualInputDetails(string manualInputDetails)
        {
            radLabelManualInputDetails.Text = manualInputDetails;
            _isManual = true;
        }

        private void RadFormMain_Load(object sender, EventArgs e)
        {
            _isManual = false;

            LoadSettings();

            if (_settings.ContainsKey("lastrundate"))
            {
                radLabelLastRunDate.Text = _settings["lastrundate"];
                radLabelPreviousRunDate.Text = _settings["previousrundate"];
            }
        }

        private void LoadSettings()
        {
            _settings.Clear();
            radDropDownListAccountNo.Items.Clear();

            var settingsData = File.ReadAllLines(ConfigurationManager.AppSettings["SettingsFile"], Encoding.Default);
            var accountsData = File.ReadAllLines(ConfigurationManager.AppSettings["AccountsFile"], Encoding.Default);

            foreach (var entry in settingsData)
            {
                var namevalue = entry.Split('=');
                _settings.Add(namevalue[0], namevalue[1]);
            }

            var header = string.Empty;

            foreach (var entry in accountsData)
            {
                // example : SBM~00110100207295~SAVINGS~MUR
                var accountDetails = entry.Split('~');

                var bankId = accountDetails[0].Trim();
                var acctNo = accountDetails[1].Trim();
                var acctType = accountDetails[2].Trim();
                var acctCurr = accountDetails[3].Trim();

                if (!header.Equals(bankId))
                {
                    radDropDownListAccountNo.Items.Add("");
                    radDropDownListAccountNo.Items.Add(String.Format("     === {0} ===", bankId));
                    header = bankId;
                }
                radDropDownListAccountNo.Items.Add(new AccountItem(bankId, acctNo, acctType, acctCurr));
            }
        }

        private void radButtonLaunchQuicken_Click(object sender, EventArgs e)
        {
            Process.Start(ConfigurationManager.AppSettings["QuickenPath"]);
        }

        private void radLabelPreviousRunDate_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(radLabelPreviousRunDate.Text);
        }

        private void radLabelLastRunDate_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(radLabelLastRunDate.Text);
        }
    }
}

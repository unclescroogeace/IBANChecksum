using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBANChecksum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            validationResult.Text = "";
            textBox1.Text = "";
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            String clientIBAN = textBox1.Text;

            if (checkIBAN(clientIBAN))
            {
                validationResult.ForeColor = ColorTranslator.FromHtml("#33cc33");
                validationResult.Text = "VALID";
            }
            else
            {
                
                validationResult.ForeColor = ColorTranslator.FromHtml("#ff0000");
                validationResult.Text = "NOT VALID";
            }
        }

        private bool checkIBAN(string clientIBAN)
        {
            clientIBAN = clientIBAN.Replace(" ", String.Empty);
            if (clientIBAN.Length == 22)
            {
                if (Regex.IsMatch(clientIBAN, @"^[A-Z0-9]+$") && (clientIBAN[0] == 'B' && clientIBAN[1] == 'G'))
                {
                    String iban = clientIBAN.Substring(4, clientIBAN.Length - 4) + clientIBAN.Substring(0, 4);
                    int shiftNum = 55;
                    int value;
                    StringBuilder sb = new StringBuilder();

                    foreach(char c in iban)
                    {
                        if (Char.IsLetter(c))
                        {
                            value = c - shiftNum;
                        }
                        else
                        {
                            value = int.Parse(c.ToString());
                        }
                        sb.Append(value);
                    }

                    string checksumString = sb.ToString();
                    int checksum = int.Parse(checksumString.Substring(0, 1));
                    for (int i = 1; i < checksumString.Length; i++)
                    {
                        value = int.Parse(checksumString.Substring(i, 1));
                        checksum *= 10;
                        checksum += value;
                        checksum %= 97;
                    }

                    return checksum == 1;
                }
                else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }
    }
}

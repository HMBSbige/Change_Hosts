using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Change_Hosts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var hostsAddress = textBox1.Text;
            var add = GetGeneralContent(hostsAddress);
            if (add != "")
            {
                var str2="";
                if (checkBox1.Checked == false)
                {
                    str2 = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts", Encoding.ASCII);
                    str2 = str2 + '\n' + add;
                }
                else
                    str2 = add;
                const string strt1 = "# Last updated: ";
                const string strt2 = "# This work is licensed";

                File.WriteAllText(@"C:\Windows\System32\drivers\etc\hosts", str2, Encoding.ASCII);
                var hostDate= "\nHosts版本：";
                if (hostsAddress == "https://raw.githubusercontent.com/racaljk/hosts/master/hosts")
                    hostDate += str2.Substring(str2.IndexOf(strt1, StringComparison.Ordinal) + strt1.Length,
                        str2.IndexOf(strt2, StringComparison.Ordinal) - (str2.IndexOf(strt1, StringComparison.Ordinal) + strt1.Length + 1));
                else
                    hostDate = "";
                MessageBox.Show(@"修改成功！" + hostDate, @"提示：");
            }
            else
            {
                MessageBox.Show(@"修改失败！
请检查是否有权限访问HOSTS或HOSTS地址是否正确", @"提示：");
            }
        }
        private static string GetGeneralContent(string strUrl)
        {
            var strMsg = string.Empty;
            try
            {
                var request = WebRequest.Create(strUrl);
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                strMsg = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            {
                // ignored
            }
            return strMsg;
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Change_Hosts
{
    public partial class Form1 : Form
    {
        private const string LocalHostsAddress = @"C:\Windows\System32\drivers\etc\hosts";
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var hosts = textBox3.Text;

                System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding(false);
                File.WriteAllText(LocalHostsAddress, hosts, utf8);

                toolStripStatusLabel1.Text=@"修改Hosts成功！";
            }
            catch
            {
                toolStripStatusLabel1.Text = @"修改Hosts失败！请检查是否有权限修改Hosts" + @"(" + LocalHostsAddress+@")";
            }

        }
        private static string GetGeneralContent(string strUrl)
        {
            var strMsg = string.Empty;
            try
            {
                var request = WebRequest.Create(strUrl);
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                strMsg = reader.ReadToEnd();
                strMsg = strMsg.Replace("\n", "\r\n");

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

        private void button2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = @"正在获取...";
            textBox3.Text = "";
            var hostsAddress = textBox1.Text;
            var add = GetGeneralContent(hostsAddress);
            if (add != "")
            {
                toolStripStatusLabel1.Text = @"获取远程Hosts成功！";               
                textBox3.Text = add;

                var str2 = add;
                const string strt1 = "# Last updated: ";
                const string strt2 = "# This work is licensed";

                if (hostsAddress != "https://raw.githubusercontent.com/racaljk/hosts/master/hosts") return;
                var hostDate = str2.Substring(str2.IndexOf(strt1, StringComparison.Ordinal) + strt1.Length,
                    str2.IndexOf(strt2, StringComparison.Ordinal) - (str2.IndexOf(strt1, StringComparison.Ordinal) + strt1.Length)-4);
                toolStripStatusLabel1.Text = @"Hosts版本：" + hostDate;
            }
            else
            {
                toolStripStatusLabel1.Text = @"获取远程Hosts失败！请检查网络连接和HOSTS地址是否正确";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = File.ReadAllText(LocalHostsAddress, Encoding.UTF8);
                toolStripStatusLabel1.Text = @"获取本地Hosts成功！";
            }
            catch
            {
                toolStripStatusLabel1.Text = @"获取本地Hosts失败！请检查是否有权限读取Hosts" + @"(" + LocalHostsAddress + @")";
            }
                       
        }
    }
}

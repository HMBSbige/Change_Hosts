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
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var hosts = textBox3.Text;

                System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding(false);
                File.WriteAllText(LocalHostsAddress, hosts, utf8);

                Get_local_hosts();

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

        private void Button2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = @"正在获取...";
            textBox3.Text = "";
            var hostsAddress = comboBox1.Text;
            var add = GetGeneralContent(hostsAddress);
            if (add != "")
            {
                toolStripStatusLabel1.Text = @"获取远程Hosts成功！";               
                textBox3.Text = add;

                string[] ContentLines = add.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                const string str1 = @"# Last updated: ";
                try
                {
                    var str2 = ContentLines[2].Substring(0, str1.Length);
                    if (str2 == str1)
                        toolStripStatusLabel1.Text = @"Hosts更新于：" + ContentLines[2].Substring(str1.Length);
                }
                catch
                {
                    return;
                }               
            }
            else
            {
                toolStripStatusLabel1.Text = @"获取远程Hosts失败！请检查网络连接和HOSTS地址是否正确";
            }
        }
        void Get_local_hosts()
        {
            try
            {
                textBox2.Text = File.ReadAllText(path: LocalHostsAddress, encoding: Encoding.UTF8);
                toolStripStatusLabel1.Text = @"获取本地Hosts成功！";
            }
            catch
            {
                toolStripStatusLabel1.Text = @"获取本地Hosts失败！请检查是否有权限读取Hosts" + @"(" + LocalHostsAddress + @")";
            }
        }
        private void Button3_Click(object sender, EventArgs e) => Get_local_hosts();

        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip toolTip_button = new ToolTip
            {
                ShowAlways = true
            };
            toolTip_button.ShowAlways = true;
            toolTip_button.SetToolTip(button1, @"更新本机Hosts");
            toolTip_button.SetToolTip(button2, @"从远程获取Hosts");
            toolTip_button.SetToolTip(button3, @"查看本机Hosts");
        }
    }
}

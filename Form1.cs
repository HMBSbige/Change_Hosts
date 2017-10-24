using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Change_Hosts
{
    public partial class Form1 : Form
    {
        private const string LocalHostsAddress = @"C:\Windows\System32\drivers\etc\hosts";
        private string _hostsAddress=@"";
        private bool _isreceiving;

        private delegate void Textcallback(string str);
        private Textcallback _toolStripStatusLabel1TextCallback;
        private Textcallback _textBox3Textcallback;

        private delegate void GetTextcallback();
        private GetTextcallback _comboBox1TextCallBack;

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

        private void Get_Hosts()
        {
            _isreceiving = true;
            _toolStripStatusLabel1TextCallback?.Invoke(@"正在获取...");
            textBox3?.Invoke(_textBox3Textcallback, @"");

            
            comboBox1?.Invoke(_comboBox1TextCallBack);
           
            var add = GetGeneralContent(_hostsAddress);
            if (add != "")
            {
                _toolStripStatusLabel1TextCallback?.Invoke(@"获取远程Hosts成功！");
                textBox3?.Invoke(_textBox3Textcallback, add);

                string[] contentLines = add.Split(new []{ "\r\n" }, StringSplitOptions.None);
                const string str1 = @"# Last updated: ";
                try
                {
                    var str2 = contentLines[2].Substring(0, str1.Length);
                    if (str2 == str1)
                    {
                        _toolStripStatusLabel1TextCallback?.Invoke(@"Hosts更新于：" + contentLines[2].Substring(str1.Length));
                    }

                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                _toolStripStatusLabel1TextCallback?.Invoke(@"获取远程Hosts失败！请检查网络连接和HOSTS地址是否正确");
                textBox3?.Invoke(_textBox3Textcallback, _hostsAddress);
            }
            _isreceiving = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (!_isreceiving)
            {
                Thread t1 = new Thread(Get_Hosts)
                {
                    IsBackground = true
                };
                t1.Start();
            }
            else
            {
                toolStripStatusLabel1.Text = @"正在获取中...";
            }
        }

        private void Get_local_hosts()
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
            ToolTip toolTipButton = new ToolTip
            {
                ShowAlways = true
            };
            toolTipButton.ShowAlways = true;
            toolTipButton.SetToolTip(button1, @"更新本机Hosts");
            toolTipButton.SetToolTip(button2, @"从远程获取Hosts");
            toolTipButton.SetToolTip(button3, @"查看本机Hosts");
            _toolStripStatusLabel1TextCallback = Change_toolStripStatusLabel1;
            _textBox3Textcallback = Change_textbox3Text;
            _comboBox1TextCallBack = Get_comboBox1Text;
        }

        private void Change_toolStripStatusLabel1(string str)
        {
             toolStripStatusLabel1.Text = str;
        }

        private void Change_textbox3Text(string str)
        {
            textBox3.Text = str;
        }

        private void Get_comboBox1Text()
        {
            _hostsAddress = comboBox1.Text;
        }
    }
}
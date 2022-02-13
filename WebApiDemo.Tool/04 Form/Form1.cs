using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

using WebApiDemo.Tool._00_Def;
using WebApiDemo.Tool._03_Service;

namespace WebApiDemo.Tool._04_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AppendLog(string line, bool bIsContainPrefix = true)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            string text = null;
            if (bIsContainPrefix)
            {
                text = string.Format("[{0}] {1}{2}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    line,
                    Environment.NewLine
                );
            }
            else
            {
                text = string.Format("{1}{2}", line, Environment.NewLine);
            }

            textBox1.AppendText(text);
            textBox1.ScrollToCaret();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var frm = new LoginForm())
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    var model = new LoginInfoModel
                    {
                        UserName = frm.UserName,
                        Code = frm.Code,
                        Pwd = frm.Pwd
                    };

                    AppendLog(string.Format("Start Send Login Req", ApiServer.LoginUrl));
                    string json = await ApiServer.LoginAsync(model);
                    AppendLog(string.Format("Recv Login Result: {0}{1}{2}{1}", ApiServer.LoginUrl, Environment.NewLine, json, Environment.NewLine));
                }
            }
        }
    }
}

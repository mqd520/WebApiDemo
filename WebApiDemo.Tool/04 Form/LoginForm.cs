using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebApiDemo.Tool._04_Form
{
    public partial class LoginForm : Form
    {
        public string UserName { get; private set; }
        public string Pwd { get; private set; }
        public string Code { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            UserName = "";
            Pwd = "";
            Code = "";

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("UserName Is Empty", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            Pwd = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(Pwd))
            {
                MessageBox.Show("Pwd Is Empty", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            Code = textBox3.Text.Trim();
            if (string.IsNullOrEmpty(Code))
            {
                MessageBox.Show("Code Is Empty", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trx.Model;

namespace Trx
{
    public partial class Form2 : Form
    {
        SqlQuery sqlQuery = new SqlQuery();
        public Form2()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            Text = "TRX Studio - Форма авторизации.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logIn();
        }

        private void logIn()
        {
            WorkerModel worker = sqlQuery.SelectAllFromWorkerWhereWorkerLoginWorkerPassword(textBox1.Text, textBox2.Text);
            if (worker != null)
            {
                Form1 form1 = new Form1(worker.Id);
                form1.Show();
                Close();
            }
            else
            {
                MessageBox.Show(
                        "Не верные данные",
                        "Упс...",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        public void CloseApp()
        {
            Application.Exit();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                logIn();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                logIn();
        }
    }
}

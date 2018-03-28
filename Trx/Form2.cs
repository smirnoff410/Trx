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
            WorkerModel worker = sqlQuery.SelectAllFromWorkerWhereWorkerLoginWorkerPassword(textBox1.Text, textBox2.Text);
            if (worker.first_name != null)
            {
                Form1 form1 = new Form1(worker.Id);
                form1.Show();
                Close();
            }
            else
            {
                MessageBox.Show(
                        "Работник не найден",
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
    }
}

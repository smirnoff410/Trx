using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Data.SqlClient;

namespace Trx
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        string userId;
        string stringConnection;

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private ZXing.BarcodeReader reader;

        delegate void SetStringDelegate(String parameter);
        delegate void SetTextBoxDelegate(TextBox textBox, String parameter);
        delegate void SetComboBoxDelegate(ComboBox comboBox, String parameter);

        public Form1()
        {
            InitializeComponent();
        }

        void SetResult(string result)
        {
            if(!InvokeRequired)
                userId = result;
            else
                Invoke(new SetStringDelegate(SetResult), new object[] { result });
        }

        void SetTextBox(TextBox textBox, string result)
        {
            if (!InvokeRequired)
                textBox.Text = result;
            else
                Invoke(new SetTextBoxDelegate(SetTextBox), new object[] { textBox, result });
        }
        
        private void SetComboBox(ComboBox comboBox, string v)
        {
            if (!InvokeRequired)
            {
                comboBox.Items.Add(v);
                comboBox.SelectedText = "---";
            }
            else
                Invoke(new SetComboBoxDelegate(SetComboBox), new object[] { comboBox, v });
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vladislav\source\repos\Trx\Trx\Database.mdf;Integrated Security=True";

            sqlConnection = new SqlConnection(stringConnection);

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            reader = new ZXing.BarcodeReader();
            reader.Options.PossibleFormats = new List<ZXing.BarcodeFormat>();
            reader.Options.PossibleFormats.Add(ZXing.BarcodeFormat.QR_CODE);

            if(videoDevices.Count > 0)
            {
                foreach(FilterInfo device in videoDevices)
                {
                    lbCams.Items.Add(device.Name);
                }
                lbCams.SelectedIndex = 0;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(videoDevices[lbCams.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_newFrame);
            videoSource.Start();
        }

        private void video_newFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bitmap = (Bitmap)e.Frame.Clone();

            pbWebCamPreview.Image = bitmap;

            ZXing.Result result = reader.Decode((Bitmap)e.Frame.Clone());
            if(result != null)
            {
                MessageBox.Show(
                    "Пользователь найден",
                    "Пользователь найден",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                SetResult(result.Text);

                sqlConnection = new SqlConnection(stringConnection);
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception ex)
                {

                }
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [User] WHERE Id = " + userId, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        SetTextBox(textBox1, dr.GetValue(1).ToString().Trim());
                        SetTextBox(textBox2, dr.GetValue(2).ToString().Trim());
                        SetTextBox(textBox3, dr.GetValue(3).ToString().Trim());
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();

                sqlConnection = new SqlConnection(stringConnection);
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception ex)
                {

                }

                SqlCommand sqlCommand1 = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId, sqlConnection);
                using (SqlDataReader dr = sqlCommand1.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        SetComboBox(cbTraine, dr.GetValue(2).ToString().Trim());
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();

                if (videoSource != null)
                {
                    videoSource.SignalToStop();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(videoSource != null)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void cbTraine_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = cbTraine.SelectedItem.ToString();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId + " AND traine_type = N'" + selectedState + "'", sqlConnection);
            using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    SetTextBox(textBox5, dr.GetValue(4).ToString().Trim());
                    SetTextBox(textBox4, dr.GetValue(5).ToString().Trim());
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        private void decTraine_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите списать тренировку?",
                "Списание 1-ой тренировки",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.DefaultDesktopOnly);
            if(result == DialogResult.Yes)
            {
                sqlConnection = new SqlConnection(stringConnection);
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception ex)
                {

                }

                int dec = Convert.ToInt32(textBox5.Text) - 1;
                SqlCommand sqlCommand = new SqlCommand("UPDATE [UserTraine] SET count_traine = " + dec + " WHERE id_user = " + userId + " AND traine_type = N'" + cbTraine.SelectedItem.ToString() + "'", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                textBox5.Text = dec.ToString();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}

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
using Trx.Model;

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
                SetResult(result.Text);

                SqlQuery sqlQuery = new SqlQuery();
                //Запрос получения пользователя по считанному Id
                List<UserModel> userModel = new List<UserModel>();
                userModel = sqlQuery.SelectAllFromUserWhereUserId(userId);
                if(userModel.Count > 0)
                {
                    MessageBox.Show(
                        "Пользователь найден",
                        "Ура!!!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    
                    SetTextBox(textBox1, userModel[0].first_name);
                    SetTextBox(textBox2, userModel[0].second_name);
                    SetTextBox(textBox3, userModel[0].last_name);

                    //Запрос получения приобретённых пакетов пользователем
                    List<string> traineType = new List<string>();
                    traineType = sqlQuery.SelectTraineTypeFromUserTraineWhereUserId(userId);
                    for (int i = 0; i < traineType.Count; i++)
                    {
                        SetComboBox(cbTraine, traineType[i]);
                    }
                } else
                {
                    MessageBox.Show(
                        "Пользователь не найден",
                        "Упс...",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                }

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

            SqlQuery sqlQuery = new SqlQuery();
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            userTraineModel = sqlQuery.SelectAllFromUserTraineWhereUserIdAndTraineType(userId, selectedState);
            SetTextBox(textBox5, userTraineModel[0].count_traine.ToString());
            SetTextBox(textBox4, userTraineModel[0].date_start);
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
                //Уменьшение количества занятий на 1
                SqlQuery sqlQuery = new SqlQuery();
                textBox5.Text = sqlQuery.UpdateUserTraineSetCountTraineWhereUserIdAndTraineType(userId, cbTraine.SelectedItem.ToString(), Convert.ToInt32(textBox5.Text)).ToString();
            }
        }
    }
}

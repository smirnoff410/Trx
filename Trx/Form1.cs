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
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;

namespace Trx
{
    public partial class Form1 : Form
    {
        SqlQuery sqlQuery = new SqlQuery();
        string userId;
        private WorkerModel worker;

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private ZXing.BarcodeReader reader;
        private bool countExitApplication = true;

        delegate void SetStringDelegate(String parameter);
        delegate void SetLabelDelegate(Label label, String parameter);
        delegate void SetTextBoxDelegate(TextBox textBox, String parameter);
        delegate void SetComboBoxDelegate(ComboBox comboBox, String parameter);

        public Form1(int Id)
        {
            InitializeComponent();
            worker = sqlQuery.SelectAllFromWorkerWhereWorkerId(Id.ToString());
            Text = "TRX Studio - Администрирование. Добро пожаловать, " + worker.first_name + " " + worker.second_name + "!";
            if (worker.id_role != 1)
                SaveToolStripMenuItem1.Enabled = false;
        }

        void SetResult(string result)
        {
            if(!InvokeRequired)
                userId = result;
            else
                Invoke(new SetStringDelegate(SetResult), new object[] { result });
        }

        void SetLabel(Label label, string v)
        {
            if (!InvokeRequired)
                label.Text = v;
            else
                Invoke(new SetLabelDelegate(SetLabel), new object[] { label, v });
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
            }
            else
                Invoke(new SetComboBoxDelegate(SetComboBox), new object[] { comboBox, v });
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            List<UserTraineModel> userTraineModelUpdate = sqlQuery.SelectAllFromUserTraine();
            for(int i = 0; i < userTraineModelUpdate.Count; i++)
            {
                if (userTraineModelUpdate[i].date_finish < Convert.ToDecimal(ConvertToUnixTimestamp(DateTime.Today)))
                    sqlQuery.UpdateUserTraineSetCountTraineWhereUserTraineId(userTraineModelUpdate[i].Id.ToString());
            }
            btnAdd.Enabled = false;

            EnabledWidget(false);

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

            ///////////////////////////////////////////////////////////////////////////////////////Пользователь////////////////////////////////////////////////////////////////////
            List<UserModel> users = sqlQuery.SelectAllFromUser();
            
            for(int i = 0; i < users.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(users[i].Id.ToString());
                lvi.SubItems.Add(users[i].first_name);
                lvi.SubItems.Add(users[i].second_name);
                lvi.SubItems.Add(users[i].last_name);
                listView1.Items.Add(lvi);
            }

            this.Invoke(new EventHandler(delegate { textBox5.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox6.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox7.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { dateTimePicker6.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox9.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox10.Enabled = false; }));

            /////////////////////////////////////////////////////////////////////////////////////////Статистика///////////////////////////////////////////////////////////////////////
            List<WorkerModel> workerModel = sqlQuery.SelectAllFromWorker();
            for(int i = 0; i < workerModel.Count; i++)
            {
                List<WorkerTraineModel> traineModel = sqlQuery.SelectAllFromWorkerTraine();
                for(int j = 0; j < traineModel.Count; j++)
                {
                    List<ScheduleModel> scheduleModel3 = sqlQuery.SelectCountTraineFromScheduleWhereWorkerAndTraine(workerModel[i].second_name, traineModel[j].type);
                    if(scheduleModel3.Count != 0)
                    {
                        int count_user = 0;
                        ListViewItem lvi2 = new ListViewItem(workerModel[i].Id.ToString());
                        lvi2.SubItems.Add(workerModel[i].first_name);
                        lvi2.SubItems.Add(workerModel[i].second_name);
                        lvi2.SubItems.Add(workerModel[i].last_name);
                        lvi2.SubItems.Add(scheduleModel3.Count.ToString());
                        lvi2.SubItems.Add(traineModel[j].type);
                        for (int k = 0; k < scheduleModel3.Count; k++)
                            count_user+= scheduleModel3[k].count_user;
                        lvi2.SubItems.Add(count_user.ToString());
                        listView4.Items.Add(lvi2);
                    }
                }
            }

            List<TraineModel> traineModel1 = sqlQuery.SelectAllFromTraine();
            for(int i = 0; i < traineModel1.Count; i++)
            {
                List<UserTraineModel> userTraine = sqlQuery.SelectAllFromUserTraineWhereTraineType(traineModel1[i].type);
                if(userTraine.Count > -1)
                {
                    int count_price = 0;
                    ListViewItem lvi3 = new ListViewItem(traineModel1[i].type);
                    lvi3.SubItems.Add(userTraine.Count.ToString());
                    for (int j = 0; j < userTraine.Count; j++)
                        count_price += userTraine[j].price;
                    lvi3.SubItems.Add(count_price.ToString());
                    listView6.Items.Add(lvi3);
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////Расписание////////////////////////////////////////////////////////////////
            dateTimePicker3.Format = DateTimePickerFormat.Time;
            dateTimePicker4.Format = DateTimePickerFormat.Time;
            List<ScheduleModel> schedule = sqlQuery.SelectAllFromSchedule();

            for(int i = 0; i < schedule.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(schedule[i].Id.ToString());
                lvi.SubItems.Add(schedule[i].worker);
                lvi.SubItems.Add(schedule[i].traine);
                lvi.SubItems.Add(ConvertFromUnixTimestamp(Convert.ToDouble(schedule[i].date_start)).ToString());
                lvi.SubItems.Add(ConvertFromUnixTimestamp(Convert.ToDouble(schedule[i].date_end)).ToString());
                lvi.SubItems.Add(schedule[i].count_user.ToString());
                listView3.Items.Add(lvi);
            }

            List<WorkerModel> worker = sqlQuery.SelectAllFromWorker();
            for(int i = 0; i < worker.Count; i++)
            {
                SetComboBox(comboBox1, worker[i].second_name);
            }
            List<WorkerTraineModel> workerTraine = sqlQuery.SelectAllFromWorkerTraine();
            for (int i = 0; i < workerTraine.Count; i++)
            {
                SetComboBox(comboBox2, workerTraine[i].type);
            }


            ///////////////////////////////////////////////////////////////////////////////////Сотрудники//////////////////////////////////////////////////////////////////////
            List<WorkerModel> workers = sqlQuery.SelectAllFromWorker();

            for (int i = 0; i < workers.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(workers[i].Id.ToString());
                lvi.SubItems.Add(workers[i].first_name);
                lvi.SubItems.Add(workers[i].second_name);
                lvi.SubItems.Add(workers[i].last_name);
                listView2.Items.Add(lvi);
            }

            this.Invoke(new EventHandler(delegate { textBox16.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox17.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox18.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox19.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox20.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox4.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox26.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox27.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox28.Enabled = false; }));
            this.Invoke(new EventHandler(delegate { textBox29.Enabled = false; }));

            ////////////////////////////////////////////////////////////////////////////////////////Услуги////////////////////////////////////////////////////////////////
            List<TraineModel> traineModel2 = sqlQuery.SelectAllFromTraine();
            for(int i = 0; i < traineModel2.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(traineModel2[i].Id.ToString());
                if (traineModel2[i].subscription == 1)
                    lvi.SubItems.Add("Абонемент");
                else
                    lvi.SubItems.Add("Не абонемент");
                lvi.SubItems.Add(traineModel2[i].type);
                lvi.SubItems.Add(traineModel2[i].price.ToString());
                lvi.SubItems.Add(traineModel2[i].count_raine.ToString());
                lvi.SubItems.Add(Convert.ToInt32(TimeSpan.FromSeconds(Convert.ToDouble(traineModel2[i].validity)).TotalDays).ToString());
                listView5.Items.Add(lvi);
            }
            SetComboBox(comboBox9, "Абонемент");
            SetComboBox(comboBox9, "Не абонемент");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label54.Text = "";
            label57.Text = "";
            label45.Text = "";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            comboBox5.Enabled = false;
            cbTraine2.Enabled = false;
            videoSource = new VideoCaptureDevice(videoDevices[lbCams.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_newFrame);
            videoSource.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(
                           "Вы действительно хотите заморозить услугу клиента на 1 неделю?",
                           "Предупреждение",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning,
                           MessageBoxDefaultButton.Button2,
                           MessageBoxOptions.DefaultDesktopOnly);
                if(dialogResult == DialogResult.Yes)
                    sqlQuery.UpdateUserTraineSetDateFinishWhereUserIdAndTraineType(userId, comboBox8.SelectedItem.ToString(), ConvertToUnixTimestamp(Convert.ToDateTime(label16.Text)) + 604800, label10.Text, ConvertToUnixTimestamp(Convert.ToDateTime(label11.Text)), ConvertToUnixTimestamp(Convert.ToDateTime(label16.Text)), label56.Text);
            }
            catch(Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
            }
        }

        private void video_newFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bitmap = (Bitmap)e.Frame.Clone();

            pbWebCamPreview.Image = bitmap;

            ZXing.Result result = reader.Decode((Bitmap)e.Frame.Clone());
            if(result != null)
            {
                SetResult(result.Text);
                
                //Запрос получения пользователя по считанному Id
                List<UserModel> userModel = new List<UserModel>();
                userModel = sqlQuery.SelectAllFromUserWhereUserId(userId);
                if(comboBox8.Items.Count != 0)
                {
                    this.Invoke(new EventHandler(delegate { comboBox8.Items.Clear();}));
                    this.Invoke(new EventHandler(delegate { comboBox8.SelectedIndex = -1; }));
                    this.Invoke(new EventHandler(delegate { label10.Text = ""; }));
                    this.Invoke(new EventHandler(delegate { label53.Text = ""; }));
                    this.Invoke(new EventHandler(delegate { label11.Text = ""; }));
                    this.Invoke(new EventHandler(delegate { label16.Text = ""; }));
                    this.Invoke(new EventHandler(delegate { label56.Text = ""; }));
                }
                    
                List<UserTraineModel> userTraineModel = sqlQuery.SelectAllFromUserTraineWhereUserId(userId);
                if (userModel.Count > 0)
                {
                    //label10.Text = "";
                    //label11.Text = "";
                    //label16.Text = "";
                    //label13.Text = "";
                    MessageBox.Show(
                        "Пользователь найден",
                        "Ура!!!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                                        
                    SetLabel(label7, userModel[0].first_name);
                    SetLabel(label8, userModel[0].second_name);
                    SetLabel(label9, userModel[0].last_name);
                    for(int i = 0; i < userTraineModel.Count; i++)
                    {
                        if (userTraineModel[i].count_traine > 0)
                        {
                            SetComboBox(comboBox8, userTraineModel[i].traine_type);
                        }
                    }

                    //Запрос получения приобретённых пакетов пользователем
                    //List<UserTraineModel> traineType = new List<UserTraineModel>();
                    //traineType = sqlQuery.SelectAllUserTraineWhereUserId(userId);
                } else
                {
                    DialogResult dialogResult = MessageBox.Show(
                        "Пользователь не найден, добавить нового пользователя?",
                        "Упс...",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2,
                        MessageBoxOptions.DefaultDesktopOnly);
                    if(dialogResult == DialogResult.Yes)
                    {
                        this.Invoke(new EventHandler(delegate { btnAdd.Enabled = true; }));
                        ClearMainLabel();
                        EnabledWidget(true);
                        //List<TraineModel> traine = new List<TraineModel>();
                        //traine = sqlQuery.SelectAllFromTraine();
                        //for (int i = 0; i < traine.Count; i++)
                        //{
                        //    SetComboBox(cbTraine2, traine[i].type);
                        //}
                        string[] substraction = { "Абонемент", "Не абонемент" };
                        for(int i = 0; i < 2; i++)
                        {
                            SetComboBox(comboBox5, substraction[i]);
                        }
                    }
                }

                if (videoSource != null)
                {
                    videoSource.SignalToStop();
                }
            }
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itm = comboBox8.SelectedItem.ToString();
            int index = comboBox8.SelectedIndex;
            TraineModel traineModel = sqlQuery.SelectAllFromTraineWhereType(itm);
            List<UserTraineModel> userTraineModel = sqlQuery.SelectAllFromUserTraineWhereUserIdAndCountTraine(userId);
            if (traineModel.subscription == 1)
                SetLabel(label53, "Абонемент");
            else
                SetLabel(label53, "Не абонемент");
            SetLabel(label10, userTraineModel[index].count_traine.ToString());
            SetLabel(label11, ConvertFromUnixTimestamp(Convert.ToDouble(userTraineModel[index].date_start)).ToString());
            SetLabel(label16, ConvertFromUnixTimestamp(Convert.ToDouble(userTraineModel[index].date_finish)).ToString());
            SetLabel(label45, userTraineModel[index].sale.ToString());
            label45.Text += "%";
            SetLabel(label56, userTraineModel[index].price.ToString());
        }

        private void ClearMainLabel()
        {
            this.Invoke(new EventHandler(delegate { label7.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label8.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label9.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label53.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label10.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label11.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label16.Text = ""; }));
            this.Invoke(new EventHandler(delegate { label56.Text = ""; }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(videoSource != null)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
            if (countExitApplication)
            {
                Form2 form2 = new Form2();
                form2.CloseApp();
                countExitApplication = false;
            }
            else
                countExitApplication = true;
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
                try
                {
                    if (userId == null)
                    {
                        MessageBox.Show(
                            "Пользователь не считан",
                            "Предупреждение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        if (Convert.ToInt32(label10.Text) > 0)
                        {
                            int count_traine_dec = sqlQuery.UpdateUserTraineSetCountTraineWhereUserIdAndTraineType(userId, comboBox8.SelectedItem.ToString(), Convert.ToInt32(label10.Text), label10.Text, ConvertToUnixTimestamp(Convert.ToDateTime(label11.Text)), ConvertToUnixTimestamp(Convert.ToDateTime(label16.Text)), label56.Text);
                            if (count_traine_dec > -1)
                                label10.Text = count_traine_dec.ToString();

                            int maxUserVisits = sqlQuery.SelectMaxIdFromUserVisits();
                            UserVisitsModel userVisits = new UserVisitsModel
                            {
                                Id = ++maxUserVisits,
                                id_user = Convert.ToInt32(userId),
                                traine_type = comboBox8.SelectedItem.ToString(),
                                count_traine = count_traine_dec,
                                date = Convert.ToDecimal(ConvertToUnixTimestamp(DateTime.Now))
                            };
                            sqlQuery.InsertIntoUserVisits(userVisits);
                        }
                        else
                        {
                            MessageBox.Show(
                            "У пользователя закончилась услуга!",
                            "Предупреждение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                }
                catch(Exception ex)
                {
                    ErrorMessage.ErrorMessage1("Ошибка");
                }
            }
        }

        private void btnAdd_click(object sender, EventArgs e)
        {
            try
            {
                if (sqlQuery.SelectAllFromTraineWhereUserIdAndTraineType(userId, cbTraine2.SelectedItem.ToString()))
                {
                    MessageBox.Show(
                    "У пользователя уже приобретён пакет. Выберите другой.",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    List<UserModel> userModel = sqlQuery.SelectAllFromUserWhereUserId(userId);
                    int result = 1;

                    if (userModel.Count == 0)
                    {
                        //Добавляем нового пользователя
                        UserModel user = new UserModel
                        {
                            Id = Convert.ToInt32(userId),
                            first_name = textBox1.Text,
                            second_name = textBox2.Text,
                            last_name = textBox3.Text
                        };
                        result = sqlQuery.InsertIntoUser(user);
                    }
                    int maxUserTraine = sqlQuery.SelectMaxIdFromUserTraine();
                    if (maxUserTraine > -1)
                    {
                        double date_finish = ConvertToUnixTimestamp(Convert.ToDateTime(label17.Text));
                        UserTraineModel userTraineModel = new UserTraineModel
                        {
                            Id = ++maxUserTraine,
                            id_user = Convert.ToInt32(userId),
                            traine_type = cbTraine2.SelectedItem.ToString(),
                            count_traine = Convert.ToInt32(label54.Text),
                            date_start = Convert.ToDecimal(ConvertToUnixTimestamp(DateTime.Today)),
                            date_finish = Convert.ToDecimal(date_finish),
                            price = Convert.ToInt32(label57.Text),
                            sale = Convert.ToInt32(label13.Text.Trim(new char[] { '%' }))
                        };
                        int result1 = sqlQuery.InsertIntoUserTraine(userTraineModel);
                        if (result == 1 && result1 == 1)
                        {
                            MessageBox.Show(
                            "Пользователь успешно создан",
                            "Пользователь успешно создан",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);

                            EnabledWidget(false);
                            label57.Text = "";
                        }
                        else
                        {
                            MessageBox.Show(
                            "Пользователь не создан",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Заполнены не все поля",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void btnAdd_click1(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new EventHandler(delegate { btnAdd.Enabled = true; }));
                EnabledWidget(true);
                List<TraineModel> traine = new List<TraineModel>();
                traine = sqlQuery.SelectAllFromTraine();
                comboBox5.Items.Clear();
                string[] substraction = { "Абонемент", "Не абонемент" };
                for (int i = 0; i < 2; i++)
                {
                    SetComboBox(comboBox5, substraction[i]);
                }
                //for (int i = 0; i < traine.Count; i++)
                //{
                //    SetComboBox(cbTraine2, traine[i].type);
                //}
                List<UserModel> userModel = sqlQuery.SelectAllFromUserWhereUserId(userId);
                textBox1.Text = userModel[0].first_name;
                textBox2.Text = userModel[0].second_name;
                textBox3.Text = userModel[0].last_name;
            }
            catch(Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
            }
        }

        private void EnabledWidget(bool flag)
        {
            if (flag)
            {
                this.Invoke(new EventHandler(delegate { textBox1.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox2.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox3.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { label14.Text = DateTime.Today.ToString(); }));
                this.Invoke(new EventHandler(delegate { cbTraine2.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { comboBox5.Enabled = true; }));

                this.Invoke(new EventHandler(delegate { cbTraine2.SelectedIndex = -1; }));
                this.Invoke(new EventHandler(delegate { comboBox5.SelectedIndex = -1; }));
            }
            else
            {
                this.Invoke(new EventHandler(delegate { textBox1.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { textBox2.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { textBox3.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { label54.Text = ""; }));
                this.Invoke(new EventHandler(delegate { label14.Text = ""; }));
                this.Invoke(new EventHandler(delegate { label17.Text = ""; }));
                this.Invoke(new EventHandler(delegate { label57.Text = ""; }));
                this.Invoke(new EventHandler(delegate { cbTraine2.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { comboBox5.Enabled = false; }));

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                this.Invoke(new EventHandler(delegate { cbTraine2.SelectedIndex = -1; }));
                //this.Invoke(new EventHandler(delegate { comboBox5.SelectedIndex = -1; }));
                comboBox5.Items.Clear();
                cbTraine2.Items.Clear();
            }
        }

        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private bool SearchRegex(string str, string substr)
        {
            Regex regex1 = new Regex(@substr);
            MatchCollection matches = regex1.Matches(str);
            if (matches.Count > 0)
                return true;
            else
                return false;
        }

        private void cbTraine2_SelectedIndexChanged(object sender, EventArgs e)
        {
            double date_finish;
            if(cbTraine2.SelectedItem != null)
            {
                TraineModel traineModel = sqlQuery.SelectAllFromTraineWhereType(cbTraine2.SelectedItem.ToString());
                label54.Text = traineModel.count_raine.ToString();
                date_finish = Convert.ToDouble(traineModel.validity) + ConvertToUnixTimestamp(DateTime.Today);
                label17.Text = ConvertFromUnixTimestamp(date_finish).ToString();
                if (sqlQuery.SelectAllFromTraineWhereUserIdForSale(userId) >= 3)
                {
                    label13.Text = "10%";
                    label57.Text = (traineModel.price - (traineModel.price * 0.1)).ToString();
                }
                else
                {
                    label13.Text = "0%";
                    label57.Text = traineModel.price.ToString();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////// Пользователи ////////////////////////////////////////////////////////////////////////
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(
                           "Вы действительно хотите заморозить услугу клиента на 1 неделю?",
                           "Предупреждение",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning,
                           MessageBoxDefaultButton.Button2,
                           MessageBoxOptions.DefaultDesktopOnly);
                if (dialogResult == DialogResult.Yes)
                    sqlQuery.UpdateUserTraineSetDateFinishWhereUserIdAndTraineType(label25.Text, comboBox7.SelectedItem.ToString(), ConvertToUnixTimestamp(Convert.ToDateTime(label65.Text)) + 604800, label63.Text, ConvertToUnixTimestamp(Convert.ToDateTime(label64.Text)), ConvertToUnixTimestamp(Convert.ToDateTime(label65.Text)), label66.Text);
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
            }
        }

        private void btnFind2_Click(object sender, EventArgs e)
        {
            List<UserModel> users = sqlQuery.SelectAllFromUserWhereSecondName(textBox33.Text);
            listView1.Items.Clear();

            for (int i = 0; i < users.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(users[i].Id.ToString());
                lvi.SubItems.Add(users[i].first_name);
                lvi.SubItems.Add(users[i].second_name);
                lvi.SubItems.Add(users[i].last_name);
                listView1.Items.Add(lvi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите списать тренировку?",
                "Списание 1-ой тренировки",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                try
                {
                    if(Convert.ToInt32(label63.Text) != 0)
                    {
                        int count_traine_dec = sqlQuery.UpdateUserTraineSetCountTraineWhereUserIdAndTraineType(label25.Text, comboBox7.SelectedItem.ToString(), Convert.ToInt32(label63.Text), label63.Text, ConvertToUnixTimestamp(Convert.ToDateTime(label64.Text)), ConvertToUnixTimestamp(Convert.ToDateTime(label65.Text)), label66.Text);
                        if (count_traine_dec > -1)
                            label63.Text = count_traine_dec.ToString();

                        int maxUserVisits = sqlQuery.SelectMaxIdFromUserVisits();
                        UserVisitsModel userVisits = new UserVisitsModel
                        {
                            Id = ++maxUserVisits,
                            id_user = Convert.ToInt32(label25.Text),
                            traine_type = comboBox7.SelectedItem.ToString(),
                            count_traine = count_traine_dec,
                            date = Convert.ToDecimal(ConvertToUnixTimestamp(DateTime.Now))
                        };
                        sqlQuery.InsertIntoUserVisits(userVisits);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(
                    "Ошибка",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            List<UserModel> users = sqlQuery.SelectAllFromUser();
            listView1.Items.Clear();

            for (int i = 0; i < users.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(users[i].Id.ToString());
                lvi.SubItems.Add(users[i].first_name);
                lvi.SubItems.Add(users[i].second_name);
                lvi.SubItems.Add(users[i].last_name);
                listView1.Items.Add(lvi);
            }
        }

        private void btnEdit2_Click(object sender, EventArgs e)
        {
            this.Invoke(new EventHandler(delegate { btnSave2.Enabled = true; }));
            this.Invoke(new EventHandler(delegate { textBox5.Enabled = true; }));
            this.Invoke(new EventHandler(delegate { textBox6.Enabled = true; }));
            this.Invoke(new EventHandler(delegate { textBox7.Enabled = true; }));
            this.Invoke(new EventHandler(delegate { dateTimePicker6.Enabled = true; }));
            this.Invoke(new EventHandler(delegate { textBox9.Enabled = true; }));
            this.Invoke(new EventHandler(delegate { textBox10.Enabled = true; }));
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //comboBox6.Items.Clear();
            label25.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox5.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox6.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBox7.Text = listView1.SelectedItems[0].SubItems[3].Text;
            List<UserModel> userModel = sqlQuery.SelectAllFromUserWhereUserId(listView1.SelectedItems[0].SubItems[0].Text);
            dateTimePicker6.Value = ConvertFromUnixTimestamp(Convert.ToDouble(userModel[0].age));
            DateTime bday = ConvertFromUnixTimestamp(Convert.ToDouble(userModel[0].age));
            DateTime now = DateTime.Today;
            int age = now.Year - bday.Year;
            if (bday > now.AddYears(-age)) age--;
            label46.Text = age.ToString();
            textBox9.Text = userModel[0].phone;
            textBox10.Text = userModel[0].email;
            string[] str = { "Приобретённые услуги", "Действующие услуги" };
            for (int i = 0; i < 2; i++)
                SetComboBox(comboBox6, str[i]);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.Invoke(new EventHandler(delegate { comboBox7.SelectedIndex = -1; }));
            comboBox7.Items.Clear();
            string selectedState = comboBox6.SelectedItem.ToString();
            List<UserTraineModel> userTraineModel = sqlQuery.SelectAllFromUserTraineWhereUserId(label25.Text);
            if (selectedState == "Приобретённые услуги")
            {
                for (int i = 0; i < userTraineModel.Count; i++)
                {
                    SetComboBox(comboBox7, userTraineModel[i].traine_type);
                }
            }
            else
            {
                for (int i = 0; i < userTraineModel.Count; i++)
                {
                    if(userTraineModel[i].count_traine > 0 && userTraineModel[i].date_finish > Convert.ToDecimal(ConvertToUnixTimestamp(DateTime.Today)))
                        SetComboBox(comboBox7, userTraineModel[i].traine_type);
                }
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedState = comboBox6.SelectedItem.ToString();
                int index = comboBox7.SelectedIndex;
                List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
                if (selectedState == "Приобретённые услуги")
                {
                    userTraineModel = sqlQuery.SelectAllFromUserTraineWhereUserId(label25.Text);
                }
                else
                {
                    userTraineModel = sqlQuery.SelectAllFromUserTraineWhereUserIdAndCountTraineAndDateFinish(label25.Text, ConvertToUnixTimestamp(DateTime.Today));
                }
                if (userTraineModel.Count != 0)
                {
                    label63.Text = userTraineModel[index].count_traine.ToString();
                    label64.Text = ConvertFromUnixTimestamp(Convert.ToDouble(userTraineModel[index].date_start)).ToString();
                    label65.Text = ConvertFromUnixTimestamp(Convert.ToDouble(userTraineModel[index].date_finish)).ToString();
                    label66.Text = userTraineModel[index].price.ToString();
                }
            }
            catch(Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            this.Invoke(new EventHandler(delegate { btnSave2.Enabled = false; }));
            UserModel user = new UserModel()
            {
                Id = Convert.ToInt32(label25.Text),
                first_name = textBox5.Text,
                second_name = textBox6.Text,
                last_name = textBox7.Text,
                age = Convert.ToDecimal(ConvertToUnixTimestamp(dateTimePicker6.Value)),
                phone = textBox9.Text,
                email = textBox10.Text
            };
            if(sqlQuery.UpdateUserSetAllWhereUserId(user))
            {
                MessageBox.Show(
                    "Пользователь успешно изменён",
                    "Пользователь успешно изменён",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                this.Invoke(new EventHandler(delegate { textBox5.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { textBox6.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { textBox7.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { dateTimePicker6.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { textBox9.Enabled = false; }));
                this.Invoke(new EventHandler(delegate { textBox10.Enabled = false; }));
            }
            else
            {
                MessageBox.Show(
                    "Ошибка",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }
        
        /////////////////////////////////////////////////////////////// Сотрудники /////////////////////////////////////////////////////////////////////

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            WorkerModel worker = sqlQuery.SelectAllFromWorkerWhereWorkerId(listView2.SelectedItems[0].SubItems[0].Text);
            label38.Text = listView2.SelectedItems[0].SubItems[0].Text;
            textBox16.Text = listView2.SelectedItems[0].SubItems[1].Text;
            textBox17.Text = listView2.SelectedItems[0].SubItems[2].Text;
            textBox18.Text = listView2.SelectedItems[0].SubItems[3].Text;
            textBox19.Text = worker.login;
            string role;
            if (worker.id_role == 1)
                role = "Администратор";
            else
                role = "Тренер";
            textBox20.Text = role;
            textBox4.Text = ConvertFromUnixTimestamp(Convert.ToDouble(worker.date_of_birth)).ToString();
            textBox26.Text = worker.phone;
            textBox27.Text = worker.education;
            textBox28.Text = worker.special_course;
            textBox29.Text = ConvertFromUnixTimestamp(Convert.ToDouble(worker.date_start)).ToString();
        }

        private void btnEdit5_Click(object sender, EventArgs e)
        {
            if(worker.id_role == 1)
            {
                this.Invoke(new EventHandler(delegate { textBox16.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox17.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox18.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox19.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox20.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox4.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox26.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox27.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox28.Enabled = true; }));
                this.Invoke(new EventHandler(delegate { textBox29.Enabled = true; }));
            }
            else
                MessageBox.Show(
                        "У Вас нет прав доступа для редактирования сотрудника",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
        }

        private void btnSave5_Click(object sender, EventArgs e)
        {
            try
            {
                WorkerModel worker = new WorkerModel()
                {
                    Id = Convert.ToInt32(label38.Text),
                    first_name = textBox16.Text,
                    second_name = textBox17.Text,
                    last_name = textBox18.Text,
                    login = textBox19.Text,
                    id_role = Convert.ToInt32(textBox20.Text),
                    date_of_birth = Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(textBox4.Text))),
                    phone = textBox26.Text,
                    education = textBox27.Text,
                    special_course = textBox28.Text,
                    date_start = Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(textBox29.Text)))
                };
                if (sqlQuery.UpdateWorkerSetAllWhereWorkerId(worker))
                {
                    MessageBox.Show(
                        "Сотрудник успешно изменён",
                        "Сотрудник успешно изменён",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    this.Invoke(new EventHandler(delegate { textBox16.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox17.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox18.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox19.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox20.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox4.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox26.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox27.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox28.Enabled = false; }));
                    this.Invoke(new EventHandler(delegate { textBox29.Enabled = false; }));
                }
                else
                {
                    MessageBox.Show(
                        "Ошибка",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch(Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
            }
            
        }

        private void btnReset5_Click(object sender, EventArgs e)
        {
            List<WorkerModel> workers = sqlQuery.SelectAllFromWorker();
            listView2.Items.Clear();

            for (int i = 0; i < workers.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(workers[i].Id.ToString());
                lvi.SubItems.Add(workers[i].first_name);
                lvi.SubItems.Add(workers[i].second_name);
                lvi.SubItems.Add(workers[i].last_name);
                listView2.Items.Add(lvi);
            }
        }

        private void btnRemove5_Click(object sender, EventArgs e)
        {
            if(worker.id_role == 1)
            {
                if (sqlQuery.DeleteWorkerWhereId(listView2.SelectedItems[0].SubItems[0].Text))
                    if (listView2.SelectedItems.Count != 0)
                    {
                        listView2.SelectedItems[0].Remove();
                        MessageBox.Show(
                            "Сотрудник удалён",
                            "Успешно!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
            }
            else
                MessageBox.Show(
                    "У вас нет прав на удаление сотрудника",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
        }

        private void btnAdd5_Click(object sender, EventArgs e)
        {
            if (worker.id_role == 1)
            {
                if (textBox11.Text != "" || textBox12.Text != "" || textBox13.Text != "" || textBox14.Text != "" || textBox15.Text != "" || textBox21.Text != "")
                {

                    if (!sqlQuery.SelectAllFromWorkerWhereWorkerLogin(textBox14.Text))
                    {
                        int role;
                        if (textBox21.Text == "Администратор")
                            role = 1;
                        else
                            role = 2;
                        int max = sqlQuery.SelectMaxIdFromWorker();
                        if (max > -1)
                        {
                            WorkerModel worker = new WorkerModel()
                            {
                                Id = ++max,
                                first_name = textBox11.Text,
                                second_name = textBox12.Text,
                                last_name = textBox13.Text,
                                login = textBox14.Text,
                                password = textBox15.Text,
                                id_role = role
                            };
                            if (sqlQuery.InsertIntoWorker(worker))
                                MessageBox.Show(
                                        "Сотрудник добавлен",
                                        "Успешно!",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                            else
                                MessageBox.Show(
                                        "Ошибка",
                                        "Ошибка",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                                    "Логин уже занят",
                                    "Выберите другой логин",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
                    MessageBox.Show(
                    "Не все поля заполнены",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
                MessageBox.Show(
                    "У Вас нет прав доступа для добавления сотрудника",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);


        }

        ///////////////////////////////////////////////////////////////////////Статистика////////////////////////////////////////////////////////////////////////


        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                listView6.Items.Clear();
                List<TraineModel> traineModel1 = sqlQuery.SelectAllFromTraine();
                for (int i = 0; i < traineModel1.Count; i++)
                {
                    List<UserTraineModel> userTraine = sqlQuery.SelectAllFromUserTraineWhereTraineType(traineModel1[i].type);
                    if (userTraine.Count > -1)
                    {
                        int count_price = 0;
                        int count_traine = 0;
                        ListViewItem lvi3 = new ListViewItem(traineModel1[i].type);
                        for (int j = 0; j < userTraine.Count; j++)
                            if(userTraine[j].date_start >= Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(dateTimePicker5.Value.ToLongDateString()))))
                            {
                                count_price += userTraine[j].price;
                                count_traine++;
                            }
                        lvi3.SubItems.Add(count_traine.ToString());
                        lvi3.SubItems.Add(count_price.ToString());
                        listView6.Items.Add(lvi3);
                    }
                }

                listView4.Items.Clear();
                List<WorkerModel> workerModel = sqlQuery.SelectAllFromWorker();
                for (int i = 0; i < workerModel.Count; i++)
                {
                    List<WorkerTraineModel> traineModel = sqlQuery.SelectAllFromWorkerTraine();
                    for (int j = 0; j < traineModel.Count; j++)
                    {
                        List<ScheduleModel> scheduleModel3 = sqlQuery.SelectCountTraineFromScheduleWhereWorkerAndTraine(workerModel[i].second_name, traineModel[j].type);
                        if (scheduleModel3.Count != 0)
                        {
                            int count_user = 0;
                            for (int k = 0; k < scheduleModel3.Count; k++)
                                if (scheduleModel3[k].date_start >= Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(dateTimePicker5.Value.ToLongDateString()))) &&
                                    scheduleModel3[k].date_end <= Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(dateTimePicker7.Value.ToLongDateString()))))
                                    count_user += scheduleModel3[k].count_user;
                            if (count_user > 0)
                            {
                                ListViewItem lvi2 = new ListViewItem(workerModel[i].Id.ToString());
                                lvi2.SubItems.Add(workerModel[i].first_name);
                                lvi2.SubItems.Add(workerModel[i].second_name);
                                lvi2.SubItems.Add(workerModel[i].last_name);
                                lvi2.SubItems.Add(scheduleModel3.Count.ToString());
                                lvi2.SubItems.Add(traineModel[j].type);
                                lvi2.SubItems.Add(count_user.ToString());
                                listView4.Items.Add(lvi2);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
            }
        }


        ///////////////////////////////////////////////////////////////////////Расписание////////////////////////////////////////////////////////////////////////

        private void btnAdd4_Click(object sender, EventArgs e)
        {
            try
            {
                int max = sqlQuery.SelectMaxIdFromSchedule();
                if (max > -1)
                {
                    ScheduleModel schedule = new ScheduleModel()
                    {
                        Id = ++max,
                        worker = comboBox1.SelectedItem.ToString(),
                        traine = comboBox2.SelectedItem.ToString(),
                        date_start = Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(dateTimePicker1.Value.ToLongDateString() + " " + dateTimePicker3.Text))),
                        date_end = Convert.ToDecimal(ConvertToUnixTimestamp(Convert.ToDateTime(dateTimePicker2.Value.ToLongDateString() + " " + dateTimePicker4.Text))),
                        count_user = Convert.ToInt32(textBox24.Text)
                    };
                    if (sqlQuery.InsertIntoSchedule(schedule))
                        MessageBox.Show(
                                "Тренировка добавлена",
                                "Успешно!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                    else
                        MessageBox.Show(
                                "Ошибка",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void btnReset4_Click(object sender, EventArgs e)
        {
            List<ScheduleModel> schedule = sqlQuery.SelectAllFromSchedule();
            listView3.Items.Clear();

            for (int i = 0; i < schedule.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(schedule[i].Id.ToString());
                lvi.SubItems.Add(schedule[i].worker);
                lvi.SubItems.Add(schedule[i].traine);
                lvi.SubItems.Add(ConvertFromUnixTimestamp(Convert.ToDouble(schedule[i].date_start)).ToString());
                lvi.SubItems.Add(ConvertFromUnixTimestamp(Convert.ToDouble(schedule[i].date_end)).ToString());
                lvi.SubItems.Add(schedule[i].count_user.ToString());
                listView3.Items.Add(lvi);
            }
        }

        private void LogInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if(e.TabPageIndex == 2 && worker.id_role != 1)
            {
                tabControl1.SelectedIndex = 0;
                MessageBox.Show(
                        "У вас нет прав доступа к этой вкладке",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            countExitApplication = false;
            Form2 form2 = new Form2();
            form2.Show();
            Close();
        }

        private void SaveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                        "Пожалуйста, сохраните и закройте все открытые процессы excel. Возможна потеря данных.",
                        "Процессы excel",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                if(result == DialogResult.Yes)
                {
                    using (SaveFileDialog dialog = new SaveFileDialog())
                    {
                        dialog.Filter = "xlsx files (*.xlsx)|*.xlsx";
                        dialog.FilterIndex = 2;
                        dialog.RestoreDirectory = true;

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            string file = dialog.FileName;
                            if (File.Exists(file))
                                File.Delete(file);
                            // Save data
                            Excel.Application oApp = new Excel.Application();
                            Excel.Workbook oBook = oApp.Workbooks.Add();
                            Excel.Worksheet oSheet = (Excel.Worksheet)oBook.Worksheets.get_Item(1);

                            string[] sData = new string[] { "#", "Имя", "Фамилия", "Отчество", "Занятий", "Тип", "Кол-во человек" };
                            for (int i = 0; i < listView4.Items.Count + 1; i++)
                            {
                                for (int c = 0; c < sData.Length; c++)
                                {
                                    if (i == 0)
                                        oSheet.Cells[i + 1, c + 1] = sData[c];
                                    else
                                        oSheet.Cells[i + 1, c + 1] = listView4.Items[i - 1].SubItems[c].Text;
                                }
                            }

                            Excel.Worksheet oSheet2 = (Excel.Worksheet)oBook.Worksheets.get_Item(2);
                            string[] sData2 = new string[] { "Услуга", "Продано", "Стоимость" };
                            for (int i = 0; i < listView6.Items.Count + 1; i++)
                            {
                                for (int c = 0; c < sData2.Length; c++)
                                {
                                    if (i == 0)
                                        oSheet2.Cells[i + 1, c + 1] = sData2[c];
                                    else
                                        oSheet2.Cells[i + 1, c + 1] = listView6.Items[i - 1].SubItems[c].Text;
                                }
                            }

                            Excel.Worksheet oSheet3 = (Excel.Worksheet)oBook.Worksheets.get_Item(3);
                            List<UserVisitsModel> userVisits = sqlQuery.SelectAllFromUserVisits();
                            userVisits.Reverse();
                            string[] sData3 = new string[] { "#", "Имя", "Фамилия", "Отчество", "Тип", "Оставшихся занятий", "Дата" };
                            for (int i = 0; i < userVisits.Count; i++)
                            {
                                List<UserModel> user = sqlQuery.SelectAllFromUserWhereUserId(userVisits[i].id_user.ToString());
                                string[] sDataModel = new string[] { userVisits[i].Id.ToString(), user[0].first_name, user[0].second_name, user[0].last_name, userVisits[i].traine_type, userVisits[i].count_traine.ToString(), ConvertFromUnixTimestamp(Convert.ToDouble(userVisits[i].date)).ToString() };
                                for (int c = 0; c < sData3.Length; c++)
                                {
                                    if (i == 0)
                                    {
                                        oSheet3.Cells[i + 1, c + 1] = sData3[c];
                                        oSheet3.Cells[i + 2, c + 1] = sDataModel[c];
                                    }
                                    else
                                        oSheet3.Cells[i + 2, c + 1] = sDataModel[c];
                                }
                            }

                            oBook.SaveAs(file);
                            oBook.Close();
                            oApp.Quit();
                            Process[] List;
                            List = Process.GetProcessesByName("EXCEL");
                            foreach (Process proc in List)
                            {
                                proc.Kill();
                            }
                            MessageBox.Show(
                                    "Статистика загружена в файл excel",
                                    "Успешно!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage.ErrorMessage1("Ошибка выгрузки статистика в excel");
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Invoke(new EventHandler(delegate { cbTraine2.SelectedIndex = -1; }));
            cbTraine2.Items.Clear();
            string selectedState = comboBox5.SelectedItem.ToString();
            string str = "";
            if (selectedState == "Абонемент")
                str = "1";
            else
                str = "2";

            List<TraineModel> TraineModel = new List<TraineModel>();
            TraineModel = sqlQuery.SelectAllFromTraineWhereSubscription(str);
            for(int i = 0; i < TraineModel.Count; i++)
            {
                SetComboBox(cbTraine2, TraineModel[i].type);
            }
        }

        /////////////////////////////////////////////////////////////////////////////Услуги////////////////////////////////////////////////////////////////////
        private void listView5_DoubleClick(object sender, EventArgs e)
        {
            textBox32.Text = listView5.SelectedItems[0].SubItems[0].Text;
            comboBox9.SelectedItem = listView5.SelectedItems[0].SubItems[1].Text;
            textBox22.Text = listView5.SelectedItems[0].SubItems[2].Text;
            textBox23.Text = listView5.SelectedItems[0].SubItems[3].Text;
            textBox30.Text = listView5.SelectedItems[0].SubItems[4].Text;
            textBox31.Text = listView5.SelectedItems[0].SubItems[5].Text;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!sqlQuery.SelectAllFromTraineWhereId(textBox32.Text))
                {
                    int substraction = 1;
                    if (comboBox9.SelectedItem.ToString() == "Абонемент")
                        substraction = 1;
                    else
                        substraction = 2;
                    TraineModel traineModel = new TraineModel
                    {
                        Id = Convert.ToInt32(textBox32.Text),
                        type = textBox22.Text,
                        price = Convert.ToInt32(textBox23.Text),
                        subscription = substraction,
                        count_raine = Convert.ToInt32(textBox30.Text),
                        validity = Convert.ToDecimal(Convert.ToInt32(textBox31.Text) * 86400)
                    };
                    if (sqlQuery.InsertIntoTraine(traineModel))
                    {
                        MessageBox.Show(
                            "Услуга успешно добавлена!",
                            "Успешно!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
                {
                    MessageBox.Show(
                    "Услуга с таким идентификатором уже существует(#)",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int substraction = 1;
                if (comboBox9.SelectedItem.ToString() == "Абонемент")
                    substraction = 1;
                else
                    substraction = 2;
                TraineModel traineModel = new TraineModel
                {
                    Id = Convert.ToInt32(textBox32.Text),
                    type = textBox22.Text,
                    price = Convert.ToInt32(textBox23.Text),
                    subscription = substraction,
                    count_raine = Convert.ToInt32(textBox30.Text),
                    validity = Convert.ToDecimal(Convert.ToInt32(textBox31.Text) * 86400)
                };
                DialogResult result = MessageBox.Show(
                        "Вы уверены, что хотите изменить услугу с идентификатором " + textBox32.Text + "?",
                        "Изменение услуги",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                if(result == DialogResult.Yes)
                    if (sqlQuery.UpdateTraineSetAllWhereUserId(traineModel))
                    {
                        MessageBox.Show(
                            "Услуга успешно изменена!",
                            "Успешно!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<TraineModel> traineModel = sqlQuery.SelectAllFromTraine();
            listView5.Items.Clear();

            for (int i = 0; i < traineModel.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(traineModel[i].Id.ToString());
                if (traineModel[i].subscription == 1)
                    lvi.SubItems.Add("Абонемент");
                else
                    lvi.SubItems.Add("Не абонемент");
                lvi.SubItems.Add(traineModel[i].type);
                lvi.SubItems.Add(traineModel[i].price.ToString());
                lvi.SubItems.Add(traineModel[i].count_raine.ToString());
                lvi.SubItems.Add(Convert.ToInt32(TimeSpan.FromSeconds(Convert.ToDouble(traineModel[i].validity)).TotalDays).ToString());
                listView5.Items.Add(lvi);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (worker.id_role == 1)
            {
                if (sqlQuery.DeleteTraineWhereId(listView5.SelectedItems[0].SubItems[0].Text))
                    if (listView5.SelectedItems.Count != 0)
                    {
                        listView5.SelectedItems[0].Remove();
                        MessageBox.Show(
                            "Услуга удалёна",
                            "Успешно!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
            }
            else
                MessageBox.Show(
                    "У вас нет прав на удаление услуг",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}

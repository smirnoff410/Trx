using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trx.Model;

namespace Trx
{
    public class SqlQuery
    {
        private string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vladislav\source\repos\Trx\Trx\Database.mdf;Integrated Security=True";
        private SqlConnection sqlConnection;

        //SELECT
        public WorkerModel SelectAllFromWorkerWhereWorkerLoginWorkerPassword(string login, string password)
        {
            WorkerModel worker = new WorkerModel();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Worker] Where login = N'" + login + "' AND password = N'" + password + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        worker = new WorkerModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            first_name = dr.GetValue(1).ToString().Trim(),
                            second_name = dr.GetValue(2).ToString().Trim(),
                            last_name = dr.GetValue(3).ToString().Trim(),
                            login = dr.GetValue(4).ToString().Trim(),
                            password = dr.GetValue(5).ToString().Trim(),
                            id_role = Convert.ToInt32(dr.GetValue(6))
                        };
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                        return worker;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserModel> SelectAllFromUser()
        {
            List<UserModel> userModel = new List<UserModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [User]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserModel user = new UserModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            first_name = dr.GetValue(1).ToString().Trim(),
                            second_name = dr.GetValue(2).ToString().Trim(),
                            last_name = dr.GetValue(3).ToString().Trim()
                        };
                        userModel.Add(user);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserVisitsModel> SelectAllFromUserVisits()
        {
            List<UserVisitsModel> userVisitsModel = new List<UserVisitsModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserVisits]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserVisitsModel userVisits = new UserVisitsModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date = Convert.ToDecimal(dr.GetValue(4))
                        };
                        userVisitsModel.Add(userVisits);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userVisitsModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<WorkerTraineModel> SelectAllFromWorkerTraine()
        {
            List<WorkerTraineModel> workerTraineModel = new List<WorkerTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [WorkerTraine]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        WorkerTraineModel workerTraine = new WorkerTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            type = dr.GetValue(1).ToString().Trim()
                        };
                        workerTraineModel.Add(workerTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return workerTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserModel> SelectAllFromUserWhereUserId(string userId)
        {
            List<UserModel> userModel = new List<UserModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [User] WHERE Id = " + userId, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        string phone = "", email = "";
                        int age = 0;
                        var Age = dr.GetValue(4);
                        if (Age.ToString() != "")
                            age = Convert.ToInt32(Age);
                        var Phone = dr.GetValue(5);
                        if (Phone.ToString() != "")
                            phone = Phone.ToString();
                        var Email = dr.GetValue(6);
                        if (Email.ToString() != "")
                            email = Email.ToString();
                        UserModel user = new UserModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            first_name = dr.GetValue(1).ToString().Trim(),
                            second_name = dr.GetValue(2).ToString().Trim(),
                            last_name = dr.GetValue(3).ToString().Trim(),
                            age = age,
                            phone = phone,
                            email = email
                        };
                        userModel.Add(user);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserModel> SelectAllFromUserWhereSecondName(string second_name)
        {
            List<UserModel> userModel = new List<UserModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [User] WHERE second_name = N'" + second_name + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        string phone = "", email = "";
                        int age = 0;
                        var Age = dr.GetValue(4);
                        if (Age.ToString() != "")
                            age = Convert.ToInt32(Age);
                        var Phone = dr.GetValue(5);
                        if (Phone.ToString() != "")
                            phone = Phone.ToString();
                        var Email = dr.GetValue(6);
                        if (Email.ToString() != "")
                            email = Email.ToString();
                        UserModel user = new UserModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            first_name = dr.GetValue(1).ToString().Trim(),
                            second_name = dr.GetValue(2).ToString().Trim(),
                            last_name = dr.GetValue(3).ToString().Trim(),
                            age = age,
                            phone = phone,
                            email = email
                        };
                        userModel.Add(user);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<string> SelectTraineTypeFromUserTraineWhereUserId(string userId)
        {
            List<string> traineType = new List<string>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand1 = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId, sqlConnection);
                using (SqlDataReader dr = sqlCommand1.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        traineType.Add(dr.GetValue(2).ToString().Trim());
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return traineType;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserTraineModel> SelectAllUserTraineWhereUserId(string userId)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId + "", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim())
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserTraineModel> SelectAllFromUserTraineWhereUserIdAndTraineType(string userId, string traine_type)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId + " AND traine_type = N'" + traine_type + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim())
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserTraineModel> SelectAllFromUserTraineWhereUserId(string userId)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                            price = Convert.ToInt32(dr.GetValue(6))
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserTraineModel> SelectAllFromUserTraineWhereUserIdAndCountTraineAndDateFinish(string userId, double date_finish)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId + " AND count_traine > 0 AND date_finish > " + date_finish, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                            price = Convert.ToInt32(dr.GetValue(6))
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserTraineModel> SelectAllFromUserTraineWhereUserIdAndCountTraine(string userId)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId + " AND count_traine > 0", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                            price = Convert.ToInt32(dr.GetValue(6)),
                            sale = Convert.ToInt32(dr.GetValue(7))
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public TraineModel SelectAllFromTraineWhereType(string type)
        {
            TraineModel traineModel = new TraineModel();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Traine] WHERE type = N'" + type + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        traineModel = new TraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            type = dr.GetValue(1).ToString().Trim(),
                            price = Convert.ToInt32(dr.GetValue(2)),
                            subscription = Convert.ToInt32(dr.GetValue(3)),
                            count_raine = Convert.ToInt32(dr.GetValue(4)),
                            validity = Convert.ToDecimal(dr.GetValue(5))
                        };
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return traineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<TraineModel> SelectAllFromTraineWhereSubscription(string subscription)
        {
            List<TraineModel> TraineModel = new List<TraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Traine] WHERE subscription = " + subscription, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        TraineModel Traine = new TraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            type = dr.GetValue(1).ToString().Trim(),
                            price = Convert.ToInt32(dr.GetValue(2)),
                            subscription = Convert.ToInt32(dr.GetValue(3)),
                            count_raine = Convert.ToInt32(dr.GetValue(4)),
                            validity = Convert.ToDecimal(dr.GetValue(5))
                        };
                        TraineModel.Add(Traine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return TraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<TraineModel> SelectAllFromTraine()
        {
            List<TraineModel> traineModel = new List<TraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Traine]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        TraineModel traine = new TraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            type = dr.GetValue(1).ToString().Trim(),
                            price = Convert.ToInt32(dr.GetValue(2)),
                            subscription = Convert.ToInt32(dr.GetValue(3)),
                            count_raine = Convert.ToInt32(dr.GetValue(4)),
                            validity = Convert.ToDecimal(dr.GetValue(5))
                        };
                        traineModel.Add(traine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return traineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public bool SelectAllFromTraineWhereId(string id)
        {
            TraineModel traineModel = new TraineModel();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Traine] WHERE Id = " + id + "", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                        return true;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public List<ScheduleModel> SelectAllFromSchedule()
        {
            List<ScheduleModel> scheduleModel = new List<ScheduleModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Schedule]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        ScheduleModel schedule = new ScheduleModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            worker = dr.GetValue(1).ToString().Trim(),
                            traine = dr.GetValue(2).ToString().Trim(),
                            date_start = Convert.ToDecimal(dr.GetValue(3).ToString().Trim()),
                            date_end = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            count_user = Convert.ToInt32(dr.GetValue(5))
                        };
                        scheduleModel.Add(schedule);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return scheduleModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<ScheduleModel> SelectCountTraineFromScheduleWhereWorkerAndTraine(string worker, string traine)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                List<ScheduleModel> scheduleModel = new List<ScheduleModel>();
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Schedule] WHERE worker = N'" + worker + "' AND traine = N'" + traine + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        ScheduleModel schedule = new ScheduleModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            worker = dr.GetValue(1).ToString().Trim(),
                            traine = dr.GetValue(2).ToString().Trim(),
                            date_start = Convert.ToDecimal(dr.GetValue(3).ToString().Trim()),
                            date_end = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            count_user = Convert.ToInt32(dr.GetValue(5))
                        };
                        scheduleModel.Add(schedule);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return scheduleModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<WorkerModel> SelectAllFromWorker()
        {
            List<WorkerModel> workerModel = new List<WorkerModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Worker]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        WorkerModel worker = new WorkerModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            first_name = dr.GetValue(1).ToString().Trim(),
                            second_name = dr.GetValue(2).ToString().Trim(),
                            last_name = dr.GetValue(3).ToString().Trim(),
                            login = dr.GetValue(4).ToString().Trim(),
                            password = dr.GetValue(5).ToString().Trim(),
                            id_role = Convert.ToInt32(dr.GetValue(6).ToString().Trim())
                        };
                        workerModel.Add(worker);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return workerModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public WorkerModel SelectAllFromWorkerWhereWorkerId(string Id)
        {
            WorkerModel workerModel = new WorkerModel();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Worker] WHERE Id = " + Id, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        decimal date_of_birth = 0, date_start = 0;
                        string phone = "", education = "", special_course = "";
                        var Date_of_birth = dr.GetValue(7);
                        if (Date_of_birth.ToString() != "")
                            date_of_birth = Convert.ToDecimal(Date_of_birth);
                        var Phone = dr.GetValue(8);
                        if (Phone.ToString() != "")
                            phone = Phone.ToString();
                        var Education = dr.GetValue(9);
                        if (Education.ToString() != "")
                            education = Education.ToString();
                        var Special_course = dr.GetValue(10);
                        if (Special_course.ToString() != "")
                            special_course = Special_course.ToString();
                        var Date_start = dr.GetValue(11);
                        if (Date_start.ToString() != "")
                            date_start = Convert.ToDecimal(Date_start);
                        workerModel = new WorkerModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            first_name = dr.GetValue(1).ToString().Trim(),
                            second_name = dr.GetValue(2).ToString().Trim(),
                            last_name = dr.GetValue(3).ToString().Trim(),
                            login = dr.GetValue(4).ToString().Trim(),
                            password = dr.GetValue(5).ToString().Trim(),
                            id_role = Convert.ToInt32(dr.GetValue(6).ToString().Trim()),
                            date_of_birth = date_of_birth,
                            phone = phone,
                            education = education,
                            special_course = special_course,
                            date_start = date_start
                        };
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return workerModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public string SelectWorkerRoleFromRolesWhereWorkerId(string id_role)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                string role = "";
                SqlCommand sqlCommand = new SqlCommand("SELECT type FROM [Roles] WHERE Id = " + id_role, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        role = dr.GetValue(0).ToString().Trim();
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return role;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public int SelectMaxIdFromUserTraine()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                int max = 0;
                SqlCommand sqlCommand = new SqlCommand("SELECT MAX(Id) FROM [UserTraine]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        var temp = dr.GetValue(0);
                        if (temp.ToString() != "")
                            max = Convert.ToInt32(dr.GetValue(0));
                        else
                            max = 0;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return max;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public int SelectMaxIdFromUserVisits()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                int max = 0;
                SqlCommand sqlCommand = new SqlCommand("SELECT MAX(Id) FROM [UserVisits]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        var temp = dr.GetValue(0);
                        if (temp.ToString() != "")
                            max = Convert.ToInt32(dr.GetValue(0));
                        else
                            max = 0;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return max;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public int SelectMaxIdFromWorker()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                int max = 0;
                SqlCommand sqlCommand = new SqlCommand("SELECT MAX(Id) FROM [Worker]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        var temp = dr.GetValue(0);
                        if (temp.ToString() != "")
                            max = Convert.ToInt32(dr.GetValue(0));
                        else
                            max = 0;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return max;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public int SelectMaxIdFromSchedule()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                int max = 0;
                SqlCommand sqlCommand = new SqlCommand("SELECT MAX(Id) FROM [Schedule]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        var temp = dr.GetValue(0);
                        if (temp.ToString() != "")
                            max = Convert.ToInt32(dr.GetValue(0));
                        else
                            max = 0;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return max;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public int SelectCountTraineFromScheduleWhereWorkerSecondNameAndDateEnd(string second_name, decimal date_end)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                int count = 0;
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Schedule] WHERE worker = N'" + second_name + "' AND date_end < " + date_end, sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        count++;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return count;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public int SelectCountTraineTypeFromScheduleWhereTraineType(string traine)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                int count = 0;
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Schedule] WHERE traine = N'" + traine + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        count++;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return count;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return 0;
            }
        }

        public bool SelectAllFromTraineWhereUserIdAndTraineType(string userId, string traine_type)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM UserTraine WHERE id_user = " + userId + " AND traine_type = N'" + traine_type + "' AND count_traine > 0", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                        return true;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public int SelectAllFromTraineWhereUserIdForSale(string userId)
        {
            sqlConnection = new SqlConnection(stringConnection);
            int count = 0;
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM UserTraine WHERE id_user = " + userId + " AND traine_type = N'На 8 занятий' OR traine_type = N'На 12 занятий' OR traine_type = N'Безлимитный'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        count++;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return count;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return 0;
            }
        }

        public bool SelectAllFromWorkerWhereWorkerLogin(string login)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Worker WHERE login = N'" + login + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                        return true;
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public List<UserTraineModel> SelectAllFromUserTraineWhereTraineType(string traine)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE traine_type = N'" + traine + "'", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                            price = Convert.ToInt32(dr.GetValue(6)),
                            sale = Convert.ToInt32(dr.GetValue(7))
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        public List<UserTraineModel> SelectAllFromUserTraine()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine]", sqlConnection);
                using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        UserTraineModel userTraine = new UserTraineModel
                        {
                            Id = Convert.ToInt32(dr.GetValue(0)),
                            id_user = Convert.ToInt32(dr.GetValue(1)),
                            traine_type = dr.GetValue(2).ToString().Trim(),
                            count_traine = Convert.ToInt32(dr.GetValue(3)),
                            date_start = Convert.ToDecimal(dr.GetValue(4).ToString().Trim()),
                            date_finish = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                            price = Convert.ToInt32(dr.GetValue(6)),
                            sale = Convert.ToInt32(dr.GetValue(7))
                        };
                        userTraineModel.Add(userTraine);
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
                return userTraineModel;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return null;
            }
        }

        //UPDATE

        public bool UpdateUserSetAllWhereUserId(UserModel user)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE [User] SET first_name = N'" + user.first_name + "', second_name = N'" + user.second_name + "', last_name = N'" + user.last_name + "', age = " + user.age + ", phone = N'" + user.phone + "', email = N'" + user.email + "' WHERE Id = " + user.Id + "", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public bool UpdateTraineSetAllWhereUserId(TraineModel traine)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE [Traine] SET type = N'" + traine.type + "', price = " + traine.price + ", subscription = " + traine.subscription + ", count_traine = " + traine.count_raine + ", validity = " + traine.validity + " WHERE Id = " + traine.Id + "", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public bool UpdateUserTraineSetCountTraineWhereUserTraineId(string Id)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE [UserTraine] SET count_traine = 0 WHERE Id = " + Id, sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public bool UpdateWorkerSetAllWhereWorkerId(WorkerModel worker)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE [Worker] SET first_name = N'" + worker.first_name + "', second_name = N'" + worker.second_name + "', last_name = N'" + worker.last_name + "', login = N'" + worker.login + "', id_role = " + worker.id_role + ", date_of_birth = " + worker.date_of_birth + ", phone = N'" + worker.phone + "', education = N'" + worker.education + "', special_course = N'" + worker.special_course + "', work_start = " + worker.date_start + " WHERE Id = " + worker.Id + "", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public int UpdateUserTraineSetCountTraineWhereUserIdAndTraineType(string userId, string traine_type, int dec, string count_traine, double date_start, double date_finish, string price)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                dec--;
                SqlCommand sqlCommand = new SqlCommand("UPDATE [UserTraine] SET count_traine = " + dec + " WHERE id_user = " + userId + " AND traine_type = N'" + traine_type + "' AND count_traine = " + count_traine + " AND date_start = " + date_start + " AND date_finish = " + date_finish + " AND price = " + price + "", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return dec;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public bool UpdateUserTraineSetDateFinishWhereUserIdAndTraineType(string userId, string traine_type, double datefinish, string count_traine, double date_start, double date_finish, string price)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("UPDATE [UserTraine] SET date_finish = " + datefinish + " WHERE id_user = " + userId + " AND traine_type = N'" + traine_type + "' AND count_traine = " + count_traine + " AND date_start = " + date_start + " AND date_finish = " + date_finish + " AND price = " + price + "", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        //INSERT

        public int InsertIntoUser(UserModel user)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [User] VALUES(" + user.Id + ", N'" + user.first_name + "', N'" + user.second_name + "', N'" + user.last_name + "', " + user.age + ", N'" + user.phone + "', N'" + user.email + "')", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return rowCount;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public bool InsertIntoUserVisits(UserVisitsModel userVisits)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [UserVisits] VALUES(" + userVisits.Id + ", " + userVisits.id_user + ", N'" + userVisits.traine_type + "', " + userVisits.count_traine + ", " + userVisits.date + ")", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public int InsertIntoUserTraine(UserTraineModel userTraineModel)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [UserTraine] VALUES(" + userTraineModel.Id + ", " + userTraineModel.id_user + ", N'" + userTraineModel.traine_type + "', " + userTraineModel.count_traine + ", " + userTraineModel.date_start + ", " + userTraineModel.date_finish + ", " + userTraineModel.price + ", " + userTraineModel.sale + ")", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return rowCount;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return -1;
            }
        }

        public bool InsertIntoWorker(WorkerModel worker)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Worker] VALUES(" + worker.Id + ", N'" + worker.first_name + "', N'" + worker.second_name + "', N'" + worker.last_name + "', N'" + worker.login + "', N'" + worker.password + "', " + worker.id_role + ", " + worker.date_of_birth + ", N'" + worker.phone + "', N'" + worker.education + "', N'" + worker.special_course + "', " + worker.date_start + ")", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public bool InsertIntoSchedule(ScheduleModel schedule)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Schedule] VALUES(" + schedule.Id + ", N'" + schedule.worker + "', N'" + schedule.traine + "', " + schedule.date_start + ", " + schedule.date_end + ", " + schedule.count_user + ")", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public bool InsertIntoTraine(TraineModel traine)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Traine] VALUES(" + traine.Id + ", N'" + traine.type + "', " + traine.price + ", " + traine.subscription + ", " + traine.count_raine + ", " + traine.validity + ")", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        //DELETE

        public bool DeleteWorkerWhereId(string id)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("DELETE [Worker] WHERE Id = " + id, sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }

        public bool DeleteTraineWhereId(string id)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("DELETE [Traine] WHERE Id = " + id, sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorMessage1(ex.Message);
                return false;
            }
        }
    }
}

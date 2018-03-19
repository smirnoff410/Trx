using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Model;

namespace Trx
{
    public class SqlQuery
    {
        private string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vladislav\source\repos\Trx\Trx\Database.mdf;Integrated Security=True";
        private SqlConnection sqlConnection;

        //SELECT

        public List<UserModel> SelectAllFromUser()
        {
            List<UserModel> userModel = new List<UserModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

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

        public List<UserModel> SelectAllFromUserWhereUserId(string userId)
        {
            List<UserModel> userModel = new List<UserModel>();
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

        public List<string> SelectTraineTypeFromUserTraineWhereUserId(string userId)
        {
            List<string> traineType = new List<string>();
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
                    traineType.Add(dr.GetValue(2).ToString().Trim());
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return traineType;
        }

        public List<UserTraineModel> SelectAllUserTraineWhereUserId(string userId)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

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
                        worker_name = dr.GetValue(3).ToString().Trim(),
                        count_traine = Convert.ToInt32(dr.GetValue(4)),
                        date_start = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                        date_finish = Convert.ToDecimal(dr.GetValue(6).ToString().Trim())
                    };
                    userTraineModel.Add(userTraine);
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return userTraineModel;
        }

        public List<UserTraineModel> SelectAllFromUserTraineWhereUserIdAndTraineType(string userId, string traine_type)
        {
            List<UserTraineModel> userTraineModel = new List<UserTraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [UserTraine] WHERE id_user = " + userId + " AND traine_type = N'" + traine_type + "' AND (count_traine > 0)", sqlConnection);
            using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    UserTraineModel userTraine = new UserTraineModel
                    {
                        Id = Convert.ToInt32(dr.GetValue(0)),
                        id_user = Convert.ToInt32(dr.GetValue(1)),
                        traine_type = dr.GetValue(2).ToString().Trim(),
                        worker_name = dr.GetValue(3).ToString().Trim(),
                        count_traine = Convert.ToInt32(dr.GetValue(4)),
                        date_start = Convert.ToDecimal(dr.GetValue(5).ToString().Trim()),
                        date_finish = Convert.ToDecimal(dr.GetValue(6).ToString().Trim())
                    };
                    userTraineModel.Add(userTraine);
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return userTraineModel;
        }

        public List<TraineModel> SelectAllFromTraine()
        {
            List<TraineModel> traineModel = new List<TraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Traine]", sqlConnection);
            using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    TraineModel traine = new TraineModel
                    {
                        Id = Convert.ToInt32(dr.GetValue(0)),
                        type = dr.GetValue(1).ToString().Trim(),
                        price = Convert.ToInt32(dr.GetValue(2).ToString().Trim())
                    };
                    traineModel.Add(traine);
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return traineModel;
        }

        public List<WorkerModel> SelectAllFromWorker()
        {
            List<WorkerModel> workerModel = new List<WorkerModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

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

        public WorkerModel SelectAllFromWorkerWhereWorkerId(string Id)
        {
            WorkerModel workerModel = new WorkerModel();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Worker] WHERE Id = " + Id, sqlConnection);
            using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    workerModel = new WorkerModel
                    {
                        Id = Convert.ToInt32(dr.GetValue(0)),
                        first_name = dr.GetValue(1).ToString().Trim(),
                        second_name = dr.GetValue(2).ToString().Trim(),
                        last_name = dr.GetValue(3).ToString().Trim(),
                        login = dr.GetValue(4).ToString().Trim(),
                        password = dr.GetValue(5).ToString().Trim(),
                        id_role = Convert.ToInt32(dr.GetValue(6).ToString().Trim())
                    };
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return workerModel;
        }

        public string SelectWorkerRoleFromRolesWhereWorkerId(string id_role)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
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

        public int SelectMaxIdFromUserTraine()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
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

        public int SelectMaxIdFromWorker()
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
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

        public List<TraineModel> SelectAllFromTraineRightJoinOnTraineTypeCountTraineUserId(string userId)
        {
            List<TraineModel> traineModel = new List<TraineModel>();
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("SELECT Traine.Id, Traine.type, Traine.type FROM Traine RIGHT JOIN UserTraine ON Traine.type = UserTraine.traine_type AND UserTraine.count_traine > 0 AND UserTraine.id_user = " + userId + "", sqlConnection);
            using (SqlDataReader dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    TraineModel traine = new TraineModel
                    {
                        Id = Convert.ToInt32(dr.GetValue(0)),
                        type = dr.GetValue(1).ToString().Trim(),
                        price = Convert.ToInt32(dr.GetValue(2))
                    };
                    traineModel.Add(traine);
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return traineModel;
        }

        //UPDATE

        public bool UpdateUserSetAllWhereUserId(UserModel user)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
            SqlCommand sqlCommand = new SqlCommand("UPDATE [User] SET first_name = N'" + user.first_name + "', second_name = N'" + user.second_name + "', last_name = N'" + user.last_name + "' WHERE Id = " + user.Id + "", sqlConnection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return true;
        }

        public bool UpdateWorkerSetAllWhereWorkerId(WorkerModel worker)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
            SqlCommand sqlCommand = new SqlCommand("UPDATE [Worker] SET first_name = N'" + worker.first_name + "', second_name = N'" + worker.second_name + "', last_name = N'" + worker.last_name + "', login = N'" + worker.login + "', id_role = " + worker.id_role + " WHERE Id = " + worker.Id + "", sqlConnection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return true;
        }

        public int UpdateUserTraineSetCountTraineWhereUserIdAndTraineType(string userId, string traine_type, int dec)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
            dec--;
            SqlCommand sqlCommand = new SqlCommand("UPDATE [UserTraine] SET count_traine = " + dec + " WHERE id_user = " + userId + " AND traine_type = N'" + traine_type + "'", sqlConnection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return dec;
        }

        //INSERT

        public int InsertIntoUser(UserModel user)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO [User] VALUES(" + user.Id + ", N'" + user.first_name + "', N'" + user.second_name + "', N'" + user.last_name + "')", sqlConnection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return rowCount;
        }

        public int InsertIntoUserTraine(UserTraineModel userTraineModel)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }

            SqlCommand sqlCommand = new SqlCommand("INSERT INTO [UserTraine] VALUES(" + userTraineModel.Id + ", " + userTraineModel.id_user + ", N'" + userTraineModel.traine_type + "',N' " + userTraineModel.worker_name + "', " + userTraineModel.count_traine + ", " + userTraineModel.date_start + ", " + userTraineModel.date_finish + ")", sqlConnection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return rowCount;
        }

        public bool InsertIntoWorker(WorkerModel worker)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
            try
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Worker] VALUES(" + worker.Id + ", N'" + worker.first_name + "', N'" + worker.second_name + "', N'" + worker.last_name + "', N'" + worker.login + "', N'" + worker.password + "', " + worker.id_role + ")", sqlConnection);
                int rowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        //DELETE

        public bool DeleteWorkerWhereId(string id)
        {
            sqlConnection = new SqlConnection(stringConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {

            }
            SqlCommand sqlCommand = new SqlCommand("DELETE [Worker] WHERE Id = " + id, sqlConnection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return true;
        }
    }
}

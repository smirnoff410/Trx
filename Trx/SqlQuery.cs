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
                        id_worker = Convert.ToInt32(dr.GetValue(3)),
                        count_traine = Convert.ToInt32(dr.GetValue(4)),
                        date_start = dr.GetValue(5).ToString().Trim()
                    };
                    userTraineModel.Add(userTraine);
                }
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
            return userTraineModel;
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
    }
}

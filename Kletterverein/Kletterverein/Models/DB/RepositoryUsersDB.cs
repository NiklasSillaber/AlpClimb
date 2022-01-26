using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Models.DB
{
    public class RepositoryUsersDB : IRepositoryUsers
    {
        private string _connectionString = "Server=localhost;database=alpClimb;user=root;password=";
        private DbConnection _conn;

        public void Connect()
        {
            if (this._conn == null)
            {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open)
            {
                this._conn.Open();
            }
        }

        public void Disconnect()
        {
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open))
            {
                this._conn.Close();
            }
        }

        public bool Delete(int userId)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdDelete = this._conn.CreateCommand();
                cmdDelete.CommandText = "delete from users where user_id = @userid";

                DbParameter paramUI = cmdDelete.CreateParameter();
                paramUI.ParameterName = "userid";
                paramUI.DbType = DbType.Int32;
                paramUI.Value = userId;

                cmdDelete.Parameters.Add(paramUI);

                return cmdDelete.ExecuteNonQuery() == 1;

            }

            return false;
        }

        public User GetUser(int userId)
        {

            //KEINE SCHLEIFE VERWENDEN


            //if (this._conn?.State == ConnectionState.Open)
            //{
            //    DbCommand cmdSelect = this._conn.CreateCommand();
            //    cmdSelect.CommandText = "select * from users where user_id = @userid";

            //    DbParameter paramUI = cmdSelect.CreateParameter();
            //    paramUI.ParameterName = "userid";
            //    paramUI.DbType = DbType.Int32;
            //    paramUI.Value = userId;

            //    User = new User();

            //    return cmdSelect.ExecuteReader;

            //}

            return null;
        }

        public bool Insert(User user)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();
                cmdInsert.CommandText = "insert into users values(null, @firstname, @lastname, sha2(@pwd,512), @birthdate, @mail, @gender);";

                DbParameter paramFN = cmdInsert.CreateParameter();
                paramFN.ParameterName = "firstname";
                paramFN.DbType = DbType.String;
                paramFN.Value = user.Firstname;

                DbParameter paramLN = cmdInsert.CreateParameter();
                paramLN.ParameterName = "lastname";
                paramLN.DbType = DbType.String;
                paramLN.Value = user.Lastname;

                DbParameter paramPWD = cmdInsert.CreateParameter();
                paramPWD.ParameterName = "pwd";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = user.Password;

                DbParameter paramBD = cmdInsert.CreateParameter();
                paramBD.ParameterName = "birthdate";
                paramBD.DbType = DbType.Date;
                paramBD.Value = user.Birthdate;

                DbParameter paramEM = cmdInsert.CreateParameter();
                paramEM.ParameterName = "mail";
                paramEM.DbType = DbType.String;
                paramEM.Value = user.EMail;

                DbParameter paramG = cmdInsert.CreateParameter();
                paramG.ParameterName = "gender";
                paramG.DbType = DbType.Int32;
                paramG.Value = user.Gender;

                cmdInsert.Parameters.Add(paramFN);
                cmdInsert.Parameters.Add(paramLN);
                cmdInsert.Parameters.Add(paramPWD);
                cmdInsert.Parameters.Add(paramBD);
                cmdInsert.Parameters.Add(paramEM);
                cmdInsert.Parameters.Add(paramG);

                return cmdInsert.ExecuteNonQuery() == 1;

            }

            return false;
        }

        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Update(int userId, User newUserData)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdUpdate = this._conn.CreateCommand();
                cmdUpdate.CommandText = "update users set firstname = @firstname, lastname = @lastname, password = @password," +
                   "birthdate = @birthdate, email = @email, gender = @gender where user_id = @userid;";

                DbParameter paramFN = cmdUpdate.CreateParameter();
                paramFN.ParameterName = "firstname";
                paramFN.DbType = DbType.String;
                paramFN.Value = newUserData.Firstname;

                DbParameter paramLN = cmdUpdate.CreateParameter();
                paramLN.ParameterName = "lastname";
                paramLN.DbType = DbType.String;
                paramLN.Value = newUserData.Lastname;

                DbParameter paramPWD = cmdUpdate.CreateParameter();
                paramPWD.ParameterName = "pwd";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = newUserData.Password;

                DbParameter paramBD = cmdUpdate.CreateParameter();
                paramBD.ParameterName = "birthdate";
                paramBD.DbType = DbType.Date;
                paramBD.Value = newUserData.Birthdate;

                DbParameter paramEM = cmdUpdate.CreateParameter();
                paramEM.ParameterName = "mail";
                paramEM.DbType = DbType.String;
                paramEM.Value = newUserData.EMail;

                DbParameter paramG = cmdUpdate.CreateParameter();
                paramG.ParameterName = "gender";
                paramG.DbType = DbType.Int32;
                paramG.Value = newUserData.Gender;

                DbParameter paramUID = cmdUpdate.CreateParameter();
                paramG.ParameterName = "userid";
                paramG.DbType = DbType.Int32;
                paramG.Value = newUserData.UserId;

                cmdUpdate.Parameters.Add(paramFN);
                cmdUpdate.Parameters.Add(paramLN);
                cmdUpdate.Parameters.Add(paramPWD);
                cmdUpdate.Parameters.Add(paramBD);
                cmdUpdate.Parameters.Add(paramEM);
                cmdUpdate.Parameters.Add(paramG);
                cmdUpdate.Parameters.Add(paramUID);

                return cmdUpdate.ExecuteNonQuery() == 1;
            }
            return false;
        }
    }
}

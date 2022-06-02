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
        //Verbindung zu gehosteter DB von remoteMysql.com
        private string _connectionString = "Server=remotemysql.com;database=KJ8WTvDY9Y;user=KJ8WTvDY9Y;password=BVdseHMrTJ";
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
            User a;
            if (this._conn?.State == ConnectionState.Open)
            {
                //leeres Command erzeugen
                DbCommand cmdOneUser = this._conn.CreateCommand();
                //SQL-Befehl angeben
                cmdOneUser.CommandText = "select * from users where user_id = @userid";


                DbParameter paramUI = cmdOneUser.CreateParameter();
                paramUI.ParameterName = "userid";
                paramUI.DbType = DbType.Int32;
                paramUI.Value = userId;

                cmdOneUser.Parameters.Add(paramUI);

                //mit dem DbDataReader kann zeilenweise durch das Ergebnis gegangen werden
                using (DbDataReader reader = cmdOneUser.ExecuteReader())
                {
                    //Read() ... eine Zeile (Datensatz) lesen
                    if (reader.Read())
                    {

                        reader.Read();
                        //und dann der Liste hinzufügen
                        a = new User()
                        {
                            //UserId ... Name des Properties der Klasse User
                            //user_id ... Name des Feldes in der Db-Klasse
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Password = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            EMail = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"])
                        }; //DbNull.Value
                        return a;
                    }
                } //hier wird der DbDataReader automatisch wieder freigegeben
                  //hier wird die Methode Dispose() von DbDataReader aufgerufen
                  //kurze Schreibweise für try ... finally
            }

            return null;
        }

        public User GetUserWithEmail(String email)
        {
            User a;
            if (this._conn?.State == ConnectionState.Open)
            {
                //leeres Command erzeugen
                DbCommand cmdOneUser = this._conn.CreateCommand();
                //SQL-Befehl angeben
                cmdOneUser.CommandText = "select * from users where email = @email";


                DbParameter paramEM = cmdOneUser.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                cmdOneUser.Parameters.Add(paramEM);

                //mit dem DbDataReader kann zeilenweise durch das Ergebnis gegangen werden
                using (DbDataReader reader = cmdOneUser.ExecuteReader())
                {
                    //Read() ... eine Zeile (Datensatz) lesen
                    if (reader.Read())
                    {

                        reader.Read();
                        //und dann der Liste hinzufügen
                        a = new User()
                        {
                            //UserId ... Name des Properties der Klasse User
                            //user_id ... Name des Feldes in der Db-Klasse
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Password = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            EMail = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"])
                        }; //DbNull.Value
                        return a;
                    }
                } //hier wird der DbDataReader automatisch wieder freigegeben
                  //hier wird die Methode Dispose() von DbDataReader aufgerufen
                  //kurze Schreibweise für try ... finally
            }

            return null;
        }

        public int GetUserIdWithEmail(String email)
        {
            User a;
            if (this._conn?.State == ConnectionState.Open)
            {
                //leeres Command erzeugen
                DbCommand cmdOneUser = this._conn.CreateCommand();
                //SQL-Befehl angeben
                cmdOneUser.CommandText = "select * from users where email = @email";


                DbParameter paramEM = cmdOneUser.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                cmdOneUser.Parameters.Add(paramEM);

                //mit dem DbDataReader kann zeilenweise durch das Ergebnis gegangen werden
                using (DbDataReader reader = cmdOneUser.ExecuteReader())
                {
                    //Read() ... eine Zeile (Datensatz) lesen
                    if (reader.Read())
                    {

                        reader.Read();
                        //und dann der Liste hinzufügen
                        a = new User()
                        {
                            //UserId ... Name des Properties der Klasse User
                            //user_id ... Name des Feldes in der Db-Klasse
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Password = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            EMail = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"])
                        }; //DbNull.Value
                        return a.UserId;
                    }
                } //hier wird der DbDataReader automatisch wieder freigegeben
                  //hier wird die Methode Dispose() von DbDataReader aufgerufen
                  //kurze Schreibweise für try ... finally
            }

            return 0;
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

        public bool Login(string email, string password)
        {

            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdLogin = this._conn.CreateCommand();
                cmdLogin.CommandText = "select * from users where email = @email and password = sha2(@password,512);";

                DbParameter paramEM = cmdLogin.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                DbParameter paramPWD = cmdLogin.CreateParameter();
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = password;

                cmdLogin.Parameters.Add(paramEM);
                cmdLogin.Parameters.Add(paramPWD);

                using (DbDataReader reader = cmdLogin.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                } 
            }
            return false;
        }

        //Verschlüsselungsstring wird verändert wenn man nicht das gleiche Passwort nimmt
        public bool Update(User newUserData)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdUpdate = this._conn.CreateCommand();
                cmdUpdate.CommandText = "update users set firstname = @firstname, lastname = @lastname," +
                   "birthdate = @birthdate, email = @email, gender = @gender where user_id = @userid;";

                DbParameter paramFN = cmdUpdate.CreateParameter();
                paramFN.ParameterName = "firstname";
                paramFN.DbType = DbType.String;
                paramFN.Value = newUserData.Firstname;

                DbParameter paramLN = cmdUpdate.CreateParameter();
                paramLN.ParameterName = "lastname";
                paramLN.DbType = DbType.String;
                paramLN.Value = newUserData.Lastname;

                DbParameter paramBD = cmdUpdate.CreateParameter();
                paramBD.ParameterName = "birthdate";
                paramBD.DbType = DbType.Date;
                paramBD.Value = newUserData.Birthdate;

                DbParameter paramEM = cmdUpdate.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = newUserData.EMail;

                DbParameter paramG = cmdUpdate.CreateParameter();
                paramG.ParameterName = "gender";
                paramG.DbType = DbType.Int32;
                paramG.Value = newUserData.Gender;

                DbParameter paramUID = cmdUpdate.CreateParameter();
                paramUID.ParameterName = "userid";
                paramUID.DbType = DbType.Int32;
                paramUID.Value = newUserData.UserId;

                cmdUpdate.Parameters.Add(paramFN);
                cmdUpdate.Parameters.Add(paramLN);
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

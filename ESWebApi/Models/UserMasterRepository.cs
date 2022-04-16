using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ESWebApi.Models
{
    public class UserMasterRepository : IDisposable
    {
        public User ValidateUser(string username, string password)
        {
            User user = new User();
            var userdata = String.Empty;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    string sql = "select * from user_master where name ='" + username + "' and password = '" + password + "'";
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["NpgsqlConnectionString"].ToString();
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            user = new User()
                            {
                                userid = Convert.ToInt32(reader["userid"]),
                                username = Convert.ToString(reader["name"]),
                                usertypeid = Convert.ToInt32(reader["usertypeid"]),
                                emailid = Convert.ToString(reader["emailid"]),
                                mobileno = Convert.ToString(reader["mobileno"]),
                                adharcardno = Convert.ToString(reader["adharcardno"]),
                                isactive = Convert.ToBoolean(reader["isactive"])
                            };                           
                        }
                    }
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception ex) {
                user = new User()
                {
                    emailid = ex.Message
                };
            }
            return user;
        }


        public void Dispose()
        {
            //context.Dispose();
        }
    }
}
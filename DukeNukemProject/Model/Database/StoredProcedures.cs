using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Data;

namespace DukeNukemProject.Model.Database
{
    public class StoredProcedures
    {
        private string connectionString = "Server=ealdb1.eal.local;Database=ejl70_db;User Id=ejl70_usr;Password=Baz1nga70;";
        private SqlConnection connection;
        internal List<string> Result{ get; set; }
        internal int Count { get; set; }
        internal Dictionary<int, List<string>> DicResult { get; set; }
        internal void NonQuerySP(string procedureName, Dictionary<string, string> targetParams)
        {
            using (connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(procedureName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var param in targetParams)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("An error occurs when try to connect to the server");
                }
            }
        }

        internal bool QuerySP(string procedureName, Dictionary<string, string> targetParams = null)
        {
            Result = new List<string>();
            bool listIsNotEmpty = false;
            using (connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(procedureName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(targetParams != null)
                    {
                        foreach (var param in targetParams)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        int columns = reader.FieldCount;
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Result.Add(reader[i].ToString());
                            }
                        }
                    }
                    if(Result.Count > 0)
                    {
                        listIsNotEmpty = true;
                    }
                }
                catch
                {
                    MessageBox.Show("An error occurs when try to connect to the server");
                }
                
            }
            return listIsNotEmpty;
        }

        public bool MultiRowQuerySP(string procedureName, Dictionary<string, string> targetParams = null)
        {
            DicResult = new Dictionary<int, List<string>>();
            List<string> row;
            bool listIsNotEmpty = false;
            using (connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(procedureName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(targetParams != null)
                    {
                        foreach (var param in targetParams)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    Count = 0;
                    if (reader.HasRows)
                    {
                        int columns = reader.FieldCount;
                        while (reader.Read())
                        {
                            row = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader[i].ToString());
                            }
                            DicResult.Add(Count, row);
                            Count++;
                            if (row.Count > 0)
                            {
                                listIsNotEmpty = true;
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("An error occurs when try to connect to the server");
                }
            }
            return listIsNotEmpty;
        }
    }
}

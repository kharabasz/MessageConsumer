using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Database
{
    public class DataRepository : IRepository
    {
        private string ConnectionString { get; set; }

        public DataRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DataRepository()
        {

        }

        public List<T> GetAll<T>(string sqlCommand, Dictionary<string, Tuple<System.Data.DbType, int?, object>> parameters) where T : new()
        {
            var results = new List<T>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sqlCommand;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var key in parameters.Keys)
                        {
                            cmd.Parameters.Add(new SqlParameter(key, parameters[key].Item1));
                            cmd.Parameters[key].Value = parameters[key].Item3;
                            if (parameters[key].Item2.HasValue)
                            {
                                cmd.Parameters[key].Size = parameters[key].Item2.Value;
                            }
                        }
                    }

                    var sqlResults = cmd.ExecuteReader();
                    while (sqlResults.Read())
                    {
                        T obj = new T();
                        var properties = obj.GetType().GetProperties().ToList();

                        for (int i = 0; i < sqlResults.FieldCount; i++)
                        {
                            string name = sqlResults.GetName(i);
                            if (properties.Any(p => p.Name == name))
                            {
                                var prop = properties.First(p => p.Name == name);
                                var value = sqlResults.GetValue(i);
                                prop.SetValue(obj, value);
                            }
                        }

                        results.Add(obj);
                    }
                }
            }

            return results;
        }


        public void Insert(string sqlCommand, Dictionary<string, Tuple<System.Data.DbType, int?, object>> parameters)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sqlCommand;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var key in parameters.Keys)
                        {
                            cmd.Parameters.Add(new SqlParameter(key, parameters[key].Item1));
                            cmd.Parameters[key].Value = parameters[key].Item3;
                            if (parameters[key].Item2.HasValue)
                            {
                                cmd.Parameters[key].Size = parameters[key].Item2.Value;
                            }
                        }
                    }

                    var sqlResults = cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

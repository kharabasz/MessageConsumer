using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Database
{
    public interface IRepository
    {
        List<T> GetAll<T>(string sqlCommand, Dictionary<string, Tuple<System.Data.DbType, int?, object>> parameters) where T : new();

        void Insert(string sqlCommand, Dictionary<string, Tuple<System.Data.DbType, int?, object>> parameters);
    }
}

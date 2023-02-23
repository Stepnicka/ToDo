using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.DbAccessLibrary.Db
{
    internal abstract class Repository
    {
        public Repository(Func<IDbConnection> GetConnection, Func<IDbTransaction> GetTransaction)
        {
            this.getConnection = GetConnection;
            this.getTransaction = GetTransaction;
        }

        protected Func<IDbConnection> getConnection;
        protected Func<IDbTransaction> getTransaction;

        protected async Task<List<T>> LoadData<T, U>(string sql, CommandType commandType, U parameters)
        {
            var rows = await getConnection().QueryAsync<T>(sql, parameters, commandType: commandType, transaction: getTransaction());

            return rows.ToList();
        }

        protected async Task<List<T>> LoadData<T, U>(string sql, CommandType commandType, U parameters, Type[] types, Func<object[], T> map, string splitOn)
        {
            var rows = await getConnection().QueryAsync(sql, types, map, parameters, splitOn: splitOn, commandType: commandType, transaction: getTransaction());

            return rows.ToList();
        }

        protected async Task<int> SaveData<U>(string sql, CommandType commandType, U parameters)
        {
            return await getConnection().ExecuteAsync(sql, parameters, commandType: commandType, transaction: getTransaction());
        }
    }
}

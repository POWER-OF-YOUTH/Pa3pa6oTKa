using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Databases
{
    public class MySQLDataBase<T> where T : MySQLDataBase<T>
    {
        private MySqlConnection CurrentConnection;

        public event Action<T> ReleaseEvent;

        public MySQLDataBase(string connectionString)
        {
            CurrentConnection = new MySqlConnection(connectionString);
            CurrentConnection.StateChange += this.CurrentConnection_StateChange;
            CurrentConnection.Open();
        }

        private void CurrentConnection_StateChange(object sender, StateChangeEventArgs e)
        {

        }

        #region Executors
        public T1 ExecuteReader<T1>(MySqlCommand command, Func<MySqlDataReader, T1> func)
        {
            command.Connection = CurrentConnection;
            var reader = command.ExecuteReader();
            var result = func(reader);
            reader.Close();
            return result;
        }

        public void ExecuteReader(MySqlCommand command, Action<MySqlDataReader> func)
        {
            command.Connection = CurrentConnection;
            var reader = command.ExecuteReader();
            func(reader);
            reader.Close();
        }

        public async void ExecuteReaderAsync(MySqlCommand command, Action<DbDataReader> func)
        {
            command.Connection = CurrentConnection;
            var reader = await command.ExecuteReaderAsync();
            func(reader);
            reader.Close();
        }

        public int ExecuteNonQuery(MySqlCommand command)
        {
            command.Connection = CurrentConnection;
            int result = command.ExecuteNonQuery();
            return result;
        }

        public async void ExecuteNonQueryAsync(MySqlCommand command)
        {
            await command.ExecuteNonQueryAsync();
            ReleaseEvent?.Invoke((T)this);
        }

        public object ExecuteScalar(MySqlCommand command)
        {
            command.Connection = CurrentConnection;
            object result = command.ExecuteScalar();
            return result;
        }

        protected void Release()
        {
            ReleaseEvent?.Invoke((T)this);
        }
        #endregion
    }
}

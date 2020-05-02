using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Databases
{
    public class MainDataBase : MySQLDataBase<MainDataBase>
    {
        #region TableNames
        private static readonly string Table_Events = "events";
        #endregion

        #region SQLQueries
        private static readonly string SQL_CreateEvent;
        private static readonly string SQL_GetEventByTime;
        #endregion

        static MainDataBase()
        {
            SQL_CreateEvent = $"INSERT INTO {Table_Events} (name, description, startTime) VALUES (@name, @desc, @sTime)";
            SQL_GetEventByTime = $"SELECT * FROM {Table_Events} WHERE startTime > @sTime AND startTime < @eTime";
        }

        public MainDataBase() : base(File.ReadLines(@"secretdatabaseinformation.txt").First())
        {
        }

        public void CreateEvent(string title, string description, DateTime time)
        {
            var command = new MySqlCommand(SQL_CreateEvent);
            command.Parameters.Add(new MySqlParameter("@name", title));
            command.Parameters.Add(new MySqlParameter("@desc", description));
            command.Parameters.Add(new MySqlParameter("@sTime", time));
            ExecuteNonQuery(command);
            Release();
        }

        public List<Event> GetEventByTime(DateTime startTime, DateTime endTime)
        {
            var command = new MySqlCommand(SQL_GetEventByTime);
            command.Parameters.Add(new MySqlParameter("@sTime", startTime));
            command.Parameters.Add(new MySqlParameter("@eTime", endTime));
            List<Event> events = new List<Event>();
            ExecuteReaderAsync(command, x =>
            {
                while (x.Read())
                {
                    var @event = new Event();
                    @event.ID = x.GetInt32(0);
                    @event.Name = x.GetString(1);
                    @event.Description = x.GetString(2);
                    @event.StartTime = x.GetDateTime(3);
                    events.Add(@event);
                }
            });
            Release();
            return events;
        }
    }
}

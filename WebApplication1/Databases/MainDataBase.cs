using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Databases
{
    public class MainDataBase : MySQLDataBase<MainDataBase>
    {
        #region TableNames
        private static readonly string Table_Events = "events";
            private static readonly string Table_Homeworks = "homeworks";
        #endregion

        #region SQLQueries
        private static readonly string SQL_CreateEvent;
        private static readonly string SQL_GetEventByTime;
        private static readonly string SQL_CreateHomework;
        private static readonly string SQL_GetHomeworkByTime;
        #endregion

        static MainDataBase()
        {
            SQL_CreateHomework = $"INSERT INTO {Table_Homeworks} (title,description,attachment,deadline) VALUES (@title, @description,@attachment,@deadline)";
            SQL_GetHomeworkByTime = $"SELECT * FROM {Table_Homeworks} WHERE deadline > @sdeadline AND deadline < @edeadline";
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

        public List<Event> GetEventsByTime(DateTime startTime, DateTime endTime)
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
                    @event.StartTime = x.GetDateTime(4);
                    events.Add(@event);
                }
            });
            Release();
            return events;
        }
        public void CreateHomework(string title, string description, DateTime time, string way)
        {
            var command = new MySqlCommand(SQL_CreateEvent);
            command.Parameters.Add(new MySqlParameter("@title", title));
            command.Parameters.Add(new MySqlParameter("@description", description));
            command.Parameters.Add(new MySqlParameter("@attachment", way));
            command.Parameters.Add(new MySqlParameter("@deadline", time));
            ExecuteNonQuery(command);
            Release();
        }
        public List<Event> GetHomeworksByTime(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
            //Release();
            //return events;
        }


    }
}

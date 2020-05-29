using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication1.DatabaseModels;
using WebApplication1.Models;

namespace WebApplication1.Databases
{
    public class MainDataBase : MySQLDataBase<MainDataBase>
    {
        #region TableNames
        private static readonly string Table_Events = "events";
        private static readonly string Table_Homeworks = "homeworks";
        private static readonly string Table_Courses = "courses";
        private static readonly string Table_Users = "users";
        #endregion

        #region SQLQueries
        private static readonly string SQL_CreateEvent;
        private static readonly string SQL_GetEventByTime;
        private static readonly string SQL_CreateHomework;
        private static readonly string SQL_GetHomeworkByTime;
        private static readonly string SQL_RegisterUser;
        private static readonly string SQL_ContainsLogin;
        #endregion

        static MainDataBase()
        {
            SQL_CreateHomework = $"INSERT INTO {Table_Homeworks} (title,description,attachment,deadline) VALUES (@title, @description,@attachment,@deadline)";
            SQL_GetHomeworkByTime = $"SELECT h.id, h.groupid, h.title, h.description, h.attachment, h.deadline, h.courseid, c.courseName FROM {Table_Homeworks} as h, {Table_Courses} as c WHERE h.courseid = c.id AND deadline >= @sdeadline AND deadline <= @edeadline ORDER BY h.deadline";
            SQL_CreateEvent = $"INSERT INTO {Table_Events} (name, description, startTime) VALUES (@name, @desc, @sTime)";
            SQL_GetEventByTime = $"SELECT * FROM {Table_Events} WHERE startTime >= @sTime AND startTime <= @eTime";
            SQL_RegisterUser = $"INSERT INTO {Table_Users} (login, firstname, lastname, otchestvo, password) VALUES (@login, @fn, @ln, @ot, @pass)";
            SQL_ContainsLogin = $"SELECT COUNT(*) FROM users WHERE login = @login";
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

        public List<Homework> GetHomeworksByTime(DateTime startTime, DateTime endTime)
        {
            var command = new MySqlCommand(SQL_GetHomeworkByTime);
            command.Parameters.Add(new MySqlParameter("@sdeadline", startTime));
            command.Parameters.Add(new MySqlParameter("@edeadline", endTime));
            var homeworks = new List<Homework>();
            ExecuteReaderAsync(command, x =>
            {
                while (x.Read())
                {
                    var homework = new Homework();
                    homework.ID = x.GetInt32(0);
                    homework.GroupID = x.GetInt32(1);
                    homework.Title = x.GetString(2);
                    homework.Description = x.GetString(3);
                    if (!x.IsDBNull(4))
                        homework.Attachment = x.GetString(4);
                    homework.Deadline = x.GetDateTime(5);
                    homework.Course = new Course()
                    {
                        ID = x.GetInt32(6),
                        CourseName = x.GetString(7)
                    };
                    homeworks.Add(homework);
                }
            });
            Release();
            return homeworks;
        }

        public bool RegisterUser(string login, string firstName, string lastName, string otchestvo, byte[] password)
        {
            var command = new MySqlCommand(SQL_RegisterUser);
            command.Parameters.Add(new MySqlParameter("@login", login));
            command.Parameters.Add(new MySqlParameter("@fn", firstName));
            command.Parameters.Add(new MySqlParameter("@ln", lastName));
            command.Parameters.Add(new MySqlParameter("@ot", otchestvo));
            command.Parameters.Add(new MySqlParameter("@pass", password));
            var result = ExecuteNonQuery(command);
            Release();
            return result == 1;
        }

        public bool ContainsLogin(string login)
        {
            var command = new MySqlCommand(SQL_ContainsLogin);
            command.Parameters.Add(new MySqlParameter("@login", login));
            var result = (long)ExecuteScalar(command);
            Release();
            return result > 0;
        }
    }
}

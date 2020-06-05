using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication1.Controllers;
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
        private static readonly string Table_Students = "students";
        private static readonly string Table_Prepods = "prepods";
        private static readonly string Table_Organizers = "organizers";
        private static readonly string Table_Groups = "groups";
        #endregion

        #region SQLQueries
        private static readonly string SQL_CreateEvent;
        private static readonly string SQL_GetEventByTime;
        private static readonly string SQL_CreateHomework;
        private static readonly string SQL_GetHomeworkByTime;
        private static readonly string SQL_RegisterUser;
        private static readonly string SQL_ContainsLogin;
        private static readonly string SQL_GetToken;
        private static readonly string SQL_GetUser;
        private static readonly string SQL_IsTeacher;
        private static readonly string SQL_IsStudent;
        private static readonly string SQL_IsEventsHolder;
        private static readonly string SQL_GetTeacherGroups;
        private static readonly string SQL_GetStudentGroups;
        private static readonly string SQL_HomeworkChain;
        #endregion

        static MainDataBase()
        {
            SQL_HomeworkChain = $" INSERT INTO homeworkassigment(groupid, homeworkid) VALUES(@groupid, @homeworkid)";
            SQL_CreateHomework = $"INSERT INTO {Table_Homeworks} (title,description,attachment,deadline) VALUES (@title, @description,@attachment,@deadline)";
            SQL_GetHomeworkByTime = $"SELECT h.id, h.groupid, h.title, h.description, h.attachment, h.deadline, h.courseid, c.courseName FROM {Table_Homeworks} as h, {Table_Courses} as c WHERE h.courseid = c.id AND deadline >= @sdeadline AND deadline <= @edeadline ORDER BY h.deadline";
            SQL_CreateEvent = $"INSERT INTO {Table_Events} (name, description, startTime) VALUES (@name, @desc, @sTime)";
            SQL_GetEventByTime = $"SELECT * FROM {Table_Events} WHERE startTime >= @sTime AND startTime <= @eTime";
            SQL_RegisterUser = $"INSERT INTO {Table_Users} (login, firstname, lastname, otchestvo, password, token) VALUES (@login, @fn, @ln, @ot, @pass, @token)";
            SQL_ContainsLogin = $"SELECT COUNT(*) FROM {Table_Users} WHERE login = @login";
            SQL_GetToken = $"SELECT token FROM {Table_Users} WHERE login = @login AND password = @pass";
            SQL_GetUser = $"SELECT id, firstName, lastName FROM {Table_Users} WHERE token = @token";
            SQL_IsTeacher = $"SELECT COUNT(*) FROM {Table_Prepods} WHERE userid = @userid";
            SQL_IsStudent = $"SELECT COUNT(*) FROM {Table_Students} WHERE userid = @userid";
            SQL_IsEventsHolder = $"SELECT COUNT(*) FROM {Table_Organizers} WHERE userid = @userid";
            SQL_GetTeacherGroups = $"SELECT g.id as groupid, g.groupName, c.id as courseid, c.coursename " +
                $"FROM {Table_Groups} as g, {Table_Courses} as c, {Table_Prepods} as p " +
                $"WHERE p.userid = @userid AND p.groupid = g.id AND c.id = g.courseid";
            SQL_GetStudentGroups = $"SELECT g.id as groupid, g.groupName, c.id as courseid, c.coursename " +
                $"FROM {Table_Groups} as g, {Table_Courses} as c, {Table_Students} as p " +
                $"WHERE p.userid = @userid AND p.groupid = g.id AND c.id = g.courseid";
        }

        public MainDataBase() : base(File.ReadLines(@"secretdatabaseinformation.txt").First())
        {
        }

        public void ChainHomework(int groupid,int homeworkid)
        {
            var command = new MySqlCommand(SQL_HomeworkChain);
            command.Parameters.Add(new MySqlParameter("@groupid", groupid));
            command.Parameters.Add(new MySqlParameter("@homeworkid", homeworkid));
            ExecuteNonQuery(command);
            Release();

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
        public int CreateHomework(string attachment, string description, string title, DateTime deadline )
        {
            var command = new MySqlCommand(SQL_CreateHomework);
            command.Parameters.Add(new MySqlParameter("@title", title));
            command.Parameters.Add(new MySqlParameter("@description",  description));
            command.Parameters.Add(new MySqlParameter("@attachment", attachment));
            command.Parameters.Add(new MySqlParameter("@deadline", deadline));
            
            ExecuteNonQuery(command);
            Release();
            return ((int)command.LastInsertedId);
           
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

        //public void CreateHomework(string title, string description, DateTime time, string way)
        //{
        //    var command = new MySqlCommand(SQL_CreateEvent);
        //    command.Parameters.Add(new MySqlParameter("@title", title));
        //    command.Parameters.Add(new MySqlParameter("@description", description));
        //    command.Parameters.Add(new MySqlParameter("@attachment", way));
        //    command.Parameters.Add(new MySqlParameter("@deadline", time));
        //    ExecuteNonQuery(command);
        //    Release();
        //}

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
            var token = new Random().NextString(64);
            var command = new MySqlCommand(SQL_RegisterUser);
            command.Parameters.Add(new MySqlParameter("@login", login));
            command.Parameters.Add(new MySqlParameter("@fn", firstName));
            command.Parameters.Add(new MySqlParameter("@ln", lastName));
            command.Parameters.Add(new MySqlParameter("@ot", otchestvo));
            command.Parameters.Add(new MySqlParameter("@pass", password));
            command.Parameters.Add(new MySqlParameter("@token", token));
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

        public string GetToken(string login, byte[] password)
        {
            var command = new MySqlCommand(SQL_GetToken);
            command.Parameters.Add(new MySqlParameter("@login", login));
            command.Parameters.Add(new MySqlParameter("@pass", password));
            string result = null;
            ExecuteReader(command, (reader) =>
            {
                if (reader.Read())
                {
                    result = reader.GetString(0);
                }
            });
            Release();
            return result;
        }

        internal void FillUser(AccountModel model, string token)
        {
            var command = new MySqlCommand(SQL_GetUser);
            command.Parameters.Add(new MySqlParameter("@token", token));
            ExecuteReader(command, reader =>
            {
                if (reader.Read())
                {
                    model.UserID = reader.GetInt32(0);
                    model.FirstName = reader.GetString(1);
                    model.LastName = reader.GetString(2);
                }
                reader.Close();
            });
            command = new MySqlCommand(SQL_IsTeacher);
            command.Parameters.Add(new MySqlParameter("@userid", model.UserID));
            model.IsTeacher = (long)ExecuteScalar(command) > 0;
            command = new MySqlCommand(SQL_IsStudent);
            command.Parameters.Add(new MySqlParameter("@userid", model.UserID));
            model.IsStudent = (long)ExecuteScalar(command) > 0;
            command = new MySqlCommand(SQL_IsEventsHolder);
            command.Parameters.Add(new MySqlParameter("@userid", model.UserID));
            model.IsEventsHolder = (long)ExecuteScalar(command) > 0;
            Release();
        }

        public List<Group> GetTeacherGroups(int userid)
        {
            var command = new MySqlCommand(SQL_GetTeacherGroups);
            command.Parameters.Add(new MySqlParameter("@userid", userid));
            var result = new List<Group>();
            ExecuteReader(command, reader =>
            {
                while (reader.Read())
                {
                    var group = new Group();
                    group.GroupID = reader.GetInt32(0);
                    group.GroupName = reader.GetString(1);
                    group.Course = new Course()
                    {
                        ID = reader.GetInt32(2),
                        CourseName = reader.GetString(3)
                    };
                    result.Add(group);
                }
            });
            return result;
        }

        public List<Group> GetStudentGroups(int userid)
        {
            var command = new MySqlCommand(SQL_GetStudentGroups);
            command.Parameters.Add(new MySqlParameter("@userid", userid));
            var result = new List<Group>();
            ExecuteReader(command, reader =>
            {
                while (reader.Read())
                {
                    var group = new Group();
                    group.GroupID = reader.GetInt32(0);
                    group.GroupName = reader.GetString(1);
                    group.Course = new Course()
                    {
                        ID = reader.GetInt32(2),
                        CourseName = reader.GetString(3)
                    };
                    result.Add(group);
                }
            });
            return result;
        }
    }
}

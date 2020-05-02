using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Databases;

namespace WebApplication1
{
    public class DatabaseManager
    {
        public static Queue<MainDataBase> MainDatabases = new Queue<MainDataBase>();

        static DatabaseManager()
        {

        }

        private static void CreateMain()
        {
            Debug.WriteLine("Create Main");
            var db = new MainDataBase();
            db.ReleaseEvent += DbMain_Release;
            MainDatabases.Enqueue(db);
        }

        private static void DbMain_Release(MainDataBase db)
        {
            Debug.WriteLine("Release Main");
            MainDatabases.Enqueue(db);
        }

        public static MainDataBase GetMain()
        {
            Debug.WriteLine("Get Main");
            if (MainDatabases.Count == 0)
                CreateMain();
            return MainDatabases.Dequeue();
        }
    }
}

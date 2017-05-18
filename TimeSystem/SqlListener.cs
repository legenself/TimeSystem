using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSystem
{
    public static class SqlListener
    {
        static SqlConnection connection;
        static string query = "SELECT [Id] ,[Uid],[Cron] ,[ApplicationUid],[Paras],[Enable],[Repeat] ,[Delay]  FROM [dbo].[Schedule_t]";

        static string _connStr = ConfigurationManager.AppSettings.Get("sqlconnectstring");
 
        public static void Start() {
            SqlDependency.Stop(_connStr);
            SqlDependency.Start(_connStr);
            connection = new SqlConnection(_connStr);
            StartDependency();
        }
        public static void StartDependency()
        {
            //这里很奇怪，每次都需要新的command对象
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Notification = null;
                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }
        private static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                //注销监测事件
                SqlDependency dependency = (SqlDependency)sender;
                dependency.OnChange -= dependency_OnChange;
                TaskHelper.Sche();
                Console.WriteLine("更新");

                StartDependency();
            }
        }
        public static void Stop()
        {
            SqlDependency.Stop(_connStr);
            if (connection != null)
                connection.Close();
        }
    }
}

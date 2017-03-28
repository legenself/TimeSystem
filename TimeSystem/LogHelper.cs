using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSystem
{
    public class LogHelper
    {
        /// <summary>
        /// 输出日志到数据库
        /// </summary>
        /// <param name="task"></param>
        public static void WriteLog(Schedule_t task, string msg = null)
        {
            lock (typeof(LogHelper))
            {
                TaskEntities db = new TaskEntities();
                db.Log_t.Add(new Log_t()
                {
                    Message = msg,
                    Result = 0,
                    Time = DateTime.Now,
                    TaskUid = task.Uid,
                    Type = 0
                });
                db.SaveChanges();

            }
        }
        /// <summary>
        /// 输出日志到数据库
        /// </summary>
        /// <param name="task"></param>
        public static void WriteLog(string msg)
        {
            lock (typeof(LogHelper))
            {
                TaskEntities db = new TaskEntities();
                db.Log_t.Add(new Log_t()
                {
                    Message = msg,
                    Result = 0,
                    Time = DateTime.Now,
                    Type = 0
                });
                db.SaveChanges();
            }
        }
    }
}

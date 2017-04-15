using CSRedis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSystem
{
    public partial class Schedule_t {
        public Application_t application {
            get {
                TaskEntities db = new TaskEntities();
                return db.Application_t.FirstOrDefault(p => p.Uid == this.ApplicationUid);
            }
        }

        public Dictionary<string,string> status{
            get {
                using (RedisClient rc = new RedisClient("192.168.1.70"))
                {
                    string s = Uid.ToString().ToLower();
 
                    return rc.HGetAll(s);
                }
            }
        }
        public string realLogPath
        {
            get
            {
                var path = this.LogPath + this.Id + "-" + this.Uid.ToString() + @"\";

                return path;
            }
        }
    }
}

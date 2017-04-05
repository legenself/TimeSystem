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
                return db.Application_t.FirstOrDefault(p=>p.Uid==this.ApplicationUid);
            }
        }  
        public string realLogPath {
            get {
                var path = this.LogPath + this.Id + "-" + this.Uid.ToString() + @"\";

                return path;
            }
        }

        public string GetLastLogItem
        {
            get
            {
                var filename = logs.Last();

                return GetLogItems(filename, (int)GetIndex(filename).Count-1);
            }
        }
        public List<long> GetIndex(string filename) {
            FileStream fs;
            //获得文件所在路径  
 
            try
            {
                fs = new FileStream(realLogPath + filename, FileMode.Open);
            }
            catch (Exception)
            {
                throw;
            }
            List<long> index = new List<long>();
            int c;
            while ((c = fs.ReadByte()) != -1)
            {
                if (c == 36)
                {
                    index.Add(fs.Position - 1);
                }
            }
            fs.Close();
            return index;
        }
        public string GetLogItems(string filename, int i)
        {

            List<long> index = GetIndex(filename);
            FileStream fs = new FileStream(realLogPath + filename, FileMode.Open);
            //获得文件所在路径  
            return GetLogItem((int)index[i - 1], (int)index[i], fs);
        }
        public List<string> GetLogItems(string filename,int[] list ) {
           
            List<long> index = GetIndex(filename);
            FileStream fs  = new FileStream(realLogPath + filename, FileMode.Open);
            List<string> items = new List<string>();
            //获得文件所在路径  
            foreach(var i in list) {
                items.Add(GetLogItem((int)index[i-1], (int)index[i],fs));
            }
            return items;
        }
        private string GetLogItem(int start,int end, FileStream fs) {
            if (start <= 0)
            {
                return "";
            }
            fs.Position = start;
            long length = end-start;
            byte[] bytes = new byte[length];
            fs.Read(bytes, 0, (int)length);
            return Encoding.Default.GetString(bytes);
        }
        public string[] logs {
            get {
               return Directory.GetFiles(realLogPath, "*.txt").Select(p=>Path.GetFileName(p)).ToArray();
            }
        }
    }
}

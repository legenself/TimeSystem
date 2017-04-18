using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeSystemManageWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult Status()
        {
            TaskEntities db = new TaskEntities();
            var schedules = db.Schedule_t.ToList();
            RedisClient rc = new RedisClient("192.168.1.70");
            schedules.ForEach(
                p =>
                {
                    p.App = db.Application_t.First(p1 => p1.Uid == p.ApplicationUid);
                    var status = rc.HGetAll(p.Uid.ToString());
                    if (status!=null&& status.Count!=0) {
                        status["update"] = DateTime.Parse(status["update"]).ToString("yyyy-MM-dd HH:mm:ss");
                        var st = DateTime.Parse(status["startTime"]);
                        status["startTime"] = st.ToString("yyyy-MM-dd HH:mm:ss");
                        var ts = (int)decimal.Parse(status["history"]);
                        status.Add("predict",DateTime.Parse(status["startTime"]).AddSeconds(ts).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    p.Status = status;
                }
                );
            
            JsonResult result = new JsonResult();
            result.Data = schedules;


            return result;
        }

        public JsonResult updateSchedule(Guid uid,string cron ,int enable,string paras)
        {
            JsonResult result = new JsonResult();
            try
            {

                TaskEntities db = new TaskEntities();
                var old = db.Schedule_t.FirstOrDefault(p => p.Uid == uid);
                old.Paras = paras;
                old.Enable = enable;
                old.Cron = cron;
                db.SaveChanges();
                RedisClient rc = new RedisClient("192.168.1.70");
                rc.Publish("cmd", "refresh");


                result.Data = new {status=1, msg = "成功修改并刷新" };
            }
            catch (Exception ex){
                result.Data = new { status = 0, msg =ex.ToString()};
            }
            return result;
        }
       
    }
}
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
        public JsonResult Service() {
            JsonResult result = new JsonResult();

            

            return result;
        }
        public JsonResult Notification()
        {
            RedisClient rc = new RedisClient("192.168.1.70");
            JsonResult result = new JsonResult();

            rc.Keys("status*");

            return result;
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
                    p.Status = rc.HGetAll("status_"+p.Uid.ToString());
                }
                );
            JsonResult result = new JsonResult();
            result.Data = schedules;


            return result;
        }

        public JsonResult updateSchedule(Guid uid, string cron, int enable, string paras)
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

                result.Data = new { status = 1, msg = "成功修改" };
            }
            catch (Exception ex)
            {
                result.Data = new { status = 0, msg = ex.ToString() };
            }
            return result;
        }

    }
}
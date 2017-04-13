using CSRedis;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace TimeSystem
{
    /// <summary>
    /// webapi控制器
    /// </summary>
    public class TaskController : ApiController
    {
        [HttpGet]
        public IHttpActionResult All()
        {
            var jobs = TaskHelper.Schedules.Select(p => new
            {
                Id = p.Id,
                Application = p.application.Name,
                Description = p.application.Description,
                Memo = p.Memo,
                Cron = p.Cron,
                Enable = p.Enable,
                Status=p.status
                //Logfile = p.logs,
                //lastlog=p.GetLastLogItem
            }).ToArray();
            return Json<dynamic>(jobs);
        }
        [HttpGet]
        public bool refresh()
        {
            try
            {

                TaskHelper.Sche();
                LogHelper.WriteLog("刷新成功");

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("刷新失败" + ex);

                return false;
            }
        }
    }
}

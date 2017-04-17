using CSRedis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace TimeSystem
{
    public class CoreService
    {
        private static Hashtable ht = new Hashtable();
        RedisClient redis;
        string srvName;
        string srvDesc;
        public CoreService(string srvName, string srvDesc)
        {

            this.srvName = srvName;
            this.srvDesc = srvDesc;
            redis = new RedisClient("192.168.1.70");
            redis.Ping();
            //redis.SubscriptionChanged += (s, e) =>
            //{
            //    Console.WriteLine("There are now {0} open channels", e.Response.Count);
            //};
            redis.SubscriptionReceived += (s, e) =>
            {
                switch (e.Message.Body.ToLower()) {
                    case "refresh":
                        TaskHelper.Sche();
                        break;
                }

                //Console.WriteLine("Message received: {0}", e.Message.Body);
            };

        }
        public void Start()
        {
            try
            {
                LogHelper.WriteLog(srvName + "将要启动了...");

                //服务启动
                LoadJob();

                LogHelper.WriteLog(srvName + "启动成功!");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(srvName + "启动失败:" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 暂停服务时执行
        /// </summary>
        public void Stop()
        {
            LogHelper.WriteLog(srvName + "停止了!");
            //服务停止
            StopJob();
        }
        /// <summary>
        /// 关闭服务时执行
        /// </summary>
        public void Shutdown()
        {
            LogHelper.WriteLog(srvName + "关闭了!");
            //服务关闭
            StopJob();
        }
        /// <summary>
        /// 继续服务时
        /// </summary>
        public void Continue()
        {
            LoadJob();
            LogHelper.WriteLog(srvName + "继续了!");
            //服务继续

        }
        /// <summary>
        /// 暂停服务
        /// </summary>
        public void Pause()
        {
            LogHelper.WriteLog(srvName + "暂停了!");
            //服务暂停
            StopJob();
        }

        private void StopJob()
        {
            redis.PUnsubscribe("cmd");
          
        }

        private void LoadJob()
        {


            TaskHelper.Sche();
            Task.Run(() => {
                redis.PSubscribe("cmd");
            });
        }
    }
}

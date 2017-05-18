using CSRedis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace TimeSystem
{
    public class CoreService
    {
        private static Hashtable ht = new Hashtable();

        string srvName;
        string srvDesc;

        public CoreService(string srvName, string srvDesc)
        {

            this.srvName = srvName;
            this.srvDesc = srvDesc;
            
         }
        public void Start()
        {
            try
            {

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
            //服务停止
            StopJob();
            LogHelper.WriteLog(srvName + "停止了!");

        }
        /// <summary>
        /// 关闭服务时执行
        /// </summary>
        public void Shutdown()
        {
            //服务关闭
            StopJob();
            LogHelper.WriteLog(srvName + "关闭了!");

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
            //服务暂停
            StopJob();
            LogHelper.WriteLog(srvName + "暂停了!");

        }

        private void StopJob()
        {
            SqlListener.Stop();

        }

        private void LoadJob()
        {
            SqlListener.Start();
            TaskHelper.Sche();
        }
 
 


 
    }
}

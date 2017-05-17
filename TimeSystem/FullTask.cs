using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSystem
{
    public class FullTask : IJob
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {

            try
            {
                var job = context.JobDetail.JobDataMap.Get("Schedule") as Schedule_t;
                LogHelper.WriteLog(job, "开始执行");

                ExeJob(job);
                LogHelper.WriteLog(job, "执行成功");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
            }
        }
        /// <summary>
        /// 执行具体任务
        /// </summary>
        /// <param name="job"></param>
        public void ExeJob(Schedule_t job)
        {

            Process proc = null;

            proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(job.application.Path);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();

            string appcmd = "";

            appcmd = job.application.Path + " " + job.Paras + " >> " + job.realLogPath + DateTime.Now.ToString(job.LogPattern) + ".txt";
            appcmd = string.Format(@"{0} {1} {2}", job.application.Path, job.Paras + " >> " + job.realLogPath + DateTime.Now.ToString(job.LogPattern) + ".txt", "&exit");

            //myPro.StandardInput.WriteLine(str);
            proc.StandardInput.WriteLine(appcmd);
            //proc.StandardInput.WriteLine("exit");
            proc.WaitForExit();
            proc.Close();
        }
    }
}
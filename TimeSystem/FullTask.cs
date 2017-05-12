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

            //处理调用时的参数,默认没有参数
            //Type[] types = new Type[] { };
            //object[] paras = new object[] { };
            //string[] parastring = new string[] { };
            //if (job.Paras != null && job.Paras.Split(',').Length > 0)
            //{
            //    parastring = job.Paras.Split(',');
            //    //有参数情况,参数默认是一个字符串数组
            //    types = new Type[] { typeof(string[]) };
            //    paras = new object[1];
            //    paras[0] = new string[parastring.Length];
            //    for (int i = 0; i < parastring.Length; i++)
            //    {
            //        (paras[0] as string[])[i] = parastring[i];
            //    }
            //}


            Process proc = null;

            proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(job.application.Path);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            //StringBuilder sb = new StringBuilder();
            //sb.Append(job.application.Path);
            //sb.Append(' ');
            //sb.Append(@job.Paras);
            //sb.Append(" >> ");
            //sb.Append(job.realLogPath);
            //sb.Append(DateTime.Now.ToString(job.LogPattern));
            //sb.Append(".txt");
            //sb.ToString();

             string appcmd = job.application.Path + " " + job.Paras + " >> " + job.realLogPath + DateTime.Now.ToString(job.LogPattern)+".txt";

            //var ar = sb.ToString().ToArray();
            proc.StandardInput.WriteLine(appcmd);
            proc.StandardInput.WriteLine("exit");
            proc.WaitForExit();
            proc.Close();
        }
    }
}
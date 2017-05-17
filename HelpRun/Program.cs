using CSRedis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpRun
{
    public class Program
    {
        static string ApplicationUid, filepath, runargs, logpath, logpattern;
        /// <summary>
        /// 数组
        /// 想要运行的文件全路径
        /// 想要运行的文件参数
        /// 日志路径
        /// 日志格式
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            rc = new RedisClient(ConfigurationManager.AppSettings.Get("redishost"));
            rc.Connect(1000);
            if (ConfigurationManager.AppSettings.Get("password") != "")
            {

                rc.Auth(ConfigurationManager.AppSettings.Get("password"));

            }
            if (Boolean.Parse(ConfigurationManager.AppSettings.Get("debug"))) {
                rc.LPush("debug", args);
            }
            ApplicationUid = args[0];
            filepath = args[1];
            runargs = args[2];
            //logpath = args[3];
            //logpattern = args[4];
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            rc.HSet("status_" + ApplicationUid, "startTime", DateTime.Now);
            rc.HIncrBy("status_" + ApplicationUid, "condition", 1);
            rc.HSet("status_" + ApplicationUid, "updateTime", DateTime.Now);


            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(filepath);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.OutputDataReceived += Proc_OutputDataReceived;
            proc.ErrorDataReceived += Proc_ErrorDataReceived;
            proc.Start();
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();
            string cmdpattern = GenerateCmd(filepath);
            //string appcmd = job.application.Path + " " + args[1] + " >> " + job.realLogPath + DateTime.Now.ToString(job.LogPattern) + ".txt";
            //string appcmd = string.Format(cmdpattern, filepath, runargs) + "  >> " + logpath + DateTime.Now.ToString(logpattern) + ".txt"; 
            string appcmd = string.Format(cmdpattern, filepath, runargs);


            proc.StandardInput.WriteLine(appcmd);


            proc.StandardInput.WriteLine("exit");
            proc.WaitForExit();
            proc.Close();
            proc.Dispose();
            stopwatch.Stop();



            rc.HIncrBy("status_" + ApplicationUid, "condition", -1);

            sethistory(stopwatch.Elapsed.TotalSeconds);
        }

        private static void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data==null|e.Data == "") {
                return;
            }
            var now = DateTime.Now;

            rc.HSet("status_" + ApplicationUid, "updateTime", now);

            rc.LPush("error_" + ApplicationUid, now + ":" + e.Data);
        }

        static void sethistory(double runtime)
        {
            rc.HSetNx("status_" + ApplicationUid, "history", runtime);
            rc.HSet("status_" + ApplicationUid, "updateTime", DateTime.Now);

            var s = double.Parse(rc.HGet("status_" + ApplicationUid, "history"));
            rc.HSet("status_" + ApplicationUid, "history", (runtime + s) / 2.0);
        }
        static RedisClient rc;
        private static void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null ||e.Data == "")
            {
                return;
            }
            string[] data = e.Data.Split('|');
            var now = DateTime.Now;
            switch (data[0]) {
                case "message":
                    rc.HSet("status_" + ApplicationUid, "updateTime", now);
                    rc.LPush("message_" + ApplicationUid, now + " " + data[1]);
                    break;
                case "status":
                    rc.HSet("status_" + ApplicationUid, "updateTime", now);
                    rc.HSet("status_" + ApplicationUid,"status", data[1]);
                    break;
            }


        }

        public static string GenerateCmd(string filename)
        {
            string extension = filename.Split('.').Last();
            string pattern = ConfigurationManager.AppSettings.Get(extension);
            if (pattern != null)
            {
                return pattern;
            }
            return "{0} {1}";
        }
    }
}

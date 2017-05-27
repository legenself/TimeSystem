using CSRedis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisClient rc = new RedisClient(ConfigurationManager.AppSettings.Get("redishost"));
            if (ConfigurationManager.AppSettings.Get("password") != "")
            {

                rc.Auth(ConfigurationManager.AppSettings.Get("password"));

            }
            while (rc.LLen("message") > 0) {

                string message = rc.LIndex("message", 0);
                var paras = message.Split(',');
                SendMail(paras[0], paras[1], paras[2]);
                rc.LPop("message");
                    
              }

        }
        public static void SendMail(string target, string title, string content)
        {
            string emailAcount = ConfigurationManager.AppSettings.Get("hostemail");
            string emailPassword = ConfigurationManager.AppSettings.Get("hostpassword");
            var reciver = target;
            MailMessage message = new MailMessage();
            //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress(emailAcount);
            message.From = fromAddr;

            //设置收件人,可添加多个,添加方法与下面的一样
            Array.ForEach(reciver.Split(';'), p =>
            {
                message.To.Add(p);
            });

            //设置抄送人
            //message.CC.Add("izhaofu@163.com");
            //设置邮件标题
            message.Subject = title;
            //设置邮件内容
            message.Body = content;
            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
            SmtpClient client = new SmtpClient("smtp.qq.com", 587);
            //设置发送人的邮箱账号和密码
            client.Credentials = new NetworkCredential(emailAcount, emailPassword);
            //启用ssl,也就是安全发送
            client.EnableSsl = true;
            //发送邮件
            client.Send(message);
        }

    }
}

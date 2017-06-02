using CSRedis;
using Mailzory;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class Program
    {
        static void Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "MyMainDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);
                var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
                return  ;
            }

                RedisClient rc = new RedisClient(ConfigurationManager.AppSettings.Get("redishost"));
                rc.Connect(1000);


                if (ConfigurationManager.AppSettings.Get("password") != "")
                {

                    rc.Auth(ConfigurationManager.AppSettings.Get("password"));

                }



                //读取出所有信息

                List<Message> unSendMessage = new List<Message>();
                //for (var i = 0; i < unSendCount; i++) {
                //    unSendMessage.Add(rc.LPop(""));
                //}
                string item;
                while ((item = rc.LPop("message")) != null)
                {
                    unSendMessage.Add(new Message(item));
                }
                if (unSendMessage.Count == 0) {
                    Console.WriteLine($"没有待发送消息");
                    return;

                }

                Console.WriteLine($"有{unSendMessage.Count}个待发送消息");

                //string[] unSendMessage = rc.LRange("message", 0, unSendCount);
                SendMail("检测报告", unSendMessage);

                //rc.LRem("message", 0, unSendCount);

                //rc.a("message");


                Console.WriteLine($"{unSendMessage.Count}个消息 发送完成");


        }
        public static void SendMail(string title, List<Message> content)
        {
            var emailAcount = ConfigurationManager.AppSettings["EmailAcount"];
            var emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            var reciver = ConfigurationManager.AppSettings["Receiver"];
            SmtpClient client = new SmtpClient("smtp.qq.com", 587);
            //设置发送人的邮箱账号和密码
            client.Credentials = new NetworkCredential(emailAcount, emailPassword);
            //启用ssl,也就是安全发送
            client.EnableSsl = true;

            var viewPath = Path.Combine("View/Emails", "Content.cshtml");
            var template = File.ReadAllText(viewPath);

            var email = new Email(template, client);
            email.ViewBag.Name = title;
            email.ViewBag.Content = content;

            // set your desired display name (Optional)
            email.SetFrom(emailAcount, "传令官");

            // send email
            email.Send(reciver, title);
        }
    }
}

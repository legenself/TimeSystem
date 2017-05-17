using System;
using Topshelf;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Server is opened");
            var startInfo = DateTime.Now;
            HostFactory.Run(x =>                                 //1
            {
                x.Service<CoreService>(s =>                        //2
                {
                    s.ConstructUsing(name => new CoreService("TimeSystem", startInfo.ToString()));     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6
                x.SetDescription("TimeSystem" + startInfo);        //7
                x.SetDisplayName("TimeSystem");                       //8
                x.SetServiceName("TimeSystem");                       //9
            });
        }
    }
}

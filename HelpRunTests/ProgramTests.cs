using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpRun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpRun.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void MainTest()
        {
            string[] args = new string[] {
                "id",
                "C:/1-Application/DingDing/demo.py",
                "1,2,3,4,5",
                "d:/",
                "yyyy-MM-dd",
            };

            HelpRun.Program.Main(args);
         }

        [TestMethod()]
        public void GenerateCmdTest()
        {
             Dictionary<string, string> test = new Dictionary<string, string>() {
                { "sdasdsa.exe","{0} {1}"},
                { "fdskvdsv.py","python {0} {1}"},
                { "dfasfa.m","{0} {1}"},
                { "dsadada.java","{0} {1}"}
            };
            test.Keys.ToList().ForEach(p=> {
 
                Assert.AreEqual(test[p], Program.GenerateCmd(p));
            });
         }
    }
}
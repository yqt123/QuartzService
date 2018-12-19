using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IService;
using Quartz.Core.IOC;
using Quartz.Core.Owin;
using Quartz.Core.Quartz;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectContainer.ApplicationStart(new Quartz.Core.IOC.AutoFacContainer());

            //ITest _ITest = ObjectContainer.Current.Resolve<ITest>();
            //_ITest.SayHello();

            //Console.ReadLine();

            //var pcScheduler = Scheduler.Create();
            //pcScheduler.Start();


            OwinHelper owinHelper = OwinHelper.Create("8099");
            owinHelper.Start();
            Console.ReadLine();

            //Quartz.Core.Log4net.Log4.Info("test.......");
            //Console.ReadLine();
        }

    }
}

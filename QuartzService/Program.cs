using IService;
using Quartz.Core.IOC;
using Quartz.Core.Owin;
using Quartz.Core.Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new QuartzService()
            //};
            //ServiceBase.Run(ServicesToRun);
            Start();
        }

        static void Start()
        {
            ObjectContainer.ApplicationStart(new Quartz.Core.IOC.AutoFacContainer());

            ITest _ITest = ObjectContainer.Current.Resolve<ITest>();
            _ITest.SayHello(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "端口8099正在启动...");

            var pcScheduler = Scheduler.Create();
            //pcScheduler.Start();

            OwinHelper owinHelper = OwinHelper.Create("8099");
            owinHelper.Start();

            _ITest.SayHello(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "端口8099启动成功！");

            Console.ReadLine();

            //Quartz.Core.Log4net.Log4.Info("test.......");
            //Console.ReadLine();
        }
    }
}

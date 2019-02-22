using IService;
using Model;
using Nancy;
using Nancy.Helpers;
using Newtonsoft.Json;
using Quartz.Core.Quartz;
using QuartzWebNancy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzWebNancy.Modules
{
    public class QuartzModule : BaseModule
    {
        public static IQuartzSchedule bll = Quartz.Core.IOC.ObjectContainer.Current.Resolve<IQuartzSchedule>();

        public QuartzModule()
        {
            Get["/QuartzRun"] = r => QuartzView();
            Get["/QuartzRun/setStatus/{status:int}"] = r => SetQuartzStatus(r.status);
            Get["/QuartzRun/getStatus"] = r => GetQuartzStatus();
        }

        public dynamic QuartzView()
        {
            return View["View"];
        }

        public dynamic SetQuartzStatus(int status)
        {
            var pcScheduler = Scheduler.Create();
            var _status = (SchedulerStatusEnum)status;
            switch (_status)
            {
                case SchedulerStatusEnum.pause: { pcScheduler.PauseAll(); } break;
                case SchedulerStatusEnum.running: { pcScheduler.Start(); } break;
                case SchedulerStatusEnum.Shutdown: { pcScheduler.Shutdown(); } break;
            }
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = true });
            return jsonStr;
        }

        public dynamic GetQuartzStatus()
        {
            var pcScheduler = Scheduler.Create();
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = true, Result = pcScheduler.Status });
            return jsonStr;
        }
    }
}

using IService;
using Nancy;
using Nancy.Helpers;
using QuartzWebNancy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuartzWebNancy.Modules
{
    public class QuartzManageModule : BaseModule
    {
        public static IQuartzSchedule bll = Quartz.Core.IOC.ObjectContainer.Current.Resolve<IQuartzSchedule>();

        public QuartzManageModule()
        {
            Get["/quartzmanage"] = parameters => ReturnRedirectAction();
            Get["/quartzmanage/list"] = parameters => ReturnList();
            Get["/quartzmanage/add"] = parameters => ReturnAdd();
            Get["/quartzmanage/edit"] = parameters => ReturnEdit();
            Get["/quartzmanage/view"] = parameters => ReturnView();
            Get["/quartzmanage/scheduleDetails"] = parameters => GetScheduleDetails();
            Get["/quartzmanage/scheduleDetail/{id:int}"] = parameters => GetScheduleDetail(parameters.id);
        }

        public dynamic ReturnRedirectAction()
        {
            ViewBag.Title = "作业管理";
            return View["index", DynamicModel];
        }

        public dynamic ReturnList()
        {
            return View["list", DynamicModel];
        }

        public dynamic ReturnAdd()
        {
            return View["add"];
        }

        public dynamic ReturnEdit()
        {
            return View["edit"];
        }

        public dynamic ReturnView()
        {
            return View["view"];
        }

        /*******作业操作*********/
        public dynamic GetScheduleDetails()
        {
            var details = bll.ListScheduleDetails().ToList();
            var jsonStr = JsonConvert.SerializeObject(details);
            return jsonStr;
        }

        public dynamic GetScheduleDetail(int id)
        {
            var item = bll.GetScheduleDetails(id);
            var jsonStr = JsonConvert.SerializeObject(item);
            return jsonStr;
        }
    }
}

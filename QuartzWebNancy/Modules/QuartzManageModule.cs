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
using Model;

namespace QuartzWebNancy.Modules
{
    public class QuartzManageModule : BaseModule
    {
        public static IQuartzSchedule bll = Quartz.Core.IOC.ObjectContainer.Current.Resolve<IQuartzSchedule>();

        public QuartzManageModule()
        {
            Get["/quartzmanage"] = parameters => Index();
            Get["/quartzmanage/welcome"] = parameters => Welcome();
            Get["/quartzmanage/list"] = parameters => ReturnList();
            Get["/quartzmanage/add"] = parameters => ReturnAdd();
            Get["/quartzmanage/edit"] = parameters => ReturnEdit();
            Get["/quartzmanage/view"] = parameters => ReturnView();
            Get["/quartzmanage/scheduleDetails"] = parameters => GetScheduleDetails();
            Get["/quartzmanage/scheduleDetail/{id:int}"] = parameters => GetScheduleDetail(parameters.id);
            Post["/quartzmanage/saveScheduleDetail"] = parameters => SaveScheduleDetail();
            Post["/quartzmanage/deleteScheduleDetail/{id:int}"] = parameters => DeleteScheduleDetail(parameters.id);
            Post["/quartzmanage/editScheduleDetail"] = parameters => EditScheduleDetail();

            Get["/quartzmanage/triggerList"] = parameters => TriggerList();
            Get["/quartzmanage/triggers/{id:int}"] = parameters => GetTriggers(parameters.id);
        }

        public dynamic Welcome()
        {
            return View["Welcome"];
        }

        #region 作业

        public dynamic Index()
        {
            ViewBag.Title = "作业管理";
            return View["index"];
        }

        public dynamic ReturnList()
        {
            return View["list"];
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

        public dynamic GetScheduleDetails()
        {
            var details = bll.ListScheduleDetails().ToList();
            var jsonStr = JsonConvert.SerializeObject(details);
            return jsonStr;
        }

        public dynamic GetScheduleDetail(int id)
        {
            var item = bll.GetScheduleDetail(id);
            var jsonStr = JsonConvert.SerializeObject(item);
            return jsonStr;
        }

        public dynamic SaveScheduleDetail()
        {
            var post = GetParameters();
            var data = JsonConvert.DeserializeObject<ScheduleJob_Details>(post);
            var res = bll.SaveScheduleDetail(data);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }

        public dynamic DeleteScheduleDetail(int id)
        {
            var res = bll.DeleteScheduleDetail(id);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }

        public dynamic EditScheduleDetail()
        {
            var post = GetParameters();
            var data = JsonConvert.DeserializeObject<ScheduleJob_Details>(post);
            var res = bll.EditScheduleDetail(data);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }

        #endregion

        #region 触发器
        public dynamic TriggerList()
        {
            return View["TriggerList"];
        }

        public dynamic GetTriggers(int id)
        {
            var schedule = bll.GetScheduleDetail(id);
            var details = bll.ListScheduleDetailsTriggers(schedule.sched_name, schedule.job_name);
            var jsonStr = JsonConvert.SerializeObject(details);
            return jsonStr;
        }

        #endregion
    }
}

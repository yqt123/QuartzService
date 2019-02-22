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
using Quartz.Core.Quartz;

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
            Get["/quartzmanage/allTriggers"] = parameters => AllTriggers();
            Get["/quartzmanage/triggers/{id:int}"] = parameters => GetTriggers(parameters.id);
            Get["/quartzmanage/trigger/{id:int}"] = parameters => GetTrigger(parameters.id);
            Get["/quartzmanage/triggerAdd"] = parameters => TriggerAdd();
            Get["/quartzmanage/triggerEdit"] = parameters => TriggerEdit();
            Post["/quartzmanage/saveTrigger"] = parameters => SaveTrigger();
            Post["/quartzmanage/deleteTrigger/{id:int}"] = parameters => DeleteTrigger(parameters.id);
            Post["/quartzmanage/editTrigger"] = parameters => EditTrigger();

        }

        public dynamic Welcome()
        {
            return View["Welcome"];
        }

        #region 作业

        public dynamic Index()
        {
            ViewBag.Title = "作业管理";
            var pcScheduler = Scheduler.Create();
            if (pcScheduler._QtzScheduler.IsStarted)
            {
                if (pcScheduler._QtzScheduler.IsShutdown)
                {
                    DynamicModel.StatusName = "已关闭";
                }
                else
                {
                    DynamicModel.StatusName = "正在执行...";
                }
            }
            else
            {
                DynamicModel.StatusName = "未开始";
            }
            switch (pcScheduler.Status)
            {
                case SchedulerStatusEnum.pause: { DynamicModel.StatusName = "已暂停"; } break;
                case SchedulerStatusEnum.running: { DynamicModel.StatusName = "正在执行..."; } break;
                case SchedulerStatusEnum.Shutdown: { DynamicModel.StatusName = "已关闭"; } break;
                default: { DynamicModel.StatusName = "未开始"; } break;
            }
            return View["index", DynamicModel];
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

        public dynamic TriggerAdd()
        {
            return View["triggerAdd"];
        }

        public dynamic TriggerEdit()
        {
            return View["triggerEdit"];
        }

        /// <summary>
        /// 所有触发器
        /// </summary>
        /// <returns></returns>
        public dynamic AllTriggers()
        {
            var details = bll.ListTriggers();
            var jsonStr = JsonConvert.SerializeObject(details);
            return jsonStr;
        }
        /// <summary>
        /// 获取作业对应的触发器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetTriggers(int id)
        {
            var schedule = bll.GetScheduleDetail(id);
            var details = bll.ListScheduleDetailsTriggers(schedule.sched_name, schedule.job_name);
            var jsonStr = JsonConvert.SerializeObject(details);
            return jsonStr;
        }
        /// <summary>
        /// 获取单个触发器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetTrigger(int id)
        {
            var schedule = bll.GetScheduleDetail(id);
            var details = bll.ListScheduleDetailsTrigger(id);
            var jsonStr = JsonConvert.SerializeObject(details);
            return jsonStr;
        }

        public dynamic SaveTrigger()
        {
            var post = GetParameters();
            var data = JsonConvert.DeserializeObject<ScheduleJob_Details_Triggers>(post);
            var res = bll.SaveScheduleDetailsTrigger(data);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }

        public dynamic DeleteTrigger(int id)
        {
            var res = bll.DeleteScheduleDetailsTrigger(id);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }

        public dynamic EditTrigger()
        {
            var post = GetParameters();
            var data = JsonConvert.DeserializeObject<ScheduleJob_Details_Triggers>(post);
            var res = bll.EditScheduleDetailsTrigger(data);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }

        #endregion
    }
}

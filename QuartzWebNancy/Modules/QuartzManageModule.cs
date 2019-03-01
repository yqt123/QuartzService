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
            Get["/quartzmanage/home"] = parameters => ReturnHome();
            Get["/quartzmanage/add"] = parameters => ReturnAdd();
            Get["/quartzmanage/edit"] = parameters => ReturnEdit();
            Get["/quartzmanage/view"] = parameters => ReturnView();
            Get["/quartzmanage/scheduleDetails"] = parameters => GetScheduleDetails();
            Get["/quartzmanage/scheduleDetail/{id:int}"] = parameters => GetScheduleDetail(parameters.id);
            Get["/quartzmanage/scheduleStatus"] = parameters => ScheduleStatus();

            Post["/quartzmanage/saveScheduleDetail"] = parameters => SaveScheduleDetail();
            Post["/quartzmanage/deleteScheduleDetail/{id:int}"] = parameters => DeleteScheduleDetail(parameters.id);
            Post["/quartzmanage/editScheduleDetail"] = parameters => EditScheduleDetail();
            Post["/quartzmanage/JobToSchedulePlan/{id:int}"] = parameters => JobToSchedulePlan(parameters.id);

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
            if (pcScheduler._QtzScheduler.IsShutdown)
            {
                DynamicModel.StatusName = "已关闭";
            }
            else
            {
                if (pcScheduler._QtzScheduler.IsStarted)
                {
                    DynamicModel.StatusName = "正在执行...";
                }
                else
                {
                    DynamicModel.StatusName = "未开始";
                }
            }
            return View["index", DynamicModel];
        }

        public dynamic ReturnHome()
        {
            return View["home"];
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
            var pcScheduler = Scheduler.Create();
            foreach (var item in details)
            {
                var _job = pcScheduler._QtzScheduler.GetJobDetail(JobHelper.GetJobKey(item)).GetAwaiter().GetResult();
                item.isRunning = _job != null;
            }
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

        public dynamic ScheduleStatus()
        {
            string StatusName = string.Empty;
            var pcScheduler = Scheduler.Create();

            if (pcScheduler._QtzScheduler.IsShutdown)
            {
                StatusName = "已关闭";
            }
            else
            {
                if (pcScheduler._QtzScheduler.IsStarted)
                {
                    StatusName = "正在执行...";
                }
                else
                {
                    StatusName = "未开始";
                }
            }
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = true, Result = StatusName });
            return jsonStr;
        }

        public dynamic JobToSchedulePlan(int id)
        {
            var item = bll.GetScheduleDetail(id);
            var pcScheduler = Scheduler.Create();
            JobHelper.ScheduleJobByPlan(pcScheduler._QtzScheduler, item);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = true });
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

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
            Get["/quartzmanage"] = r => Index();
            Get["/quartzmanage/welcome"] = r => Welcome();
            Get["/quartzmanage/home"] = r => ReturnHome();
            Get["/quartzmanage/add"] = r => ReturnAdd();
            Get["/quartzmanage/edit"] = r => ReturnEdit();
            Get["/quartzmanage/view"] = r => ReturnView();
            Get["/quartzmanage/scheduleDetails"] = r => GetScheduleDetails();
            Get["/quartzmanage/scheduleDetail/{id:int}"] = r => GetScheduleDetail(r.id);
            Get["/quartzmanage/scheduleStatus"] = r => ScheduleStatus();

            Post["/quartzmanage/saveScheduleDetail"] = r => SaveScheduleDetail();
            Post["/quartzmanage/deleteScheduleDetail/{id:int}"] = r => DeleteScheduleDetail(r.id);
            Post["/quartzmanage/editScheduleDetail"] = r => EditScheduleDetail();
            Post["/quartzmanage/JobToSchedulePlan/{id:int}/{type:int}"] = r => JobToSchedulePlan(r.id, r.type);
            Post["/quartzmanage/ExecuteNow/{id:int}"] = r => ExecuteNow(r.id);

            Get["/quartzmanage/triggerList"] = r => TriggerList();
            Get["/quartzmanage/allTriggers"] = r => AllTriggers();
            Get["/quartzmanage/triggers/{id:int}"] = r => GetTriggers(r.id);
            Get["/quartzmanage/trigger/{id:int}"] = r => GetTrigger(r.id);
            Get["/quartzmanage/triggerAdd"] = r => TriggerAdd();
            Get["/quartzmanage/triggerEdit"] = r => TriggerEdit();
            Post["/quartzmanage/saveTrigger"] = r => SaveTrigger();
            Post["/quartzmanage/deleteTrigger/{id:int}"] = r => DeleteTrigger(r.id);
            Post["/quartzmanage/editTrigger"] = r => EditTrigger();

            Get["/quartzmanage/scheduleLog"] = r => ScheduleLog();
            Get["/quartzmanage/getScheduleLog/{start:DateTime}/{end:DateTime}"] = r => GetScheduleLog(r.start, r.end);
            Post["/quartzmanage/delScheduleLog/{start:DateTime}/{end:DateTime}"] = r => DelScheduleLog(r.start, r.end);
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
            var item = bll.GetScheduleDetail(id);
            bll.DeleteScheduleDetailsTriggers(item.sched_name, item.job_name);
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

        public dynamic JobToSchedulePlan(int id, int type)
        {
            var item = bll.GetScheduleDetail(id);
            var res = new ResultJson { Status = false };
            if (type == 1)
            {
                if (item.is_durable)
                {
                    var pcScheduler = Scheduler.Create();
                    JobHelper.ScheduleJobByPlan(pcScheduler._QtzScheduler, item);
                    res.Status = true;
                }
            }
            else
            {
                var pcScheduler = Scheduler.Create();
                pcScheduler._QtzScheduler.DeleteJob(JobHelper.GetJobKey(item));
                res.Status = true;
            }
            var jsonStr = JsonConvert.SerializeObject(res);
            return jsonStr;
        }

        public dynamic ExecuteNow(int id)
        {
            var item = bll.GetScheduleDetail(id);
            var res = new ResultJson { Status = false };
            if (item.is_durable)
            {
                var pcScheduler = Scheduler.Create();
                JobHelper.RestartJob(pcScheduler._QtzScheduler, item, item);
                res.Status = true;
            }
            var jsonStr = JsonConvert.SerializeObject(res);
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

        #region 日志

        public dynamic ScheduleLog()
        {
            return View["ScheduleLog"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetScheduleLog(DateTime start, DateTime end)
        {
            var data = bll.ListScheduleJobLog(start, end);
            var jsonStr = JsonConvert.SerializeObject(data);
            return jsonStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic DelScheduleLog(DateTime start, DateTime end)
        {
            var res = bll.DeleteScheduleJobLog(start, end);
            var jsonStr = JsonConvert.SerializeObject(new ResultJson { Status = res });
            return jsonStr;
        }
        #endregion

    }
}

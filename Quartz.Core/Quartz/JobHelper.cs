using System;
using System.Collections.Generic;
using IService;
using Model;
using Quartz;
namespace Quartz.Core.Quartz
{
    public class JobHelper
    {
        #region 声明
        public static readonly string JOBNameFormat = "JOB-{0}-{1}-{2}-{3}";
        public static readonly string TRINameFormat = "TRIGGER-{0}-{1}-{2}-{3}";
        public static readonly string jobDetailMad = "jobDetail";
        public static readonly string triggerMad = "jobTrigger";
        public static IQuartzSchedule _iSchedule = IOC.ObjectContainer.Current.Resolve<IQuartzSchedule>();

        #endregion
        /// <summary>
        /// 根据采集计划产生作业的唯一标识
        /// </summary>
        /// <param name="jobDetail">采集计划</param>
        /// <param name="clMethod">采集方式</param>
        /// <returns></returns>
        public static JobKey GetJobKey(ScheduleJob_Details jobDetail)
        {
            return new JobKey(
                string.Format(JOBNameFormat, jobDetail.sched_name, jobDetail.job_name, jobDetail.job_group, jobDetail.is_durable), jobDetail.job_group);
        }

        /// <summary>
        /// 根据采集计划产生触发器的唯一标识
        /// </summary>
        /// <param name="trigger">采集计划</param>
        /// <param name="clMethod">采集方式</param>
        /// <returns></returns>
        public static TriggerKey GetTriggerKey(ScheduleJob_Details_Triggers trigger)
        {
            return new TriggerKey(
                string.Format(TRINameFormat, trigger.sched_name, trigger.job_name, trigger.trigger_name, trigger.trigger_group), trigger.trigger_group);
        }

        /// <summary>
        /// 使用采集计划来创建作业
        /// </summary>
        /// <param name="qtzScheduler">调度器</param>
        /// <param name="jobDetail">采集计划</param>
        /// <returns>二元组</returns>
        public static Tuple<IJobDetail, List<ITrigger>> ScheduleJobByPlan(IScheduler qtzScheduler, ScheduleJob_Details jobDetail)
        {
            Tuple<IJobDetail, List<ITrigger>> tuple = null;
            List<ITrigger> triggerList = new List<ITrigger>();
            if (jobDetail.is_durable)
            {
                IJobDetail ij = CreateJobDetail(jobDetail);
                var jobTriggers = _iSchedule.ListScheduleDetailsTriggers(jobDetail.sched_name, jobDetail.job_name);
                foreach (var trigger in jobTriggers)
                {
                    ITrigger ig = CreateTrigger(trigger);
                    qtzScheduler.ScheduleJob(ij, ig);
                    triggerList.Add(ig);
                }
                tuple = new Tuple<IJobDetail, List<ITrigger>>(ij, triggerList);
            }
            return tuple;
        }

        /// <summary>
        /// 根据数据采集计划来创建JOB明细
        /// </summary>
        /// <param name="jobDetail">采集计划</param>
        /// <param name="clMethod">数据采集方法</param>
        /// <param name="attachMap">附加的数据</param>
        /// <returns></returns>
        public static IJobDetail CreateJobDetail(ScheduleJob_Details jobDetail, IDictionary<string, object> attachMap = null)
        {
            if (jobDetail == null) throw new ArgumentNullException("『CreateJobDetail』的jobDetail参数为空！");
            Type jobType = Type.GetType(jobDetail.job_class_name);
            if (jobType == null)
            {
                throw new NotImplementedException(
                    string.Format("{0}调用的类型『{1}』未实现！", jobDetail.job_name, jobDetail.job_class_name));
            }
            var jobTrigger = _iSchedule.ListScheduleDetailsTriggers(jobDetail.sched_name, jobDetail.job_name);
            //作业执行上下文携带数据
            IDictionary<string, object> dataMap = new Dictionary<string, object>() { { jobDetailMad, jobDetail }, { triggerMad, jobTrigger } };
            if (attachMap != null)
            {
                foreach (KeyValuePair<string, object> kv in attachMap)
                {
                    if (!dataMap.ContainsKey(kv.Key)) dataMap.Add(kv);
                }
            }
            IJobDetail job =
                JobBuilder
                .Create(jobType)
                .WithDescription(jobDetail.description)
                .WithIdentity(GetJobKey(jobDetail))
                .UsingJobData(new JobDataMap(dataMap))
                .Build();
            return job;
        }

        /// <summary>
        /// 根据数据采集计划来创建作业触发器
        /// </summary>
        /// <param name="jobDetail">采集计划</param>
        /// <param name="clMethod">数据采集方法</param>
        /// <param name="forJob">将此触发器添加到哪个作业</param>
        /// <returns></returns>
        public static ITrigger CreateTrigger(ScheduleJob_Details_Triggers trigger, IJobDetail forJob = null)
        {
            if (trigger == null) throw new ArgumentNullException("『CreateTrigger』的trigger参数为空！");
            TriggerBuilder builder = TriggerBuilder.Create();
            builder =
                builder
                .WithIdentity(GetTriggerKey(trigger))
                .WithDescription(trigger.description);
            builder =
                trigger.trigger_type.ToUpper() == "cron".ToUpper() ? (
                    string.IsNullOrEmpty(trigger.cronexpression) ?
                    builder.WithSimpleSchedule(x => x.WithIntervalInMinutes(string.IsNullOrEmpty(trigger.repeat_interval) ? 30 : int.Parse(trigger.repeat_interval)).WithRepeatCount(string.IsNullOrEmpty(trigger.repeat_count) ? 10 : int.Parse(trigger.repeat_count)))
                    : builder.WithCronSchedule(trigger.cronexpression)
                ) : builder.WithSimpleSchedule(x => x.WithIntervalInMinutes(string.IsNullOrEmpty(trigger.repeat_interval) ? 30 : int.Parse(trigger.repeat_interval)).WithRepeatCount(string.IsNullOrEmpty(trigger.repeat_count) ? 10 : int.Parse(trigger.repeat_count)));
            if (forJob != null) builder.ForJob(forJob);
            return builder.Build();
        }

        /// <summary>
        /// 当采集计划各属性发生改变时，需要重新启动此作业
        /// </summary>
        /// <param name="qtzScheduler">调度器</param>
        /// <param name="jobDetailOld">旧的采集计划</param>
        /// <param name="jobDetailNew">新采集计划</param>
        /// <param name="clMethod">采集方法</param>
        /// <returns></returns>
        public static Tuple<IJobDetail, List<ITrigger>> RestartJob(IScheduler qtzScheduler, ScheduleJob_Details jobDetailOld, ScheduleJob_Details jobDetailNew)
        {
            //采集计划属性发生变更，则从调度器中删除此作业
            JobKey jobKey = GetJobKey(jobDetailOld);
            IJobDetail ij = CreateJobDetail(jobDetailNew);
            List<ITrigger> triggerList = new List<ITrigger>();
            qtzScheduler.DeleteJob(jobKey); //删除旧的作业
            var jobTriggers = _iSchedule.ListScheduleDetailsTriggers(jobDetailNew.sched_name, jobDetailNew.job_name);
            foreach (var trigger in jobTriggers)
            {
                ITrigger ig = CreateTrigger(trigger, ij);
                qtzScheduler.ScheduleJob(ij, ig);//调度新的作业
                //创建一个立即执行的触发器
                ig = JobHelper.CreateOnceTrigger(58, ij);
                qtzScheduler.ScheduleJob(ig);
                triggerList.Add(ig);
            }
            return new Tuple<IJobDetail, List<ITrigger>>(ij, triggerList);
        }

        /// <summary>
        /// 创建只触发一次的触发器
        /// </summary>
        /// <param name="secondBase">时间</param>
        /// <param name="forJob">此触发器将关联到哪一个作业</param>
        /// <returns></returns>
        public static ISimpleTrigger CreateOnceTrigger(int secondBase, IJobDetail forJob = null)
        {
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, secondBase);
            TriggerBuilder builder =
                TriggerBuilder.Create()
                .WithIdentity(System.Guid.NewGuid().ToString())
                .WithDescription("此触发器只执行一次.")
                .StartAt(startTime);
            if (forJob != null) builder = builder.ForJob(forJob);
            return (ISimpleTrigger)builder.Build();
        }
    }
}

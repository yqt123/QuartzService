using IService;
using Model;
using Quartz;
using Quartz.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Job
{
    public class BaseJob : BaseService, IBaseJob
    {
        public static readonly string jobDetailMad = "jobDetail";
        public static readonly string triggerMad = "jobTrigger";
        public static readonly string jobHelperMad = "JobHelper";
        public readonly string ExecResult = "ExecResult";
        public ScheduleJob_Details jobDetail { get; private set; }
        public IEnumerable<ScheduleJob_Details_Triggers> jobDetailTrigger { get; private set; }
        public string JobName { get; set; }
        QuartzSchedule schedulebll = new QuartzSchedule();
        /// <summary>
        /// 本次执行查询截止时间
        /// </summary>
        public DateTime ExeEndQueryTime { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                ExeEndQueryTime = DateTime.Now;
                this.jobDetail = context.MergedJobDataMap[jobDetailMad] as ScheduleJob_Details;
                this.jobDetailTrigger = context.MergedJobDataMap[triggerMad] as IEnumerable<ScheduleJob_Details_Triggers>;
                if (this.jobDetail == null)
                {
                    context.Put(this.ExecResult, "取不到作业计划");
                    throw new Exception(string.Format("【{0}】的[Execute]从[IJobExecutionContext]读取不到作业计划信息，本次执行失败！", this.JobName));
                }
                Log4.Info(string.Format("【{0}】开始执行IJOB的[Execute]...", this.JobName));

                ScheduleJob_Details jobDetailNew = schedulebll.GetScheduleDetail(jobDetail.id);
                IEnumerable<ScheduleJob_Details_Triggers> jobDetailTriggerNew = schedulebll.ListScheduleDetailsTriggers(jobDetail.sched_name, jobDetail.job_name);
                //检查新查询出来的计划是否能执行
                if (jobDetailNew == null || jobDetailTriggerNew == null || jobDetailTriggerNew.Count() == 0)
                {
                    context.Scheduler.DeleteJob(context.JobDetail.Key);
                    context.Put(this.ExecResult, "完成");
                    throw new Exception(string.Format("【{0}】作业计划为空，该记录可能已经被删除。", this.jobDetail.sched_name));
                }
                if (!jobDetailNew.is_durable)
                {
                    Log4.Info(string.Format("【{0}】作业计划不允许使用，跳过此次执行。", this.jobDetail.description));
                    context.Put(this.ExecResult, "完成");
                    return Task.FromResult(true);
                }
                if (!jobDetailNew.scheEquals(jobDetail) || IsChangedTrigger(jobDetailTrigger, jobDetailTriggerNew))
                {
                    context.Scheduler.DeleteJob(context.JobDetail.Key);
                    Log4.Info(string.Format("【{0}】的作业计划属性已更改，将删除该计划的实现作业，然后重新创建一个作业。", this.jobDetail.description));
                    context.Put(this.ExecResult, "重新创建一个作业");

                    var JobHelper = context.MergedJobDataMap[jobHelperMad] as IJobHelper;
                    Tuple<IJobDetail, List<ITrigger>> tuple = JobHelper.RestartJob2(context.Scheduler, jobDetail, jobDetailNew);
                    Log4.Info(string.Format("【{0}】的重新创建一个作业完毕，[IJOB.Execute]退出。作业计划：{1}，作业：{2}，触发器：{3}，表达式：{4}。", this.jobDetail.description, jobDetailNew.sched_name, jobDetailNew.description, tuple.Item1.Key, tuple.Item1.Key.Name, tuple.Item2[0].Key.Name, ""));
                    return Task.FromResult(true);
                }
                //执行具体作业的业务逻辑
                _Execute(context);
                context.Put(this.ExecResult, "成功");
            }
            catch (Exception ex)
            {
                Log4.Info(string.Format("【{0}】执行作业失败，消息：{1}", JobName, ex.Message + ex.StackTrace));
            }
            finally
            {
                WirteScheduleLog(context);
            }
            return Task.FromResult(true);
        }

        private bool IsChangedTrigger(IEnumerable<ScheduleJob_Details_Triggers> oldTrigger, IEnumerable<ScheduleJob_Details_Triggers> newTrigger)
        {
            if (oldTrigger == null || oldTrigger.Count() == 0 || newTrigger == null || newTrigger.Count() == 0 || oldTrigger.Count() != newTrigger.Count())
                return true;
            foreach (var oldTriggerItem in oldTrigger)
            {
                var newTriggerItem = newTrigger.Where(n => n.sched_name == oldTriggerItem.sched_name && n.job_name == oldTriggerItem.job_name && n.trigger_name == oldTriggerItem.trigger_name);
                if (newTriggerItem == null || newTriggerItem.Count() == 0 || !oldTriggerItem.scheEquals(newTriggerItem.FirstOrDefault()))
                    return true;
            }
            return false;
        }

        protected virtual void _Execute(IJobExecutionContext context)
        {

        }

        /// <summary>
        /// 写执行日志到数据库
        /// </summary>
        /// <param name="context"></param>
        protected void WirteScheduleLog(IJobExecutionContext context)
        {
            string _result = string.Format("【{0}】执行完毕，执行结果：{1}", this.JobName, context.Get(this.ExecResult) != null ? context.Get(this.ExecResult).ToString() : "失败");
            Log4.Info(_result);
            if (jobDetail != null)
                schedulebll.SaveScheduleLog(new ScheduleJob_Log
                {
                    description = string.Format("【{0}】执行完毕，执行结果：{1}", jobDetail.description, context.Get(this.ExecResult) != null ? context.Get(this.ExecResult).ToString() : "失败"),
                    job_name = jobDetail.job_name,
                    sched_name = jobDetail.sched_name,
                    update_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    success = (context.Get(this.ExecResult) != null && context.Get(this.ExecResult).ToString() == "成功" ? true : false)
                });
        }

    }
}

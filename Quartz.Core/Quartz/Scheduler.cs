using IService;
using Model;
using Quartz;
using Quartz.Impl;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Quartz.Core.Quartz
{
    public class Scheduler
    {
        private static readonly object lockObj = new object(); //锁对象
        private static Scheduler _Scheduler = null;     //单例实现
        public IScheduler _QtzScheduler = null;        //调度器
        private IQuartzSchedule _iSchedule = IOC.ObjectContainer.Current.Resolve<IQuartzSchedule>();

        private Scheduler()
        {
            //初始化 Quartz 的作业调度器
            var properties = new NameValueCollection();
            var schedulerFactory = new StdSchedulerFactory(properties);
            this._QtzScheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            this.ScheduleJob();
        }

        /// <summary>
        /// 停止作业
        /// </summary>
        public void Shutdown(bool waitForJobsToComplete = true)
        {
            if (_QtzScheduler != null)
            {
                _QtzScheduler.Shutdown(waitForJobsToComplete);
            }
        }

        /// <summary>
        /// 重新开始所有的作业
        /// </summary>
        public void ResumeAll()
        {
            if (_QtzScheduler != null)
            {
                _QtzScheduler.ResumeAll();
            }
        }

        /// <summary>
        /// 启动调度器
        /// </summary>
        public void Start()
        {
            if (_QtzScheduler != null)
            {
                _QtzScheduler.Start();
            }
        }

        /// <summary>
        /// 暂停作业
        /// </summary>
        public void PauseAll()
        {
            if (_QtzScheduler != null)
            {
                _QtzScheduler.PauseAll();
            }
        }

        /// <summary>
        /// 单例实现
        /// </summary>
        /// <returns></returns>
        public static Scheduler Create()
        {
            Instance();
            return _Scheduler;
        }

        //开始安排作业
        private void ScheduleJob()
        {
            //取出所有的作业
            IEnumerable<ScheduleJob_Details> jobDetails = _iSchedule.ListScheduleDetails();
            if (jobDetails != null)
            {
                foreach (ScheduleJob_Details detail in jobDetails)
                    ScheduleJobByPlan(_QtzScheduler, detail);
            }
        }

        //使用采集计划来创建作业
        private void ScheduleJobByPlan(IScheduler sched, ScheduleJob_Details JobDetail)
        {
            JobHelper.ScheduleJobByPlan(_QtzScheduler, JobDetail);
        }

        //单例创建调度器
        private static void Instance()
        {
            if (_Scheduler == null)
            {
                lock (lockObj)
                {
                    if (_Scheduler == null)
                    {
                        _Scheduler = new Scheduler();
                    }
                }
            }
        }
    }
}

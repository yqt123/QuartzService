
namespace Model
{
    public class ScheduleJob_Details_Triggers
    {
        public int id { get; set; }
        /// <summary>
        /// 采集器名称
        /// </summary>
        public string sched_name { get; set; }
        /// <summary>
        /// 采集作业名称
        /// </summary>
        public string job_name { get; set; }
        /// <summary>
        /// 采集作业触发器
        /// </summary>
        public string trigger_name { get; set; }
        /// <summary>
        /// 采集作业触发器组别
        /// </summary>
        public string trigger_group { get; set; }
        /// <summary>
        /// 采集作业组别
        /// </summary>
        public string job_group { get; set; }
        /// <summary>
        /// 采集作业说明
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 采集时间规则设置
        /// </summary>
        public string cronexpression { get; set; }
        /// <summary>
        /// 重复次数
        /// </summary>
        public string repeat_count { get; set; }
        /// <summary>
        /// 重复时间间隔
        /// </summary>
        public string repeat_interval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 采集类型 如：cron
        /// </summary>
        public string trigger_type { get; set; }


        /// <summary>
        /// 比较值是否相等
        /// </summary>
        /// <param name="plNew"></param>
        /// <returns></returns>
        public bool scheEquals(object obj)
        {
            ScheduleJob_Details_Triggers jobDetailTrigger = obj as ScheduleJob_Details_Triggers;
            return jobDetailTrigger != null &&
                jobDetailTrigger.sched_name == sched_name &&
                jobDetailTrigger.trigger_name == trigger_name &&
                jobDetailTrigger.trigger_group == trigger_group &&
                jobDetailTrigger.job_name == job_name &&
                jobDetailTrigger.job_group == job_group &&
                jobDetailTrigger.description == description &&
                jobDetailTrigger.cronexpression == cronexpression &&
                jobDetailTrigger.repeat_count == repeat_count &&
                jobDetailTrigger.repeat_interval == repeat_interval &&
                jobDetailTrigger.startTime == startTime &&
                jobDetailTrigger.endTime == endTime &&
                jobDetailTrigger.trigger_type == trigger_type;
        }

    }
}

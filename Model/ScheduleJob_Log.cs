namespace Model
{
    public class ScheduleJob_Log
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public string sched_LogID { get; set; }
        /// <summary>
        /// 采集器名称
        /// </summary>
        public string sched_name { get; set; }
        /// <summary>
        /// 采集作业名称
        /// </summary>
        public string job_name { get; set; }
        /// <summary>
        /// 采集作业组别
        /// </summary>
        public string job_group { get; set; }
        /// <summary>
        /// 采集作业触发器
        /// </summary>
        public string trigger_name { get; set; }
        /// <summary>
        /// 采集作业触发器组别
        /// </summary>
        public string trigger_group { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string update_time { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
    }
}

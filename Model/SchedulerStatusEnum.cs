namespace Model
{
    public enum SchedulerStatusEnum
    {
        /// <summary>
        /// 未开始
        /// </summary>
        initial=0,
        /// <summary>
        /// 运行中
        /// </summary>
        running=1,
        /// <summary>
        /// 暂停
        /// </summary>
        pause=2,
        /// <summary>
        /// 已停止
        /// </summary>
        Shutdown=3,
    }
}

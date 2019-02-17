namespace Model
{
    public class ResultJson
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public dynamic Result { get; set; }
    }
}

using IService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Job
{
    public class TestJob : BaseJob
    {
        protected override void _Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;
            string msg = String.Format("SimpleJob says: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));
            Console.Out.WriteLineAsync(msg);
        }
    }
}

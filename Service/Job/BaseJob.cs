using IService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Job
{
    public class BaseJob : BaseService, IBaseJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            _Execute(context);
            return Task.FromResult(true);
        }

        protected virtual void _Execute(IJobExecutionContext context)
        {

        }
    }
}

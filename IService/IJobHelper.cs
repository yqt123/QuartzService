using Model;
using Quartz;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IJobHelper
    {
        Tuple<IJobDetail, List<ITrigger>> RestartJob2(IScheduler qtzScheduler, ScheduleJob_Details jobDetailOld, ScheduleJob_Details jobDetailNew);
    }
}

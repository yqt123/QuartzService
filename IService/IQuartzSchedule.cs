using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IQuartzSchedule : IService
    {
        IEnumerable<ScheduleJob_Details> ListScheduleDetails();

        IEnumerable<ScheduleJob_Details_Triggers> ListScheduleDetailsTriggers(string schedName, string jobName);
    }
}

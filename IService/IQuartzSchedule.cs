using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IQuartzSchedule : IService
    {
        IEnumerable<ScheduleJob_Details> ListScheduleDetails();

        IEnumerable<ScheduleJob_Details_Triggers> ListScheduleDetailsTriggers(string schedName, string jobName);

        ScheduleJob_Details GetScheduleDetails(int id);

        bool DeleteScheduleDetails(int id);

        bool SaveScheduleDetails(ScheduleJob_Details data);

        bool EditScheduleDetails(ScheduleJob_Details data);

        bool DeleteScheduleDetailsTriggers(int id);

        bool SaveScheduleDetailsTriggers(ScheduleJob_Details_Triggers data);

        bool EditScheduleDetailsTriggers(ScheduleJob_Details_Triggers data);

        bool SaveScheduleLog(ScheduleJob_Log data);

        IEnumerable<ScheduleJob_Log> ListScheduleLog();

        IEnumerable<ScheduleJob_Log> ListScheduleJobLog(DateTime startTime, DateTime endTime);

        bool DeleteScheduleJobLog(DateTime startTime, DateTime endTime);
    }
}

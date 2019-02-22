using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IQuartzSchedule : IService
    {
        IEnumerable<ScheduleJob_Details> ListScheduleDetails();

        ScheduleJob_Details GetScheduleDetail(int id);

        bool DeleteScheduleDetail(int id);

        bool SaveScheduleDetail(ScheduleJob_Details data);

        bool EditScheduleDetail(ScheduleJob_Details data);

        IEnumerable<ScheduleJob_Details_Triggers> ListTriggers();

        IEnumerable<ScheduleJob_Details_Triggers> ListScheduleDetailsTriggers(string schedName, string jobName);

        ScheduleJob_Details_Triggers ListScheduleDetailsTrigger(int id);

        bool DeleteScheduleDetailsTrigger(int id);

        bool SaveScheduleDetailsTrigger(ScheduleJob_Details_Triggers data);

        bool EditScheduleDetailsTrigger(ScheduleJob_Details_Triggers data);

        bool SaveScheduleLog(ScheduleJob_Log data);

        IEnumerable<ScheduleJob_Log> ListScheduleLog();

        IEnumerable<ScheduleJob_Log> ListScheduleJobLog(DateTime startTime, DateTime endTime);

        bool DeleteScheduleJobLog(DateTime startTime, DateTime endTime);
    }
}

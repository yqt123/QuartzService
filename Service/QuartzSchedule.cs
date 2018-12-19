using Dapper;
using IService;
using Model;
using System.Collections.Generic;
using System.Data;

namespace Service
{
    public class QuartzSchedule : BaseService, IQuartzSchedule
    {
        IEnumerable<ScheduleJob_Details> IQuartzSchedule.ListScheduleDetails()
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM scheduleJob_details;";
                return connection.Query<ScheduleJob_Details>(sql);
            }
        }

        IEnumerable<ScheduleJob_Details_Triggers> IQuartzSchedule.ListScheduleDetailsTriggers(string schedName, string jobName)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM scheduleJob_details_triggers ";
                sql += string.Format("where sched_name='{0}' and job_name='{1}';", schedName, jobName);
                return connection.Query<ScheduleJob_Details_Triggers>(sql);
            }
        }
    }
}

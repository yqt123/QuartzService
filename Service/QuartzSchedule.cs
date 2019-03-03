using Dapper;
using IService;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace Service
{
    public class QuartzSchedule : BaseService, IQuartzSchedule
    {
        public IEnumerable<ScheduleJob_Details> ListScheduleDetails()
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM scheduleJob_details;";
                return connection.Query<ScheduleJob_Details>(sql);
            }
        }

        public ScheduleJob_Details GetScheduleDetail(int id)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format("SELECT * FROM scheduleJob_details where id='{0}';", id);
                return connection.Query<ScheduleJob_Details>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteScheduleDetail(int id)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format("DELETE FROM scheduleJob_details where id='{0}';", id);
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns></returns>
        public bool SaveScheduleDetail(ScheduleJob_Details data)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format(
@"INSERT INTO ScheduleJob_Details(sched_name,job_name,job_group,outAssembly,job_class_name,is_durable,description,startTime,endTime,platformMonitoring)
VALUES( '{0}', '{1}', '{2}', '{3}', '{4}','{5}','{6}',{7},{8},{9}); ", data.sched_name, data.job_name, data.job_group, data.outAssembly, data.job_class_name, data.is_durable ? 1 : 0, data.description,
data.startTime == null ? "NULL" : data.startTime.ToString(),
data.endTime == null ? "NULL" : data.endTime.ToString(),
data.platformMonitoring ? 1 : 0);
                return connection.Execute(sql) > 0;
            }
        }

        public bool EditScheduleDetail(ScheduleJob_Details data)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format(
@"UPDATE ScheduleJob_Details set sched_name='{1}',job_name='{2}',job_group='{3}',outAssembly='{4}',job_class_name= '{5}',is_durable='{6}',description='{7}',startTime={8},endTime={9},platformMonitoring={10}
where id={0};", data.id, data.sched_name, data.job_name, data.job_group, data.outAssembly, data.job_class_name, data.is_durable ? 1 : 0, data.description,
data.startTime == null ? "NULL" : data.startTime.ToString(),
data.endTime == null ? "NULL" : data.endTime.ToString(),
data.platformMonitoring ? 1 : 0);
                return connection.Execute(sql) > 0;
            }
        }

        public IEnumerable<ScheduleJob_Details_Triggers> ListTriggers()
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM scheduleJob_details_triggers ";
                return connection.Query<ScheduleJob_Details_Triggers>(sql);
            }
        }

        public IEnumerable<ScheduleJob_Details_Triggers> ListScheduleDetailsTriggers(string schedName, string jobName)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM scheduleJob_details_triggers ";
                sql += string.Format("where sched_name='{0}' and job_name='{1}';", schedName, jobName);
                return connection.Query<ScheduleJob_Details_Triggers>(sql);
            }
        }

        public ScheduleJob_Details_Triggers ListScheduleDetailsTrigger(int id)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM scheduleJob_details_triggers ";
                sql += string.Format("where id='{0}';", id);
                return connection.Query<ScheduleJob_Details_Triggers>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteScheduleDetailsTrigger(int id)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format("DELETE FROM scheduleJob_details_triggers where id='{0}';", id);
                return connection.Execute(sql) > 0;
            }
        }

        public bool DeleteScheduleDetailsTriggers(string schedName, string jobName)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "DELETE FROM scheduleJob_details_triggers ";
                sql += string.Format("where sched_name='{0}' and job_name='{1}';", schedName, jobName);
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SaveScheduleDetailsTrigger(ScheduleJob_Details_Triggers data)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format(
@"INSERT INTO ScheduleJob_Details_Triggers(sched_name,job_name,trigger_name,trigger_group,job_group,cronexpression,trigger_type,repeat_count,repeat_interval,startTime,endTime)
VALUES('{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}','{8}','{9}',{10},{11}); ", data.id, data.sched_name, data.job_name, data.trigger_name, data.trigger_group, data.job_group, data.cronexpression, data.trigger_type, data.repeat_count, data.repeat_interval,
data.startTime == null ? "NULL" : data.startTime.ToString(),
data.endTime == null ? "NULL" : data.endTime.ToString()
);
                return connection.Execute(sql) > 0;
            }
        }
        public bool EditScheduleDetailsTrigger(ScheduleJob_Details_Triggers data)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format(
@"UPDATE ScheduleJob_Details_Triggers set sched_name='{1}',job_name='{2}',trigger_name='{3}',trigger_group='{4}',job_group='{5}',cronexpression='{6}',trigger_type='{7}',repeat_count='{8}',repeat_interval='{9}',startTime={10},endTime={11},description='{12}'
where id={0}; ", data.id, data.sched_name, data.job_name, data.trigger_name, data.trigger_group, data.job_group, data.cronexpression, data.trigger_type, data.repeat_count, data.repeat_interval,
data.startTime == null ? "NULL" : data.startTime.ToString(),
data.endTime == null ? "NULL" : data.endTime.ToString(),
data.description);
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns></returns>
        public bool SaveScheduleLog(ScheduleJob_Log data)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = string.Format(
@"INSERT INTO ScheduleJob_Log(sched_name,job_name,description,success,update_time)
VALUES('{0}', '{1}', '{2}', '{3}', '{4}'); ", data.sched_name, data.job_name, data.description, data.success ? 1 : 0, data.update_time);
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScheduleJob_Log> ListScheduleLog()
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                var sql = "SELECT * FROM ScheduleJob_Log;";
                return connection.Query<ScheduleJob_Log>(sql);
            }
        }

        /// <summary>
        /// 调度作业日志
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleJob_Log> ListScheduleJobLog(DateTime startTime, DateTime endTime)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                StringBuilder sqlsb = new StringBuilder();
                sqlsb.Append("SELECT * FROM ScheduleJob_Log ");
                sqlsb.Append(string.Format(" where update_time>'{0}' and update_time<'{1}' order by update_time desc", startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd 23:59:59")));
                return connection.Query<ScheduleJob_Log>(sqlsb.ToString());
            }
        }

        /// <summary>
        /// 调度作业日志
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool DeleteScheduleJobLog(DateTime startTime, DateTime endTime)
        {
            using (IDbConnection connection = SqlConnectionHelper.GetSQLiteConnection())
            {
                StringBuilder sqlsb = new StringBuilder();
                sqlsb.Append("Delete FROM ScheduleJob_Log ");
                sqlsb.Append(string.Format(" where update_time>'{0}' and update_time<'{1}'", startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd 23:59:59")));
                return connection.Execute(sqlsb.ToString()) > 0;
            }
        }

    }
}

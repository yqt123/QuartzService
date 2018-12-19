using System.Configuration;
using System.Reflection;

namespace Model
{
    public class SysConfig<T>
    {
        private static Assembly service = Assembly.GetAssembly(typeof(T));
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);

        static SysConfig()
        {
            ServiceName = config.AppSettings.Settings["ServiceName"].Value.ToString();
            ServiceDescription = config.AppSettings.Settings["ServiceDescription"].Value.ToString();
            OwinPort = config.AppSettings.Settings["OwinPort"].Value.ToString();
        }

        public static string ServiceName
        {
            get;
            private set;
        }

        public static string ServiceDescription
        {
            get;
            private set;
        }

        public static string OwinPort
        {
            get;
            private set;
        }
    }

    public class SysConfig
    {
        static SysConfig()
        {
            IocServiceName = ConfigurationManager.AppSettings.Get("IocServiceName");
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            if (ConfigurationManager.ConnectionStrings["MysqlConnectionString"] != null)
                MysqlConnectionString = ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;
            if (ConfigurationManager.ConnectionStrings["SQLiteConnectionString"] != null)
                SQLiteConnectionString = ConfigurationManager.ConnectionStrings["SQLiteConnectionString"].ConnectionString;
        }
        public static string OwinPort
        {
            get;
            private set;
        }

        public static string IocServiceName
        {
            get;
            private set;
        }

        public static string ConnectionString
        {
            get;
            private set;
        }

        public static string MysqlConnectionString
        {
            get;
            private set;
        }

        public static string SQLiteConnectionString
        {
            get;
            private set;
        }
    }

}

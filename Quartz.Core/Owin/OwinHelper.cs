using Microsoft.Owin.Hosting;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Core.Owin
{
    public class OwinHelper
    {
        private static readonly object lockObj = new object();  //锁对象
        private static OwinHelper _OwinHelper = null;           //单例实现
        private IDisposable _WebApp = null;        //站点
        private string _OwinPort = "9090";//默认端口
        private OwinHelper()
        {

        }

        /// <summary>
        /// 单例实现
        /// </summary>
        /// <returns></returns>
        public static OwinHelper Create(string owinPort = null)
        {
            if (_OwinHelper == null)
            {
                lock (lockObj)
                {
                    if (_OwinHelper == null)
                    {
                        _OwinHelper = new OwinHelper();
                        _OwinHelper._OwinPort = owinPort ?? _OwinHelper._OwinPort;
                    }
                }
            }
            return _OwinHelper;
        }

        /// <summary>
        /// 关掉站点
        /// </summary>
        public void Shutdown()
        {
            if (_WebApp != null)
            {
                _WebApp.Dispose();
                _WebApp = null;
            }
        }

        /// <summary>
        /// 启动站点
        /// </summary>
        public void Start()
        {
            if (_WebApp == null)
            {
                var url = "http://localhost:" + _OwinPort;
                _WebApp = WebApp.Start<Startup>(url);
            }
        }
    }
}

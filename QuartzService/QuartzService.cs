using Model;
using Quartz.Core.Owin;
using Quartz.Core.Quartz;
using System;
using System.ServiceProcess;

namespace QuartzService
{
    public partial class QuartzService : ServiceBase
    {
        private OwinHelper owinHelper;
        private Scheduler pcScheduler = null;

        public QuartzService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Quartz.Core.IOC.ObjectContainer.ApplicationStart(new Quartz.Core.IOC.AutoFacContainer());
                if (this.pcScheduler == null) pcScheduler = Scheduler.Create();
                pcScheduler.Start();

                owinHelper = OwinHelper.Create(SysConfig<ProjectInstaller>.OwinPort);
                owinHelper.Start();
            }
            catch (Exception ex)
            {
                Quartz.Core.Log4net.Log4.Error(ex.Message);
                Quartz.Core.Log4net.Log4.Error(ex.StackTrace);
            }
        }

        protected override void OnStop()
        {
            try
            {
                owinHelper.Shutdown();
                if (pcScheduler != null) pcScheduler.Shutdown();
            }
            catch (Exception ex) {
                Quartz.Core.Log4net.Log4.Error(ex.Message);
                Quartz.Core.Log4net.Log4.Error(ex.StackTrace);
            }
        }

        protected override void OnPause()
        {
            //服务暂停执行代码
            if (pcScheduler != null) pcScheduler.PauseAll();
        }

        protected override void OnContinue()
        {
            //服务恢复执行代码
            base.OnContinue();
            if (pcScheduler != null) pcScheduler.ResumeAll();
        }

        protected override void OnShutdown()
        {
            //系统即将关闭执行代码
            base.OnShutdown();
            if (pcScheduler != null) pcScheduler.Shutdown();
        }
    }
}

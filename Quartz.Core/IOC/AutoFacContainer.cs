using Autofac;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Core.IOC
{
    public class AutoFacContainer : IinjectContainer
    {
        #region 字段
        private ContainerBuilder builder;
        private IContainer container;
        #endregion

        #region 构造函数
        public AutoFacContainer()
        {
            builder = new ContainerBuilder();
            SetupResolveRules(builder);  //注入
            container = builder.Build();
        }
        #endregion

        #region IinjectContainer 成员
        public void RegisterType<T>()
        {
            builder.RegisterType<T>();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }
        #endregion

        #region 私有方法
        private void SetupResolveRules(ContainerBuilder builder)
        {
            var services = Assembly.Load(SysConfig.IocServiceName);
            builder.RegisterAssemblyTypes(services).Where(t => t.GetInterface(typeof(IService.IService).Name) != null).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
        #endregion
    }
}

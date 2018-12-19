namespace Quartz.Core.IOC
{
    /// <summary>
    /// 依赖注入接口
    /// </summary>
    public interface IinjectContainer
    {
        void RegisterType<T>();

        T Resolve<T>();
    }
}

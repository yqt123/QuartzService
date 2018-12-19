using Nancy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNancy.Modules
{
    public class BaseModule : NancyModule
    {
        //声明动态对象，用于控制器绑定数据传递到页面
        public dynamic DynamicModel = new ExpandoObject();

        public BaseModule()
        {
            SetupModelDefaults();
        }

        private void SetupModelDefaults()
        {
            Before += ctx =>
            {
                return null;
            };
        }
    }
}

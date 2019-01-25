using Nancy;
using Nancy.Helpers;
using QuartzWebNancy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzWebNancy.Modules
{
    public class ApiModule : BaseModule
    {
        public ApiModule()
        {
            Post["/nancy/api"] = parameters => ReturnStringAction();
        }

        public dynamic ReturnStringAction()
        {
            var json = GetParameters();
            return json;
        }
    }
}

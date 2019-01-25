using Nancy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzWebNancy.Modules
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

        protected string GetParameters()
        {
            if (Request.Body.Length == 0)
                return "";
            if (Request.Headers.ContentType.Contains("multipart/form-data"))
            {
                string args = string.Empty;
                if (Request.Form["formJson"] != null)
                {
                    args = Request.Form["formJson"].ToString();
                }
                else
                {
                    args = "{";
                    for (int n = 0; n < Request.Form.Count; n++)
                    {
                        string name = Request.Form.Keys[n].ToString();
                        string value = "'" + Request.Form[n].ToString() + "'";
                        if (name == "ResquestModel" && !string.IsNullOrEmpty(Request.Form[n].ToString()))
                        {
                            value = Request.Form[n].ToString();
                        }
                        if (n == Request.Form.Count - 1)
                            args += string.Format("'{0}':{1}", name, value);
                        else
                            args += string.Format("'{0}':{1},", name, value);
                    }
                    args += "}";
                }
                return args;
            }

            byte[] byts = new byte[Request.Body.Length];
            Request.Body.Read(byts, 0, byts.Length);
            return System.Text.Encoding.UTF8.GetString(byts);
        }
    }
}

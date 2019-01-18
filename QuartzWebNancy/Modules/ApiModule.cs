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

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
    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            //主页
            Get["/nancy/Home"] = parameters => {
                return Response.AsRedirect("/nancy/Home/index");
            };

            //首页
            Get["/nancy/Home/index"] = parameters => ReturnHomeAction();

            //普通参数
            //Get["/nancy/{para}"] = parameters => ReturnHomeAction(parameters.para);

            //限制 int 类型的参数
            //Get["/nancy/{para:int}"] = parameters => ReturnHomeAction(parameters.para);

            //限制 Email 类型的参数
            //Get["/nancy/{value:email}"] = parameters => ReturnHomeAction(parameters.para);

            //限制指定正则类型的参数
            Get[@"/nancy/(?<para>[\d]{1,2})"] = parameters => ReturnHomeAction(parameters.para);

            //返回字符串演示
            Get["/nancy/getStringValue"] = parameters => ReturnStringAction();

            //重定向页面演示
            Get["/nancy/redirectOtherPage"] = parameters => ReturnRedirectAction();
        }

        public dynamic ReturnHomeAction(string para)
        {
            DynamicModel.Para = para;
            //单一数值
            DynamicModel.HelloWorld = "Hello world...";
            //集合数据 1
            List<string> list1 = new List<string>() { "listValue_1", "listValue_2", "listValue_3", "listValue_4" };
            //集合数据 2 
            List<TestClass> list2 = new List<TestClass>() {
                new TestClass("1","张三"),
                new TestClass("2","李四"),
                new TestClass("3","王五")
            };
            DynamicModel.List1 = list1;
            DynamicModel.List2 = list2;
            return View["Index", DynamicModel];
        }

        public dynamic ReturnHomeAction()
        {
            //单一数值
            DynamicModel.HelloWorld = "Hello world...";
            //集合数据 1
            List<string> list1 = new List<string>() { "listValue_1", "listValue_2", "listValue_3", "listValue_4" };
            //集合数据 2 
            List<TestClass> list2 = new List<TestClass>() {
                new TestClass("1","张三"),
                new TestClass("2","李四"),
                new TestClass("3","王五")
            };
            DynamicModel.List1 = list1;
            DynamicModel.List2 = list2;
            return View["Index", DynamicModel];
        }

        public dynamic ReturnStringAction()
        {
            return "这里一般是一个json串，常用于ajax异步处理，返回json串后页面解析操作等";
        }

        public dynamic ReturnRedirectAction()
        {
            //重定向跳转 1==1 模拟判断用户登录状态有效
            if (1 == 1)
            {
                //当前用户登录状态有效
            }
            else
            {
                //跳转到登录页面
                return Response.AsRedirect("/login");
            }
            return null;
        }

        public dynamic ReturnFileAction()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Content\UpFile\使用说明.docx";
            if (!File.Exists(path))
            {
                return Response.AsJson("文件不存在,可能已经被删除！");
            }
            var msbyte = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    sr.BaseStream.CopyTo(memstream);
                }
                msbyte = memstream.ToArray();
            }

            return new Response()
            {
                Contents = stream => { stream.Write(msbyte, 0, msbyte.Length); },
                ContentType = "application/msword",
                StatusCode = HttpStatusCode.OK,
                Headers = new Dictionary<string, string> {
                        { "Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(Path.GetFileName(path))) },
                        {"Content-Length",  msbyte.Length.ToString()}
                    }
            };
        }
    }
}

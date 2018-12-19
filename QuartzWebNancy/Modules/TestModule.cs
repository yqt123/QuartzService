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
    public class TestModule : BaseModule
    {
        public TestModule()
        {
            //首页
            Get["/nancy/Test/index"] = parameters => ReturnHomeAction();
            Get["/nancy/Test/LayoutTest"] = parameters => ReturnLayoutTestAction();
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

        public dynamic ReturnLayoutTestAction()
        {
            DynamicModel.Title = "";
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
    }
}

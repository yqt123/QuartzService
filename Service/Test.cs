using IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Test : BaseService, ITest
    {
        public void SayHello()
        {
            Console.Write("hello !");
        }
    }
}

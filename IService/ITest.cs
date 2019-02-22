using System;

namespace IService
{
    public interface ITest : IService
    {
        void SayHello(string say);
    }
}

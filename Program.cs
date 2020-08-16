using System;

namespace czogovuaWraper
{
    class Program
    {
        static void Main(string[] args)
        {
            CzoGovUaWraper v = new CzoGovUaWraper();
            v.Init(10000);
            v.SingFile("C:\\Users\\andrii\\Desktop\\AndriiNetCore.pdf", "C:\\Users\\andrii\\Desktop\\pb_3356803132.jks", "mak27arr");
            Console.WriteLine("Hello World!");
            Console.ReadKey();
            v.ShutDown();
        }
    }
}

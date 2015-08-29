using System;

namespace SiteMock.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = Console.WindowWidth + 90;
            using (var site = new Site("http://*:8888/"))
            {
                Console.Read();
            }
        }
    }
}

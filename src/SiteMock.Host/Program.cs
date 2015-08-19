using System;

namespace SiteMock.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = Console.LargestWindowWidth;
            using (var site = new Site("http://localhost:8888/"))
            {
                Console.Read();
            }
        }
    }
}

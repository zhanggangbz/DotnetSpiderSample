using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var spider = new CnBlog.CnblogSpider();
            //var spider = new JDMiaoSha.JDMiaoShaSpider();

            try
            {
                var spider = new O5I5JSecondHouse.O5I5JSpider();

                while (true)
                {
                    spider.Run();
                    Console.WriteLine("End : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    System.Threading.Thread.Sleep(1000*60*60*5);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}

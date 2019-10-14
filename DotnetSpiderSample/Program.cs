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
                while (true)
                {
                    if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 22)
                    {
                        using (var spider = new LianJiaSecondHouse.LianJiaSpider())
                        //using (var spider = new O5I5JSecondHouse.O5I5JSpider())
                        {
                            //spider.StartGetCookie();
                            spider.Run();
                            //spider.StopGetCookie();
                        }

                        Console.WriteLine("End : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        System.Threading.Thread.Sleep(1000 * 60 * 60 * 4);
                    }
                    else
                    {
                        Console.WriteLine("现在是晚上");
                        System.Threading.Thread.Sleep(1000 * 60 * 60);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }
    }
}

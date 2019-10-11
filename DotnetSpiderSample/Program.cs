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

            var spider = new O5I5JSecondHouse.O5I5JSpider();

            spider.Run();

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            Console.ReadKey();
        }
    }
}

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
            var spider = new JDMiaoSha.JDMiaoShaSpider();

            spider.Run();

            Console.ReadKey();
        }
    }
}

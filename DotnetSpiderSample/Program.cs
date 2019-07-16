using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample
{
    class Program
    {
        public static string MySqlConnectStr = "Database='mysql';Data Source=192.168.0.179;User ID=root;Password=root;Port=3306;SslMode=None;Allow User Variables=True";
        static void Main(string[] args)
        {
            var spider = new CnBlog.CnblogSpider();

            spider.Run();

            Console.ReadKey();
        }
    }
}

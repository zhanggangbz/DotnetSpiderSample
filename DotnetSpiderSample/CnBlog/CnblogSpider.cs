using DotnetSpider.Core;
using DotnetSpider.Downloader;
using DotnetSpider.Extension.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.CnBlog
{
    class CnblogSpider : Spider
    {
        protected override void OnInit(params string[] arguments)
        {
            ///加入前五页的列表页
            for (int i = 1; i < 5; i++)
            {
                AddRequest(new Request(string.Format("https://www.cnblogs.com/p{0}", i)));
            }
            
            ///在页面没有解析出任何内容的情况下，依旧处理其获取到的目标连接
            SkipTargetRequestsWhenResultIsEmpty = false;
            //爬取每个请求的间隔时间
            SleepTime = 2000;

            //页面内容分析器
            AddPageProcessor(new CnblogPageProcessor());

            //数据存储方式，使用数据库存储，并在数据存在的时候更新旧数据
            AddPipeline(new MySqlEntityPipeline(Program.MySqlConnectStr, PipelineMode.InsertNewAndUpdateOld));
        }
    }
}

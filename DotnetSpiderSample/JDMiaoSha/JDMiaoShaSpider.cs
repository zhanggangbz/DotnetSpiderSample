using DotnetSpider.Core;
using DotnetSpider.Downloader;
using DotnetSpider.Extension.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.JDMiaoSha
{
    class JDMiaoShaSpider : Spider
    {
        protected override void OnInit(params string[] arguments)
        {
            AddHeaders("ai.jd.com", new Dictionary<string, object> {
                    { "Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" },
                    { "Referer", "https://miaosha.jd.com/"}
                });

            var startRequest = new Request(string.Format("https://ai.jd.com/index_new?app=Seckill&action=pcMiaoShaAreaList&callback=pcMiaoShaAreaList&_={0}", Common.BaseFunction.GetTimeStamp()));

            AddRequest(startRequest);

            ///在页面没有解析出任何内容的情况下，依旧处理其获取到的目标连接
            SkipTargetRequestsWhenResultIsEmpty = false;
            //爬取每个请求的间隔时间
            SleepTime = 2000;

            //页面内容分析器
            AddPageProcessor(new JDMiaoShaPageProcessor());

            //数据存储方式，使用数据库存储，并在数据存在的时候更新旧数据
            AddPipeline(new MySqlEntityPipeline(Common.BaseValue.MySqlConnectStr, PipelineMode.InsertNewAndUpdateOld));
        }
    }
}

using DotnetSpider.Core;
using DotnetSpider.Downloader;
using DotnetSpider.Extension.Pipeline;
using DotnetSpiderSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.O5I5JSecondHouse
{
    class O5I5JSpider : Spider
    {
        protected override void OnInit(params string[] arguments)
        {
            ///加入前五页的列表页
            InitPageWC();

            ///在页面没有解析出任何内容的情况下，依旧处理其获取到的目标连接
            SkipTargetRequestsWhenResultIsEmpty = false;
            //爬取每个请求的间隔时间
            SleepTime = 5000;

            //页面内容分析器
            AddPageProcessor(new O5I5JPageProcessor());

            //数据存储方式，使用数据库存储，并在数据存在的时候更新旧数据
            AddPipeline(new MySqlEntityPipeline(Common.BaseValue.MySqlConnectStr, PipelineMode.InsertNewAndUpdateOld));
        }

        private void InitPageWC()
        {
            for (int i = 1; i < 3; i++)
            {
                var request = BaseFunction.CreateRequest(string.Format("https://hz.5i5j.com/xiaoqu/wuchang/n{0}/", i));
                request.AddProperty("diqu", "五常");
                AddRequest(request);
            }
            for (int i = 1; i < 4; i++)
            {
                var request = BaseFunction.CreateRequest(string.Format("https://hz.5i5j.com/xiaoqu/xianlin/n{0}/", i));
                request.AddProperty("diqu", "闲林");
                AddRequest(request);
            }
            Dictionary<string, object> properties3 = new Dictionary<string, object>();
            properties3.Add("diqu", "西溪");
            for (int i = 1; i < 2; i++)
            {
                var request = BaseFunction.CreateRequest(string.Format("https://hz.5i5j.com/xiaoqu/xixi/n{0}/", i));
                request.AddProperty("diqu", "西溪");
                AddRequest(request);
            }
        }
    }
}

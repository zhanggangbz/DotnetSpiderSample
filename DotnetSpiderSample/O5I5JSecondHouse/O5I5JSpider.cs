using DotnetSpider.Core;
using DotnetSpider.Downloader;
using DotnetSpider.Extension.Pipeline;
using DotnetSpiderSample.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DotnetSpiderSample.O5I5JSecondHouse
{
    class O5I5JSpider : Spider
    {
        Timer timer = new Timer(1000*60*10);

        protected override void OnInit(params string[] arguments)
        {
            timer.Elapsed += Timer_Elapsed;

            ///加入前五页的列表页
            InitPageWC();

            this.Downloader.AddBeforeDownloadHandler(new BeforeRequestAddCookie());

            ///在页面没有解析出任何内容的情况下，依旧处理其获取到的目标连接
            SkipTargetRequestsWhenResultIsEmpty = false;
            //爬取每个请求的间隔时间
            SleepTime = 20000;

            //页面内容分析器
            AddPageProcessor(new O5I5JPageProcessor());

            //数据存储方式，使用数据库存储，并在数据存在的时候更新旧数据
            AddPipeline(new MySqlEntityPipeline(Common.BaseValue.MySqlConnectStr, PipelineMode.InsertNewAndUpdateOld));
        }

        public void StartGetCookie()
        {
            GetCookie();

            timer.Start();
        }

        public void StopGetCookie()
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetCookie();
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

        public static string CookieString = "";

        private void GetCookie()
        {
            try
            {
                Logger?.LogInformation("开始获取Cookie");

                var httpClient = new BaseUtil.HttpHelper();

                var httpItem = new BaseUtil.HttpItem()
                {
                    URL = "https://hz.5i5j.com/",
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36",
                    Method = "get"
                };

                var result = httpClient.GetHtml(httpItem);

                if (result.StatusCode == System.Net.HttpStatusCode.Redirect)
                {
                    CookieString = result.Cookie;

                    Logger?.LogInformation($"获取Cookie成功,Cookie is {CookieString}");
                }
                else
                {
                    Logger?.LogInformation("获取Cookie失败");
                }
            }
            catch(Exception ex)
            {
                Logger?.LogError(ex.Message);
                Logger?.LogError(ex.StackTrace);
            }
        }
    }
}

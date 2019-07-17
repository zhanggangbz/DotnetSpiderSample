using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Downloader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.JDMiaoSha
{
    class JDMiaoShaPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            string jsonStr = page.Content.ToString().Replace("pcMiaoShaAreaList(","").TrimEnd(';').TrimEnd(')');
            JObject jObj = JObject.Parse(jsonStr);

            if (!page.TargetUrl.Contains("&gid="))
            {
                if (jObj.ContainsKey("groups") && jObj.ContainsKey("gid"))
                {
                    JArray array = jObj.Value<JArray>("groups");
                    foreach(JObject item in array)
                    {
                        if (item != null && item.ContainsKey("gid") && !item["gid"].ToString().Equals(jObj["gid"].ToString()))
                        {
                            string url = string.Format("https://ai.jd.com/index_new?app=Seckill&action=pcMiaoShaAreaList&callback=pcMiaoShaAreaList&gid={0}&_={1}",
                                item["gid"].ToString(),
                                Common.BaseFunction.GetTimeStamp());

                            var request = new Request(url);
                            request.Referer = "https://miaosha.jd.com/";

                            page.AddTargetRequest(request);
                        }
                    }
                }
            }

            if (jObj.ContainsKey("miaoShaList"))
            {
                var list = new List<JDMiaoShaEntity>();
                foreach (JObject item in jObj.Value<JArray>("miaoShaList"))
                {
                    try
                    {
                        JDMiaoShaEntity data = new JDMiaoShaEntity();
                        data.Id = item.Value<string>("wareId");
                        data.Name = item.Value<string>("wname");
                        data.ShortName = item.Value<string>("shortWname");
                        data.ImageUrl = item.Value<string>("imageurl");
                        data.JDPrice = item.Value<decimal>("jdPrice");
                        data.MiaoShaPrice = item.Value<decimal>("miaoShaPrice");
                        data.LowestPriceDaysInfo = item.Value<string>("lowestPriceDaysInfo");
                        //data.StartTime = Common.BaseFunction.GetTimeFromTimeStamp(item.Value<long>("startTimeMills"));
                        data.StartTime = item.Value<long>("startTimeMills");

                        list.Add(data);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                page.AddResultItem("Result", list);
            }
        }
    }
}

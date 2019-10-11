using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Downloader;
using DotnetSpider.Extraction;
using DotnetSpiderSample.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotnetSpiderSample.O5I5JSecondHouse
{
    class O5I5JPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            ///如果是小区列表页面，则从列表中获取内容页的地址加入到后续目标中
            if (Regex.IsMatch(page.TargetUrl, "https://hz.5i5j.com/xiaoqu/[\\s\\S]+"))
            {
                //获取小区二手房的链接，并加入列表
                var totalCnblogElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='list-con-box']/ul[@class='pList']/li/div[@class='listCon']/div[@class='listX']/div[@class='jia']/a")).Links().GetValues();
                foreach (var cnblogElement in totalCnblogElements)
                {
                    page.AddTargetRequest(BaseFunction.CreateRequest(cnblogElement));
                }
                Logger?.LogInformation($"{page.TargetUrl}页面获取到小区连接{ totalCnblogElements.ToList().Count }个");

                //获取每个小区的信息
                var totalXiaoQuElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='list-con-box']/ul[@class='pList']/li")).Nodes();

                var xiaoquList = new List<O5I5JXiaoQuEntity>();
                var xiaoquPriceList = new List<O5I5JXiaoQuPriceEntity>();
                foreach (var xiaoquElement in totalXiaoQuElements)
                {
                    try
                    {
                        var xiaoqu = new O5I5JXiaoQuEntity();
                        xiaoqu.Name = xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/h3/a/text()")).GetValue();
                        xiaoqu.Url = xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/h3/a/@href")).GetValue();
                        xiaoqu.Describe = xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/div/p[2]")).GetValue().Trim();

                        string[] tempDesc = xiaoqu.Describe.Split('>');
                        if(tempDesc.Length == 3)
                        {
                            xiaoqu.Describe = tempDesc[2].Trim();
                        }

                        xiaoqu.Region = page.Request.Properties["diqu"];
                        xiaoqu.Id = xiaoqu.Url.Substring(xiaoqu.Url.LastIndexOf('/') + 1, xiaoqu.Url.LastIndexOf('.') - xiaoqu.Url.LastIndexOf('/') - 1);
                        xiaoqu.Url = xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/div/div/a/@href")).GetValue();
                        xiaoquList.Add(xiaoqu);

                        var xiaoquPrice = new O5I5JXiaoQuPriceEntity();
                        xiaoquPrice.Id = xiaoqu.Id;
                        xiaoquPrice.Date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);//DateTime.Now.ToString("yyyy-MM-dd");
                        xiaoquPrice.Price = BaseFunction.TryParseDecimal(xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/div/div/p[@class='redC']/strong/text()")).GetValue());
                        xiaoquPrice.PriceRange = xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/div/div/p[2]/text()")).GetValue();
                        xiaoquPrice.OnSellCount = BaseFunction.TryParseInt(xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/div/div/a/p/span/text()")).GetValue());
                        xiaoquPriceList.Add(xiaoquPrice);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                Logger?.LogInformation($"{page.TargetUrl}页面获取到小区信息{xiaoquList.Count}个，获取到小区价格信息{xiaoquPriceList.Count}个");

                page.AddResultItem("Result", xiaoquList);
                page.AddResultItem("Result1", xiaoquPriceList);

            }
            ///如果是房屋列表页
            else if (Regex.IsMatch(page.TargetUrl, "https://hz.5i5j.com/xq-ershoufang/[\\s\\S]+"))
            {
                //第一页的时候，获取翻页列表
                try
                {
                    if (page.TargetUrl.Length - page.TargetUrl.LastIndexOf('n') > 4)
                    {
                        var pageElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='pageBox']/div[@class='pageSty rf']/a")).Nodes().ToList();

                        if (pageElements != null && pageElements.Count > 2)
                        {
                            for (int i = 1; i < pageElements.Count - 1; ++i)
                            {
                                page.AddTargetRequest(BaseFunction.CreateRequest(pageElements[i].Links().GetValue()));
                            }
                        }

                        Logger?.LogInformation($"{page.TargetUrl}页面获取分页信息{pageElements.Count-2}个");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }

                //获取每个房子的信息
                var totalFangZiElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='list-con-box'][1]/ul/li/div[@class='listCon']")).Nodes();

                var houseList = new List<O5I5JHouseEntity>();
                var housePriceList = new List<O5I5JHousePriceEntity>();
                foreach (var fangElement in totalFangZiElements)
                {
                    try
                    {
                        var house = new O5I5JHouseEntity();
                        house.Title = fangElement.Select(Selectors.XPath(".//h3[@class='listTit']/a/text()")).GetValue();
                        house.Url = fangElement.Select(Selectors.XPath(".//h3[@class='listTit']/a/@href")).GetValue();
                        house.Id = house.Url.Substring(house.Url.LastIndexOf('/') + 1, house.Url.LastIndexOf('.') - house.Url.LastIndexOf('/') - 1);

                        string huxmj = fangElement.Select(Selectors.XPath(".//div[@class='listX']/p[1]")).GetValue();
                        string[] tt = huxmj.Split('·');
                        if (tt.Length > 3)
                        {
                            house.MianJi = BaseFunction.TryParseDecimal(tt[1].Replace("平", "").Replace("米", "").Trim());
                            string[] thx = tt[0].Split('>');
                            if (thx.Length == 3)
                            {
                                house.HuXing = thx[2];
                            }
                        }

                        huxmj = fangElement.Select(Selectors.XPath(".//div[@class='listX']/p[3]")).GetValue();
                        tt = huxmj.Split('·');
                        if (tt.Length > 2)
                        {
                            house.PublicTime = BaseFunction.TryParseDateTime(tt[2].Replace("发", "").Replace("布", "").Trim());
                        }
                        houseList.Add(house);


                        var housePrice = new O5I5JHousePriceEntity();
                        housePrice.Id = house.Id;
                        housePrice.Date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);//DateTime.Now.ToString("yyyy-MM-dd");
                        housePrice.Price = BaseFunction.TryParseDecimal(fangElement.Select(Selectors.XPath(".//div[@class='jia']/p[1]/strong/text()")).GetValue());

                        string ttxx = fangElement.Select(Selectors.XPath(".//div[@class='jia']/p[2]/text()")).GetValue();
                        string[] ttxx2 = ttxx.Split('/');
                        if (ttxx2.Length == 2)
                        {
                            ttxx = ttxx2[0];
                        }

                        housePrice.SumPrice = BaseFunction.TryParseDecimal(ttxx
                            .Replace("单价", "")
                            .Replace("元", ""));

                        housePriceList.Add(housePrice);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                Logger?.LogInformation($"{page.TargetUrl}页面获取到二手房信息{houseList.Count}个，获取到二手房价格信息{housePriceList.Count}个");

                page.AddResultItem("Result2", houseList);

                page.AddResultItem("Result3", housePriceList);
            }
        }
    }
}

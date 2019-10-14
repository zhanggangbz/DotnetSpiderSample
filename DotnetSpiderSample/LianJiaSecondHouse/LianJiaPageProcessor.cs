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

namespace DotnetSpiderSample.LianJiaSecondHouse
{
    class LianJiaPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            ///如果是小区列表页面，则从列表中获取内容页的地址加入到后续目标中
            if (Regex.IsMatch(page.TargetUrl, "https://hz.lianjia.com/xiaoqu/[\\s\\S]+"))
            {
                //获取小区二手房的链接，并加入列表
                //var totalCnblogElements = page.Selectable().SelectList(Selectors.XPath("//ul[@class='listContent']/li/div[@class='xiaoquListItemRight']/div[@class='xiaoquListItemSellCount']/a")).Links().GetValues();
                //foreach (var cnblogElement in totalCnblogElements)
                //{
                //    var request = BaseFunction.CreateRequest(cnblogElement);
                //    page.AddTargetRequest(request);
                //}
                //Logger?.LogInformation($"{page.TargetUrl}页面获取到小区连接{ totalCnblogElements.ToList().Count }个");

                //获取每个小区的信息
                var totalXiaoQuElements = page.Selectable().SelectList(Selectors.XPath("//ul[@class='listContent']/li")).Nodes();

                var xiaoquList = new List<LianJiaXiaoQuEntity>();
                var xiaoquPriceList = new List<LianJiaXiaoQuPriceEntity>();
                foreach (var xiaoquElement in totalXiaoQuElements)
                {
                    try
                    {
                        var xiaoqu = new LianJiaXiaoQuEntity();
                        xiaoqu.Name = xiaoquElement.Select(Selectors.XPath(".//div[@class='info']/div[@class='title']/a/text()")).GetValue();
                        xiaoqu.Url = xiaoquElement.Select(Selectors.XPath(".//div[@class='xiaoquListItemSellCount']/a/@href")).GetValue();
                        xiaoqu.Describe = xiaoquElement.Select(Selectors.XPath(".//div[@class='houseInfo']/a/text()")).GetValue().Trim();

                        xiaoqu.Region = page.Request.Properties["diqu"];
                        xiaoqu.Id = xiaoqu.Url.Substring(xiaoqu.Url.LastIndexOf('c'), xiaoqu.Url.LastIndexOf('/') - xiaoqu.Url.LastIndexOf('c'));
                        xiaoquList.Add(xiaoqu);

                        var xiaoquPrice = new LianJiaXiaoQuPriceEntity();
                        xiaoquPrice.Id = xiaoqu.Id;
                        xiaoquPrice.Date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);//DateTime.Now.ToString("yyyy-MM-dd");
                        xiaoquPrice.Price = BaseFunction.TryParseDecimal(xiaoquElement.Select(Selectors.XPath(".//div[@class='xiaoquListItemPrice']/div[@class='totalPrice']/span/text()")).GetValue());
                        //xiaoquPrice.PriceRange = xiaoquElement.Select(Selectors.XPath(".//div[@class='listCon']/div/div/p[2]/text()")).GetValue();
                        xiaoquPrice.OnSellCount = BaseFunction.TryParseInt(xiaoquElement.Select(Selectors.XPath(".//div[@class='xiaoquListItemSellCount']/a/span/text()")).GetValue());
                        xiaoquPriceList.Add(xiaoquPrice);

                        if(xiaoquPrice.OnSellCount > 0)
                        {
                            page.AddTargetRequest(BaseFunction.CreateRequest(string.Format("https://hz.lianjia.com/ershoufang/{0}/", xiaoqu.Id)));
                        }

                        if (xiaoquPrice.OnSellCount > 30)
                        {
                            int i = 1+ xiaoquPrice.OnSellCount / 30;

                            Logger?.LogInformation($"{xiaoqu.Id}小区在售{xiaoquPrice.OnSellCount}个，分{i}页");

                            for (int index = 2;index <= i; ++index)
                            {
                                page.AddTargetRequest(BaseFunction.CreateRequest(string.Format("https://hz.lianjia.com/ershoufang/pg{0}{1}/", index, xiaoqu.Id)));
                            }
                        }

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
            else if (Regex.IsMatch(page.TargetUrl, "https://hz.lianjia.com/ershoufang/[\\s\\S]+"))
            {
                string xiaoquID = "";

                Regex regex = new Regex("c\\d+");

                var matchs = regex.Matches(page.TargetUrl);

                if(matchs.Count > 0)
                {
                    foreach(Match item in matchs)
                    {
                        if(item.Value.Length>5)
                        {
                            xiaoquID = item.Value;
                            break;
                        }
                    }
                }
                
                //获取每个房子的信息
                var totalFangZiElements = page.Selectable().SelectList(Selectors.XPath("//ul[@class='sellListContent']/li/div[@class='info clear']")).Nodes();

                var houseList = new List<LianJiaHouseEntity>();
                var housePriceList = new List<LianJiaHousePriceEntity>();
                foreach (var fangElement in totalFangZiElements)
                {
                    try
                    {
                        var house = new LianJiaHouseEntity();
                        house.Title = fangElement.Select(Selectors.XPath(".//div[@class='title']/a/text()")).GetValue();
                        house.Url = fangElement.Select(Selectors.XPath(".//div[@class='title']/a/@href")).GetValue();
                        house.Id = house.Url.Substring(house.Url.LastIndexOf('/') + 1, house.Url.LastIndexOf('.') - house.Url.LastIndexOf('/') - 1);
                        house.XiaoQuId = xiaoquID;
                        string huxmj = fangElement.Select(Selectors.XPath(".//div[@class='address']/div[@class='houseInfo']")).GetValue();
                        string[] tt = huxmj.Split('|');
                        if (tt.Length > 3)
                        {
                            house.MianJi = BaseFunction.TryParseDecimal(tt[2].Replace("平", "").Replace("米", "").Trim());
                            house.HuXing = tt[1].Trim();
                        }

                        //huxmj = fangElement.Select(Selectors.XPath(".//div[@class='listX']/p[3]")).GetValue();
                        //tt = huxmj.Split('·');
                        //if (tt.Length > 2)
                        //{
                        //    house.PublicTime = BaseFunction.TryParseDateTime(tt[2].Replace("发", "").Replace("布", "").Trim());
                        //}
                        houseList.Add(house);


                        var housePrice = new LianJiaHousePriceEntity();
                        housePrice.Id = house.Id;
                        housePrice.Date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);//DateTime.Now.ToString("yyyy-MM-dd");
                        housePrice.Price = BaseFunction.TryParseDecimal(fangElement.Select(Selectors.XPath(".//div[@class='priceInfo']/div[@class='totalPrice']/span/text()")).GetValue());

                        string ttxx = fangElement.Select(Selectors.XPath(".//div[@class='priceInfo']/div[@class='unitPrice']/span/text()")).GetValue();
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

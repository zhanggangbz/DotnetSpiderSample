using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Downloader;
using DotnetSpider.Extraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotnetSpiderSample.CnBlog
{
    class CnblogPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            ///如果是列表页面，则从列表中获取内容页的地址加入到后续目标中
            if (Regex.IsMatch(page.TargetUrl, "https://www.cnblogs.com/p\\d"))
            {
                var totalCnblogElements = page.Selectable().SelectList(Selectors.XPath("//div[@class='post_item']")).Nodes();
                foreach (var cnblogElement in totalCnblogElements)
                {
                    string url = cnblogElement.Select(Selectors.XPath(".//div[@class='post_item_body']/h3")).Links().GetValue();
                    page.AddTargetRequest(new Request(url, null));
                }
            }
            ///如果是内容页，则解析内容页
            else if (Regex.IsMatch(page.TargetUrl, "https://www.cnblogs.com/[\\s\\S]+/p/\\d+.html"))
            {
                ///获取标题和作者
                string titlestr = page.Selectable().Select(Selectors.XPath("//title")).GetValue();

                string[] temparray = titlestr.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                if (temparray.Length == 3)
                {
                    var cnblog = new CnblogEntity();
                    cnblog.Url = page.TargetUrl;
                    cnblog.Title = temparray[0];
                    cnblog.Author = temparray[1];
                    cnblog.Conter = page.Selectable().Select(Selectors.XPath(".//div[@id='cnblogs_post_body']")).GetValue();
                    page.AddResultItem("Result", cnblog);
                }
            }
        }
    }
}

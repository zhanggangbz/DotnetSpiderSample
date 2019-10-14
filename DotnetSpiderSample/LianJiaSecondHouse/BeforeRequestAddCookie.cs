using DotnetSpider.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.LianJiaSecondHouse
{
    class BeforeRequestAddCookie : BeforeDownloadHandler
    {
        public override void Handle(ref Request request, IDownloader downloader)
        {
            if (!request.Headers.ContainsKey("Cookie"))
            {
                request.Headers.Add("Cookie", LianJiaSpider.CookieString);
            }
            else
            {
                request.Headers["Cookie"] = LianJiaSpider.CookieString;
            }
        }
    }
}

using DotnetSpider.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.O5I5JSecondHouse
{
    class BeforeRequestAddCookie : BeforeDownloadHandler
    {
        public override void Handle(ref Request request, IDownloader downloader)
        {
            if (!request.Headers.ContainsKey("Cookie"))
            {
                request.Headers.Add("Cookie", O5I5JSpider.CookieString);
            }
            else
            {
                request.Headers["Cookie"] = O5I5JSpider.CookieString;
            }
        }
    }
}

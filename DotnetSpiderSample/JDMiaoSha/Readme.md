#  

1.使用Fiddler对京东秒杀页面进行分析

	得到https://ai.jd.com/index_new?app=Seckill&action=pcMiaoShaAreaList&callback=pcMiaoShaAreaList&_={时间戳}，为京东秒杀商品获取的页面，且该页面返回的数据为json
	以上链接是进入秒杀界面默认获取数据的链接，如果点击其他时间段的秒杀，会通过https://ai.jd.com/index_new?app=Seckill&action=pcMiaoShaAreaList&callback=pcMiaoShaAreaList&gid={某个ID}&_={时间戳}来获取对应的商品列表

2.分析返回的json数据，得到秒杀商品的详细信息

	{
		...
		"gid": "当前返回值所属的ID",
		"groups":[获取到的所有秒杀场次，其中包含每个场次的gid],
		"miaoShaList":["本场次参与秒杀的商品列表",
			{
				"一系列商品信息"
			}
		]
	}
	
3.特殊

	以上数据返回的请求，需要添加Referer才能正常返回数据
		AddHeaders("ai.jd.com", new Dictionary<string, object> {
			{ "Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8" },
            { "Referer", "https://miaosha.jd.com/"}
        });

	只有在第一次请求的时候回去其他场次的ID，并构建新请求加入目标请求列表
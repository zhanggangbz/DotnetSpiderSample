# 博客园cnblogs.com文章抓取爬虫

1.从列表页抓取内容页连接

	列表页链接：
		https://www.cnblogs.com/p{0}
	列表区域：
		<div class="post_item">...</div>
		<div class="post_item_body">...</div>
	内容页连接：
		<div class="post_item_body"><h3><a href="{0}">....</a>...</h3>...</div>
	

2.从内容页获取文章标题，作者，内容信息
	
	标题和作者：
		<title>{0} - {1} - 博客园</title>
	内容：
		<div id="cnblogs_post_body" >...</div>
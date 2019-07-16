
using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;

namespace DotnetSpiderSample.CnBlog
{
    /// <summary>
    /// 文章内容实体
    /// 对应存放在test_spider库中的cnblog_entity_model表中
    /// </summary>
    [Schema("test_spider", "cnblog_entity_model")]
    class CnblogEntity: IBaseEntity
    {
        /// <summary>
        /// URL
        /// 主键
        /// </summary>
        [Column(200)]
        public string Url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column]
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Column]
        public string Author { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [Column(65535)]
        [Update]
        public string Conter { get; set; }
    }
}

using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;

namespace DotnetSpiderSample.JDMiaoSha
{
    [Schema("test_spider", "jdmiaosha_entity_model")]
    class JDMiaoShaEntity : IBaseEntity
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        [Column(50)]
        [Unique]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(1024)]
        public string Name { get; set; }

        /// <summary>
        /// 短名称
        /// </summary>
        [Column(1024)]
        public string ShortName { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Column(1024)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 原价格
        /// </summary>
        [Column]
        [Update]
        public decimal JDPrice { get; set; }

        /// <summary>
        /// 秒杀价格
        /// </summary>
        [Column]
        [Update]
        public decimal MiaoShaPrice { get; set; }

        /// <summary>
        /// 多久的最低价格
        /// </summary>
        [Column(100)]
        [Update]
        public string LowestPriceDaysInfo { get; set; }

        /// <summary>
        /// 秒杀开始时间
        /// </summary>
        [Column]
        [Update]
        public long StartTime { get; set; }
    }
}

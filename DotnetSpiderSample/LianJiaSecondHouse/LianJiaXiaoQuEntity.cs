using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.LianJiaSecondHouse
{
    [Schema("test_spider", "lianjia_xiaoqu_entity_model")]
    class LianJiaXiaoQuEntity : IBaseEntity
    {
        /// <summary>
        /// id
        /// 主键
        /// </summary>
        [Primary]
        [Column(Length = 100)]
        public string Id { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [Column]
        public string Url { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column]
        public string Describe { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [Column]
        public string Region { get; set; }

        [Column(DataType = DataType.Date)]
        public DateTime UpdateTime { get; set; }

        public LianJiaXiaoQuEntity()
        {
            UpdateTime = DateTime.Now;
        }
    }

    [Schema("test_spider", "lianjia_xiaoqu_price_entity_model")]
    class LianJiaXiaoQuPriceEntity : IBaseEntity
    {
        /// <summary>
        /// 日期
        /// 主键
        /// </summary>
        [Primary]
        [Column(DataType = DataType.Date)]
        public DateTime Date1 { get; set; }
        /// <summary>
        /// 小区id
        /// 主键
        /// </summary>
        [Primary]
        [Column(Length = 50)]
        public string Id { get; set; }
        /// <summary>
        /// 均价
        /// </summary>
        [Column]
        public decimal Price { get; set; }

        /// <summary>
        /// 总价区间
        /// </summary>
        [Column]
        public string PriceRange { get; set; }

        /// <summary>
        /// 在售数目
        /// </summary>
        [Column]
        public int OnSellCount { get; set; }

        [Column(DataType = DataType.Date)]
        public DateTime UpdateTime { get; set; }

        public LianJiaXiaoQuPriceEntity()
        {
            UpdateTime = DateTime.Now;
        }
    }
}

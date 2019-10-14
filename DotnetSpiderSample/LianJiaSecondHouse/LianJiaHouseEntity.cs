using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.LianJiaSecondHouse
{
    [Schema("test_spider", "lianjia_house_entity_model")]
    class LianJiaHouseEntity : IBaseEntity
    {
        /// <summary>
        /// id
        /// 主键
        /// </summary>
        [Primary]
        [Column(Length = 100)]
        public string Id { get; set; }

        /// <summary>
        /// 小区ID
        /// </summary>
        [Column]
        public string XiaoQuId { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [Column]
        public string Url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column]
        public string Title { get; set; }

        /// <summary>
        /// 户型
        /// </summary>
        [Column]
        public string HuXing { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [Column]
        public decimal MianJi { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [Column(DataType = DataType.Date)]
        public DateTime PublicTime { get; set; }

        [Column(DataType = DataType.Date)]
        public DateTime UpdateTime { get; set; }

        public LianJiaHouseEntity()
        {
            UpdateTime = DateTime.Now;
        }
    }

    [Schema("test_spider", "lianjia_house_price_entity_model")]
    class LianJiaHousePriceEntity : IBaseEntity
    {
        /// <summary>
        /// 日期
        /// 主键
        /// </summary>
        [Primary]
        [Column(DataType = DataType.Date)]
        public DateTime Date1 { get; set; }
        /// <summary>
        /// 房屋id
        /// 主键
        /// </summary>
        [Primary]
        [Column(Length = 50)]
        public string Id { get; set; }
   
        /// <summary>
        /// 单价
        /// </summary>
        [Column]
        public decimal Price { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Column]
        public decimal SumPrice { get; set; }

        [Column(DataType = DataType.Date)]
        public DateTime UpdateTime { get; set; }

        public LianJiaHousePriceEntity()
        {
            UpdateTime = DateTime.Now;
        }
    }
}

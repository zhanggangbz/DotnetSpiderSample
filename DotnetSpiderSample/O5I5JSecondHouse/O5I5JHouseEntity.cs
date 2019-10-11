using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.O5I5JSecondHouse
{
    [Schema("test_spider", "o5i5j_house_entity_model")]
    class O5I5JHouseEntity : IBaseEntity
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

        public O5I5JHouseEntity()
        {
            UpdateTime = DateTime.Now;
        }
    }

    [Schema("test_spider", "o5i5j_house_price_entity_model")]
    class O5I5JHousePriceEntity : IBaseEntity
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

        public O5I5JHousePriceEntity()
        {
            UpdateTime = DateTime.Now;
        }
    }
}

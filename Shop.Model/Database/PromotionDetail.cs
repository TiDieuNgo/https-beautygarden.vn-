using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("PromotionDetails")]
    public class PromotionDetail
    {
        [Key]
        public int id_ { get; set; }
        public int ProductId { get; set; }
        public decimal PriceDiscount { get; set; }
        public decimal Price { get; set; }
        public decimal Percent { get; set; }
        public int PromotionId { get; set; }
    }

    public class PromotionDetailMapping
    {
        public class PromotionDetail
        {
            public int id_ { get; set; }
            public int ProductId { get; set; }
            public decimal PriceDiscount { get; set; }
            public int? PricePro { get; set; }
            public decimal Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
            public decimal Percent { get; set; }
            public int PromotionId { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }
    }
}

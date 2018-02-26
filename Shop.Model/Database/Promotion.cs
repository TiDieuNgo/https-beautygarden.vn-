using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Shop.Model
{
    [Table("Promotions")]
    public class Promotion
    {
        [Key]
        public int id_ { get; set; }
        public string Title { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public short Discount { get; set; }
        public string ProductCategoryIds { get; set; }
        public string ProductIds { get; set; }
        public int IdUser { get; set; }
        public int IdUserOk { get; set; }
        public bool Active { get; set; }
        public string Region { get; set; }
    }

    public enum RegionEnum : short
    {
        All = 1,
        Bac = 2,
        Trung = 3,
        Nam = 4
    }

    public class PromotionMapping
    {
        public class Promotion
        {
            public int id_ { get; set; }
            public string Title { get; set; }
            public DateTime StartDayTime { get; set; }
            public string StartDay { get { return StartDayTime.ToString("dd/MM/yyyy HH:mm"); } }
            public DateTime EndDayTime { get; set; }
            public string EndDay { get { return EndDayTime.ToString("dd/MM/yyyy HH:mm"); } }

            public short Discount { get; set; }
            public string ProductCategoryIds { get; set; }
            public string ProductIds { get; set; }
            public bool Active { get; set; }
            public string Region { get; set; }
        }
        public class PromotionCheck
        {
            public int id_ { get; set; }
            public DateTime StartDayTime { get; set; }
            public string StartDay { get { return StartDayTime.ToString("dd/MM/yyyy HH:mm:ss"); } }
            public DateTime EndDayTime { get; set; }
            public string EndDay { get { return EndDayTime.ToString("dd/MM/yyyy HH:mm:ss"); } }

            public short Discount { get; set; }
            public string ProductIds { get; set; }
            public string Region { get; set; }

            public IList<int> ProductIdsList
            {
                get
                {
                    return string.IsNullOrEmpty(ProductIds)
                        ? new List<int>()
                        : ProductIds.Split(',').Select(o => Convert.ToInt32(o)).ToList();
                }
            }
        }
        public class PromotionCheckProduct
        {
            public int id_ { get; set; }
            public DateTime StartDayTime { get; set; }
            public string StartDay { get { return StartDayTime.ToString("dd/MM/yyyy HH:mm:ss"); } }
            public DateTime EndDayTime { get; set; }
            public string EndDay { get { return EndDayTime.ToString("dd/MM/yyyy HH:mm:ss"); } }

            public short Discount { get; set; }
            public IList<PromotionDetail> PromotionDetails { get; set; }
        }

        public static IList<ProductFrontPageMapping.ProductBox> GetPromotion(IList<PromotionMapping.PromotionCheckProduct> promotions, IList<ProductFrontPageMapping.ProductBox> products)
        {
            if (promotions.Any() && products.Any())
            {
                foreach (var p in products)
                {
                    #region check promotion
                    foreach (var pr in promotions)
                    {
                        var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == p.id_);
                        if (promotionDetail != null)
                        {
                            p.HasPromotion = true;
                            p.PricePromotion = (int)promotionDetail.PriceDiscount;
                            p.Discount = (short)promotionDetail.Percent;
                            break;
                        }
                    }
                    #endregion
                }
            }

            return products;
        }
        public static IList<ProductFrontPageMapping.ProductShow> GetPromotionProductShow(IList<PromotionMapping.PromotionCheckProduct> promotions, IList<ProductFrontPageMapping.ProductShow> products)
        {
            if (promotions.Any() && products.Any())
            {
                foreach (var p in products)
                {
                    #region check promotion
                    foreach (var pr in promotions)
                    {
                        var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == p.ProductId);
                        if (promotionDetail != null)
                        {
                            p.HasPromotion = true;
                            p.PricePromotion = (int)promotionDetail.PriceDiscount;
                            p.Discount = (short)promotionDetail.Percent;
                            break;
                        }
                    }
                    #endregion
                }
            }

            return products;
        }
    }
}

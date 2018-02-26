using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class PromotionDetailRepository : RepositoryBase<PromotionDetail>, IPromotionDetailRepository
    {
        public PromotionDetailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<PromotionDetailMapping.PromotionDetail>GetByPromotionId(int id)
        {
            return (from promotionDetail in DataContext.PromotionDetails.AsNoTracking()
                    join menu in DataContext.Menu.AsNoTracking() on promotionDetail.ProductId equals menu.id_

                    where promotionDetail.PromotionId == id
                    select new PromotionDetailMapping.PromotionDetail()
                               {
                                   ProductId = promotionDetail.ProductId,
                                   PricePro = menu.PricePro,
                                   PriceDiscount = promotionDetail.PriceDiscount,
                                   Name = menu.NameProduct,
                                   Percent = promotionDetail.Percent,
                                   id_ = promotionDetail.id_
                               }).ToList();
        }

        public IList<ProductFrontPageMapping.ProductBox> GetProductByPromotionId(int id)
        {
            return (from promotionDetail in DataContext.PromotionDetails.AsNoTracking()
                    join menu in DataContext.Menu.AsNoTracking() on promotionDetail.ProductId equals menu.id_

                    where promotionDetail.PromotionId == id && menu.HasOnHand &&menu.HasValue && menu.ok
                    select new ProductFrontPageMapping.ProductBox
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProductLong,
                        PricePro = menu.PricePro,
                        Link = menu.Link,    
                        NameProductLong = menu.NameProductLong,
                        Discount = (short) promotionDetail.Percent,
                        HasPromotion = true,
                        PricePromotion = (int) promotionDetail.PriceDiscount,
                        
                    })
                    .Distinct()
                    .Take(12)
                    .ToList();
        }
        public IList<ProductFrontPageMapping.ProductBox> GetDanhSachKhuyenmai(int id)
        {
            return (from promotionDetail in DataContext.PromotionDetails.AsNoTracking()
                    join menu in DataContext.Menu.AsNoTracking() on promotionDetail.ProductId equals menu.id_

                    where promotionDetail.PromotionId == id && menu.HasOnHand && menu.HasValue && menu.ok
                    select new ProductFrontPageMapping.ProductBox
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProductLong,
                        PricePro = menu.PricePro,
                        Link = menu.Link,
                        NameProductLong = menu.NameProductLong,
                        Discount = (short)promotionDetail.Percent,
                        HasPromotion = true,
                        PricePromotion = (int)promotionDetail.PriceDiscount,

                    })
                    .Distinct()
                    .ToList();
        }

        public PromotionDetail CheckKhuyenMaiSalePage(string barcode)
        {
            string sql = string.Format("select * from PromotionDetails where ProductId  in (select IdMenu from MenuOption where Barcode = '{0}')",barcode);
            return this.DataContext.Database.SqlQuery<PromotionDetail>(sql).FirstOrDefault();
        }

    }
    public interface IPromotionDetailRepository : IRepository<PromotionDetail>
    {
        IList<PromotionDetailMapping.PromotionDetail> GetByPromotionId(int id);
        IList<ProductFrontPageMapping.ProductBox> GetProductByPromotionId(int id);
        IList<ProductFrontPageMapping.ProductBox> GetDanhSachKhuyenmai(int id);
        PromotionDetail CheckKhuyenMaiSalePage(string barcode);
    }
}

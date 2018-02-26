using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;
using Shop.Web.Core.Caching;
using Shop.Web.Core.Helper;

namespace Shop.Data.Repositories
{
    public class PromotionRepository : RepositoryBase<Promotion>, IPromotionRepository
    {
        private const string PromotionActiveCacheKey = "PROMOTION_ACTIVE_CACHE_KEY_{0}";
        private const int time = 24 * 60 * 2;
        private readonly ICacheManager _cacheManager;

        public PromotionRepository(IDatabaseFactory databaseFactory, ICacheManager cacheManager)
            : base(databaseFactory)
        {
            _cacheManager = cacheManager;
        }

        public IList<PromotionMapping.Promotion> GetAllPromotions()
        {
            return (from promotion in DataContext.Promotions.AsNoTracking()
                    orderby promotion.id_ descending
                    select new PromotionMapping.Promotion()
                    {
                        id_ = promotion.id_,
                        StartDayTime = promotion.StartDay,
                        EndDayTime = promotion.EndDay,
                        Discount = promotion.Discount,
                        ProductCategoryIds = promotion.ProductCategoryIds,
                        ProductIds = promotion.ProductIds,
                        Active = promotion.Active,
                        Region = promotion.Region,
                    }).ToList();
        }
        public PromotionMapping.PromotionCheck GetCurrentPromotion()
        {

            return (from promotion in DataContext.Promotions.AsNoTracking()
                    where promotion.Active &&
                          promotion.StartDay <= DateTime.Now &&
                          promotion.EndDay >= DateTime.Now
                    orderby promotion.id_ descending
                    select new PromotionMapping.PromotionCheck()
                    {
                        id_ = promotion.id_,
                        StartDayTime = promotion.StartDay,
                        EndDayTime = promotion.EndDay,
                        Discount = promotion.Discount,
                        ProductIds = promotion.ProductIds,
                        Region = promotion.Region,
                    }).FirstOrDefault();
        }
        public IList<PromotionMapping.PromotionCheckProduct> GetActives()
        {
            string key = string.Format(PromotionActiveCacheKey, DateTime.Today.ToString("ddMMyyyy"));
            string previewKey = string.Format(PromotionActiveCacheKey, DateTime.Today.AddDays(-1).ToString("ddMMyyyy"));
            int minuteLater = (int)DateTime.Today.EndOfDay().Subtract(DateTime.Now).TotalMinutes;

            if (_cacheManager.IsSet(previewKey))
            {
                _cacheManager.Remove(previewKey);
            }
            return _cacheManager.Get(key, minuteLater, () =>
                                                                        {
                                                                            return
                                                                                (from promotion in
                                                                                     DataContext.Promotions.AsNoTracking
                                                                                     ()
                                                                                 where
                                                                                     promotion.Active &&
                                                                                     promotion.StartDay <= DateTime.Now &&
                                                                                     promotion.EndDay >= DateTime.Now
                                                                                 orderby promotion.id_ descending
                                                                                 select
                                                                                     new PromotionMapping.
                                                                                     PromotionCheckProduct()
                                                                                         {
                                                                                             id_ = promotion.id_,
                                                                                             StartDayTime =
                                                                                                 promotion.StartDay,
                                                                                             EndDayTime =
                                                                                                 promotion.EndDay,
                                                                                             Discount =
                                                                                                 promotion.Discount,
                                                                                             PromotionDetails =
                                                                                                 (from detail in
                                                                                                      DataContext.
                                                                                                      PromotionDetails
                                                                                                  where
                                                                                                      detail.PromotionId ==
                                                                                                      promotion.id_
                                                                                                  select detail).ToList()
                                                                                         }).ToList();
                                                                        });
        }
        public void ClearBannerCache()
        {
            string key = string.Format(PromotionActiveCacheKey, DateTime.Today.ToString("ddMMyyyy"));
            _cacheManager.Remove(key);
        }
    }
    public interface IPromotionRepository : IRepository<Promotion>
    {
        PromotionMapping.PromotionCheck GetCurrentPromotion();
        IList<PromotionMapping.Promotion> GetAllPromotions();

        IList<PromotionMapping.PromotionCheckProduct> GetActives();
        void ClearBannerCache();
    }
}

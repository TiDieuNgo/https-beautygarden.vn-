using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Helper;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class SanPhamDaXemController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPromotionRepository _promotionRepository;

        public SanPhamDaXemController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, IPromotionRepository promotionRepository)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _promotionRepository = promotionRepository;
        }

        private const int pageSize = 24;

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<int> productIds = new List<int>();
            CookieHelper cookieHelper = new CookieHelper("product_watched");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["product_watched"]);
                if (!string.IsNullOrEmpty(json))
                    productIds = json.Split(',').Select(o => Convert.ToInt32(o)).ToList();
            }
            IList<ProductFrontPageMapping.ProductBox> products = new List<ProductFrontPageMapping.ProductBox>();
            if (productIds.Any())
            {
                var allMenus = _menuRepository.GetAllMenuCache();
                products = (from m in allMenus
                            where productIds.Contains(m.id_) && m.idControl == 11 && m.ok && m.HasValue
                            select new ProductFrontPageMapping.ProductBox
                            {
                                id_ = m.id_,
                                Img = m.Img,
                                NameProduct =
                                    m.NameProduct,
                                PricePro = m.PricePro,
                                Link = m.Link,
                                HasOnHand = m.HasOnHand,
                                NgayHetHang = m.NgayHetHang
                            }).Take(9).ToList();
            }
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            DanhSach model = new DanhSach()
            {
                SanPhamTheoDanhMucs = products.ToPagedList(pageNumber, pageSize),
            };
            return View("DanhSach", model);
        }

        public ActionResult SanPhamDaXemDetail()
        {
            IList<int> productIds = new List<int>();
            CookieHelper cookieHelper = new CookieHelper("product_watched");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["product_watched"]);
                if (!string.IsNullOrEmpty(json))
                    productIds = json.Split(',').Select(o => Convert.ToInt32(o)).ToList();
            }

            IList<ProductFrontPageMapping.ProductBox> products = new List<ProductFrontPageMapping.ProductBox>();
            if (productIds.Any())
            {
                var allMenus = _menuRepository.GetAllMenuCache();
                products = (from m in allMenus
                    where productIds.Contains(m.id_) && m.idControl == 11&& m.ok && m.HasValue
                    select new ProductFrontPageMapping.ProductBox
                           {
                               id_ = m.id_,
                               Img = m.Img,
                               NameProduct =
                                   m.NameProduct,
                               PricePro = m.PricePro,
                               Link = m.Link,
                               HasOnHand = m.HasOnHand,
                               NgayHetHang = m.NgayHetHang
                           }).Take(10).ToList();
            }

            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);

            SanPhamMoiVeModel model = new SanPhamMoiVeModel()
                                      {
                                          Products = products
                                      };
            return View("SanPhamDaXemDetail", model);
         
        }
    }
}

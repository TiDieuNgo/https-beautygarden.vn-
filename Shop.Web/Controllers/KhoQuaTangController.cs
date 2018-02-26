using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Model;

namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class KhoQuaTangController : BaseController
    {
        private readonly IKhoQuaTangRepository _khoQuaTangRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private const int pageSize = 24;
        public KhoQuaTangController(IKhoQuaTangRepository khoQuaTangRepository, IMenuRepository menuRepository, IPromotionRepository promotionRepository)
        {
            _khoQuaTangRepository = khoQuaTangRepository;
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductQuaTang();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            KhoQuaTangModel model = new KhoQuaTangModel()
            {
                Products = products.ToPagedList(pageNumber, pageSize)
            };
            return View("Index", model);
        }

        /// <summary>
        /// qua tang duoc khi chosen-select chon
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult QuaTang(int productId)
        {
            var idquatang = _khoQuaTangRepository.Get(o => o.IdMenu == productId).IdSanPhamTang;
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductQuaTangByproductId(int.Parse(idquatang));
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            QuaTangModel model = new QuaTangModel()
                                    {
                                        Products = products
                                    };
            return View("quatang", model);
        }

        public ActionResult QuaTangInDetail(int productId)
        {
            var idquatang = _khoQuaTangRepository.Get(o => o.IdMenu == productId).IdSanPhamTang;
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductQuaTangByproductId(int.Parse(idquatang));
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            QuaTangModel model = new QuaTangModel()
                                 {
                                     Products = products
                                 };
            return View("quatangindetail",model);
        }

        public ActionResult QuaTangInGioHang(int productId)
        {
            // dựa vào productId kiểm tra xem sản phẩm đó có quà tặng hay không
            KhoQuaTang quatang = _khoQuaTangRepository.Get(o => o.IdMenu == productId);
            ViewBag.dataquatang = quatang;
            if (quatang != null)
            {
                int idquatang = int.Parse(quatang.IdSanPhamTang);
                Menu sanpham = _menuRepository.Get(o => o.id_ == idquatang);
                ViewBag.tenquatang = sanpham.NameProduct;
            }
            return View("quatangingiohang");
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    public class DanhSachYeuThichController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        public DanhSachYeuThichController(IMenuRepository menuRepository, IPromotionRepository promotionRepository)
        {
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
        }

        public JsonResult GetDanhSachYeuThich(List<int> IdYeuThics)
        {
            string listids = string.Join(",", IdYeuThics);
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.LaySanPhamYeuThichs(listids);
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();
            foreach (var sp in products)
            {
                if (promotions.Any())
                {
                    foreach (var pr in promotions)
                    {
                        var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == sp.id_);
                        if (promotionDetail != null)
                        {
                            sp.HasPromotion = true;
                            sp.PricePromotion = (int)promotionDetail.PriceDiscount;
                            sp.Discount = (short)promotionDetail.Percent;
                            break;
                        }
                    }
                }
            }
            DanhSach model = new DanhSach()
            {
                DanhSachYeuThichs = products
            };
            TempData["DanhSach"] = model;

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DanhSachYeuThich()
        {
            TempData.Keep();
            DanhSach model = (DanhSach)TempData["DanhSach"];
            return View(model);
        }
    }
}

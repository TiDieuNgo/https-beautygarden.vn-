using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class TagController : Controller
    {
        private readonly IDanhSachTagRepository _danhSachTagRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly ITagTinTucRepository _tagTinTucRepository;
        public TagController(IDanhSachTagRepository danhSachTagRepository, IMenuRepository menuRepository, IPromotionRepository promotionRepository, IDetailMenuRepository detailMenuRepository, ITagTinTucRepository tagTinTucRepository)
        {
            _danhSachTagRepository = danhSachTagRepository;
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
            _detailMenuRepository = detailMenuRepository;
            _tagTinTucRepository = tagTinTucRepository;
        }
        /// <summary>
        /// Danh sách tag cho sản phẩm
        /// </summary>
        /// <param name="tentag"></param>
        /// <returns></returns>
        public ActionResult DanhSach(string tentag)
        {
            ViewData["tentag"] = tentag.Replace("-", " ");
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.LayTatCaSanphamTheoTag(tentag.Replace("-"," "));
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            DanhSach model = new DanhSach()
            {
                SanPhamTheoTags = products,
                DanhSachTag = _danhSachTagRepository.GetSeoTag(tentag.Replace("-", " "))
             };
            return View("DanhSach", model);
        }

        /// <summary>
        /// Danh sách tag cho tin tức
        /// </summary>
        /// <param name="tentag"></param>
        /// <returns></returns>
        public ActionResult DanhSachtagForTinTuc(string taglink)
        {
            ViewData["tentag"] = _detailMenuRepository.GettagNameBytaglink(taglink);
            IList<DetailMenu> products = _detailMenuRepository.LayTatCaTinTucTheoTag(taglink);
            TinTucModel model = new TinTucModel()
                             {
                                 TagForTinTucs = products,
                                 TagTinTuc = _tagTinTucRepository.GetMany(o=>o.Link == taglink).FirstOrDefault()
            };
            return View("DanhSachTagTinTuc", model);
        }
    }
}

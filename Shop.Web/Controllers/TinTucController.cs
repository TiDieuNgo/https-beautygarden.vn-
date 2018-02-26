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
    public class TinTucController : BaseController
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private const int pageSize = 5;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _reviewRepository;
        private readonly IDanhSachTagRepository _danhSachTagRepository;
        private readonly ITagTinTucRepository _tagTinTucRepository;
        public TinTucController(IDetailMenuRepository detailMenuRepository, IUnitOfWork unitOfWork, IMenuRepository menuRepository, IPromotionRepository promotionRepository, IReviewRepository reviewRepository, IDanhSachTagRepository danhSachTagRepository, ITagTinTucRepository tagTinTucRepository)
        {
            _detailMenuRepository = detailMenuRepository;
            _unitOfWork = unitOfWork;
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
            _reviewRepository = reviewRepository;
            _danhSachTagRepository = danhSachTagRepository;
            _tagTinTucRepository = tagTinTucRepository;
        }

        public ActionResult Index()
        {

            return View("Index");
        }
        public ActionResult SuKienKhuyenMai(int page = 1)
        {
            TinTucModel model = new TinTucModel()
            {
                //TinhTrangSP==true --> su kien khuyen mai
                TinTucs = _detailMenuRepository.GetMany(o => o.id_Menu == 20 && o.ok && o.ShowMenu && o.ShowTinKhuyenMai).OrderByDescending(o => o.sDate).OrderBy(o => o.sPosition).ToPagedList(page, pageSize),
            };
            return View("SuKienKhuyenMai", model);
        }
        public ActionResult BiQuyetLamDep(int page = 1)
        {
            TinTucModel model = new TinTucModel()
            {
                TinTucs = _detailMenuRepository.GetMany(o => o.id_Menu == 20 && o.ok == true && o.ShowMenu == true && o.TinhTrangSP == false && o.ShowKhuyenMai == true).OrderByDescending(o => o.sDate).ToPagedList(page, pageSize),
            };
            return View("BiQuyetLamDep", model);
        }
        public ActionResult TinHot()
        {
            IList<DetailMenu> detailMenus =
                _detailMenuRepository.GetMany(o => o.id_Menu == 20 && o.ok == true && o.ShowMenu == true && o.TinhTrangSP == false && o.ShowKhuyenMai == true).Take(5).ToList();
            return View("TinHot", detailMenus);
        }
        public ActionResult Chitiet(string seolink)
        {
           //  tu id cua san pham lay duoc no thuoc danh muc tin tuc nao
            DetailMenu tinTuc = _detailMenuRepository.Get(o=>o.Link.Equals(seolink));
            if (tinTuc.TinhTrangSP && tinTuc.ShowKhuyenMai == false)//su kien khuyen mai
            {
                ViewData["duongdan"] = "/su-kien-khuyen-mai.html";
                ViewData["ten"] = "Sự Kiện Khuyến mãi";

            }
            else
            {
                ViewData["duongdan"] = "/bi-quyet-lam-dep.html";
                ViewData["ten"] = "Bí Quyết Làm Đẹp";
            }
            TinTucModel model = new TinTucModel()
            {
                tinkhacs = _detailMenuRepository.GetMany(o => o.id_Menu == 20 && o.ok  && o.ShowMenu && o.TinhTrangSP == false && o.ShowKhuyenMai).OrderByDescending(o => o.sDate).Take(8).ToList(),
                TinTucChiTiet = tinTuc,
                TagTinTucs = _tagTinTucRepository.GetTenTagForTinTuc(seolink)
            };
            _detailMenuRepository.Update(tinTuc);
            _unitOfWork.Commit();
            return View("ChiTiet", model);
        }
        public ActionResult TinTucShowHome()
        {
            IList<DetailMenu> tintucs =
                _detailMenuRepository.GetMany(
                    o =>
                    o.id_Menu == 20 && o.ok && o.ShowMenu && o.ShowTinKhuyenMai).OrderByDescending(o => o.sDate).OrderBy(o => o.sPosition).Take(10).ToList();
            return View("TinTucShowHome", tintucs);
        }
        public ActionResult SanPhamMoiVeInChiTiet()
        {
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductNew(5);
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            SanPhamMoiVeModel model=new SanPhamMoiVeModel()
                                        {
                                            Products = products,
                                        };
           
            return View("SanPhamMoiVe", model);
        }
        public ActionResult TinBaoChi(int page = 1)
        {
            TinTucModel model = new TinTucModel()
            {
                TinTucs = _detailMenuRepository.GetMany(o => o.id_Menu == 1462 && o.ok == true).OrderByDescending(o => o.sDate).OrderBy(o => o.sPosition).ToPagedList(page, pageSize),
            };
            return View("TinBaoChi", model);

        }

        public ActionResult TimKiem(string keyword, int? page)
        {
            keyword = RejectMarks(keyword);
            int pageNumber = (page ?? 1);
            IList<DetailMenu> tintucs = _detailMenuRepository.TimKiem(keyword.Trim().ToLower()).ToList();
            ViewData["key"] = keyword;
            TinTucModel model =new TinTucModel()
                               {
                                   TinTucs = tintucs.ToPagedList(pageNumber, pageSize)
                                };
            return View("TimKiem", model);
        }
    }
}

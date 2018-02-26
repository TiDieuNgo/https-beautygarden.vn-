using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Model;

namespace Shop.Web.Controllers
{
     [WhitespaceFilter]
    public class ThuongHieuController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private const int pageSize = 24;
        private readonly IPromotionRepository _promotionRepository;
        public ThuongHieuController(IMenuRepository menuRepository, IPromotionRepository promotionRepository)
        {
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
        }

        public ActionResult Index()
        {
            //chi hien thi ra nhung thuong hieu nao co san pham
            var alls = _menuRepository.GetAllBrandCache().ToList();

            var allProducts = _menuRepository.GetAllMenuCache();

            IList<ProductFrontPageMapping.BrandMapping> menus = alls.Any()
                                    ? alls.Where(o => o.idControl != 0 && o.ok && o.DungSai).ToList()
                                    : new List<ProductFrontPageMapping.BrandMapping>();

             IList<ProductFrontPageMapping.BrandMapping>datas=new List<ProductFrontPageMapping.BrandMapping>();
            if (menus.Any())
            {
                foreach (var brand in menus)
                {
                   if(allProducts.Any(o=>o.idControl==11 && o.IdNhaCungCap==brand.Id && o.HasOnHand && o.ok && o.HasValue))
                       datas.Add(brand);
                }
            }

            return View("ThuongHieuPartial", datas.OrderBy(o=>o.Name).ToList());
        }
        public ActionResult Thuonghieunoibat()
        {
            IList<ProductFrontPageMapping.ThuongHieuNoiBat> datas = _menuRepository.GetDSThuongHieuCount().Where(o=>o.id_!=1538).ToList();
            return View("thuonghieunoibat", datas);
        }
        public ActionResult Thuonghieushowhome()
        {
            ThuongHieuModel model = new ThuongHieuModel()
            {
                Brands = _menuRepository.GetAllBrandCache()
            };
            return View("Thuonghieushowhome", model);
        }
        public ActionResult DanhSach(int? page, string linkth)
        {
            int pageNumber = (page ?? 1);
            int id = _menuRepository.GetidDanhMucByLink(linkth);
            IList<Menu> sanPhams =
                   _menuRepository.getSanPhamThuocThuongHieu(linkth).OrderByDescending(o => o.sDate).OrderBy(o => o.SapXepSanPham).Where(o => o.ok && o.HasValue).ToList();
            IList<ProductFrontPageMapping.ProductBox> sanPhamsCuaDanhMuc = new List<ProductFrontPageMapping.ProductBox>();
            //lay duong dan cua san pham
            ViewData["duongdancha"] = _menuRepository.GetTenDanhMucCha(linkth);
            ViewData["duongdancon"] = _menuRepository.GetTenDanhMuc(linkth);
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();

            foreach (var sanpham in sanPhams)
            {
                ProductFrontPageMapping.ProductBox sp = new ProductFrontPageMapping.ProductBox()
                {
                    Img = sanpham.Img,
                    NameProduct = sanpham.NameProduct,
                    PricePro = sanpham.PricePro,
                    id_ = sanpham.id_,
                    Link = sanpham.Link,
                    HasOnHand = sanpham.HasOnHand,
                    NgayHetHang = sanpham.NgayHetHang
                };
                #region check promotion
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

                #endregion
                sanPhamsCuaDanhMuc.Add(sp);
            }
            DanhSach model = new DanhSach()
            {
                DanhMuc = _menuRepository.GetById(id),
                SanPhamTheoDanhMucs = sanPhamsCuaDanhMuc.ToPagedList(pageNumber, pageSize),
            };
            return View(model);
        }
        public ActionResult ThuongHieuInChiTietSP(string splink)
        {
             int idsanpham = _menuRepository.GetidDanhMucByLink(splink);
            //show thuong hieu san pham
             IList<Menu> thuonghieus = _menuRepository.GetThuongHieu(idsanpham).ToList();
            return View("ThuongHieuInChiTietSP",thuonghieus);
        }
    }
}

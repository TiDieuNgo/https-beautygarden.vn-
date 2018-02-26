using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Model;
using Shop.Web.ViewModels;

namespace Shop.Web.Controllers
{
    public class BestSellerController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private const int pageSize = 24;
        public BestSellerController(IMenuRepository menuRepository, IPromotionRepository promotionRepository)
        {
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
        }

        public ActionResult Index()
        {
            var allMenus = _menuRepository.GetAllMenuCache();
            //Lấy tất cả danh mục ông nội
            IList<ProductFrontPageMapping.CategoryBoxHomeLv1> categorysAll = allMenus.Any()
                                                                                 ? (from d in
                                                                                        allMenus.Where(
                                                                                            o =>
                                                                                            o.Style ==
                                                                                            "danh-muc-san-pham" &&
                                                                                            o.idControl != 0)
                                                                                    select
                                                                                        new ProductFrontPageMapping.
                                                                                        CategoryBoxHomeLv1
                                                                                        {
                                                                                            id_ = d.id_,
                                                                                            CategoryName =
                                                                                                d.NameProduct,
                                                                                            idControl =
                                                                                                d.idControl,
                                                                                            LevelMenu =
                                                                                                d.LevelMenu,
                                                                                            Link = d.Link,
                                                                                            IconMenu = d.IconMenu,
                                                                                            ShowMenuHome =
                                                                                                d.ShowMenuHome,
                                                                                            SapxepDanhMuc =
                                                                                                d.SapxepDanhMuc,
                                                                                            ok = d.ok
                                                                                        }).ToList()
                                                                                 : new List
                                                                                       <
                                                                                       ProductFrontPageMapping.
                                                                                       CategoryBoxHomeLv1>();
            // trang điểm, chăm sóc da
            IList<ProductFrontPageMapping.CategoryBoxHomeLv1> Menulevel1 =
                categorysAll.Where(p => p.ok && p.LevelMenu == 1 && p.idControl == 1 && p.id_ != 3351).OrderBy(p => p.SapxepDanhMuc).ToList();

            IList<ProductFrontPageMapping.CategoryBoxHomeChild> childItems = new List<ProductFrontPageMapping.CategoryBoxHomeChild>();
            if (Menulevel1.Any())
            {
                foreach (var boxHomeLv1 in Menulevel1)
                {
                    IList<ProductFrontPageMapping.ProductShow> dataProduct = new List<ProductFrontPageMapping.ProductShow>();
                    childItems.Add(new ProductFrontPageMapping.CategoryBoxHomeChild()
                    {
                        Id = boxHomeLv1.id_,
                        Link = boxHomeLv1.Link,
                        Name = boxHomeLv1.CategoryName,
                        Products = dataProduct
                    });
                }
            }
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductBestsellerShowHome();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            SanPhamViewModel.TopSeller model = new SanPhamViewModel.TopSeller()
            {
                MenuAndProducts = childItems,
                Products = products
            };
            return View("BestSeller", model);
        }

        [HttpGet]
        public ActionResult ShowDataBestSeller(int iddanhmuc)
        {
            var allMenus = _menuRepository.GetAllMenuCache();
            var ids = new List<int>() { iddanhmuc };

            var childsDm =
                allMenus.Where(o => o.Style == "danh-muc-san-pham" && o.idControl == iddanhmuc);

            ids.AddRange(childsDm.Select(o => o.id_));

            IList<ProductFrontPageMapping.ProductShow> dataProduct = _menuRepository.LaySanPhamCuaListDanhMucTrangChu(ids);

            IList<ProductFrontPageMapping.CategoryBoxHome> model = new List<ProductFrontPageMapping.CategoryBoxHome>();

            IList<ProductFrontPageMapping.CategoryBoxHomeChild> childItems = new List<ProductFrontPageMapping.CategoryBoxHomeChild>();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();

            dataProduct = PromotionMapping.GetPromotionProductShow(promotions, dataProduct);

            childItems.Add(new ProductFrontPageMapping.CategoryBoxHomeChild()
            {
                Id = 0,
                Link = "",
                Name = "",
                Products = dataProduct,
            });

            model.Add(new ProductFrontPageMapping.CategoryBoxHome()
            {
                Id = 0,
                Link = "",
                Name = "",
                Icon = "",
                Childs = childItems
            });

            SanPhamViewModel.ProductBoxViewmodel data = new SanPhamViewModel.ProductBoxViewmodel()
            {
                Categories = model,

            };


            return PartialView("ProductBoxChild", data);
        }
        public ActionResult DanhSach(int id, string sortOrder, string currentFilter, int page = 1)
        {
            int pageNumber = page;
            var allMenus = _menuRepository.GetAllMenuCache().Where(o => o.Style == "danh-muc-san-pham").ToList();
            IList<ProductFrontPageMapping.ProductBox> products = new List<ProductFrontPageMapping.ProductBox>();
            Menu danhMuc = allMenus.FirstOrDefault(o => o.id_.Equals(id));
            using (var context = new ShopDataContex())
            {
                SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);
                products =
                    context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_Top100Tuan @idanhmuc", idanhmuc).ToList();
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TuannaySort = sortOrder == "tuannay" ? "tuannay" : "tuannay";
            ViewBag.ThangnaySort = sortOrder == "thangnay" ? "thangnay" : "thangnay";
            ViewBag.Best2016Sort = sortOrder == "Best2016" ? "Best2016" : "Best2016";
            switch (sortOrder)
            {
                case "tuannay":
                    IList<ProductFrontPageMapping.ProductBox> productOrderTheoTuans = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);
                        productOrderTheoTuans =
                            context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_Top100Tuan @idanhmuc", idanhmuc).ToList();
                    }
                    products = productOrderTheoTuans.Take(100).ToList(); // trong tuan
                    break;
                case "thangnay":
                    IList<ProductFrontPageMapping.ProductBox> productOrderTheoThangs = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);

                        productOrderTheoThangs =
                            context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_Top100Thang @idanhmuc", idanhmuc).ToList();
                    }
                    products = productOrderTheoThangs.Take(100).ToList(); // trong thang
                    break;
                case "Best2016":
                    IList<ProductFrontPageMapping.ProductBox> productOrderBest2016 = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);
                        productOrderBest2016 =
                            context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_Best2016 @idanhmuc", idanhmuc).ToList();
                    }
                    products = productOrderBest2016.Take(100).ToList(); // trong nam 2016
                    break;
                default:
                    products = products.Take(100).ToList(); // trong tuan
                    break;
            }
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            products = PromotionMapping.GetPromotion(promotions, products);

            DanhSachBestSeller model = new DanhSachBestSeller()
            {
                DanhMuc = danhMuc,
                SanPhamTheoDanhMucs = products.ToPagedList(pageNumber, pageSize),
                page = page
            };
            return View("DanhSach", model);
        }

        public ActionResult DanhSach100BanChay(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Menu> sanPhams = _menuRepository.GetMany(o => o.Bestseller && o.ok && o.HasOnHand && o.HasValue)
                .ToList();
            IList<ProductFrontPageMapping.ProductBox> sanPhamsCuaDanhMuc = new List<ProductFrontPageMapping.ProductBox>();

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
                SanPhamTheoDanhMucs = sanPhamsCuaDanhMuc.ToPagedList(pageNumber, pageSize),
            };
            return View("DanhSach100BanChay", model);
        }
    }
}

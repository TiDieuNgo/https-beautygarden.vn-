using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Extensions;
using Shop.Web.Core.Helper;
using Shop.Web.Core.Models;
using Shop.Web.Model;
using Shop.Web.Models;
using SanPhamViewModel = Shop.Web.ViewModels.SanPhamViewModel;
using YKienKhachHangModel = Shop.Web.Model.YKienKhachHangModel;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class SanPhamController : BaseController
    {
        private readonly IDanhSachTagRepository _danhSachTagRepository;
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuProAddRepository _menuProAddRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMenuImageRepository _menuImageRepository;
        private readonly IMenuOptionRepository _menuOptionRepository;
        private readonly IProductStockSyncRepository _stockSyncRepository;
        private readonly IPromotionRepository _promotionRepository;
        public readonly IThietLapRepository _ThietLapRepository;
        private readonly IPromotionDetailRepository _promotionDetailRepository;
        private readonly HttpContextBase _httpContext;
        private readonly IKhoQuaTangRepository _khoQuaTangRepository;
        private readonly IThongBaoKhiCoHangRepository _baoKhiCoHangRepository;

        private const int pageSize = 24;
        public SanPhamController(IMenuRepository menuRepository, IMenuProAddRepository menuProAddRepository, IUnitOfWork unitOfWork, IMenuImageRepository menuImageRepository, IMenuOptionRepository menuOptionRepository, IProductStockSyncRepository stockSyncRepository, IPromotionRepository promotionRepository, IThietLapRepository thietLapRepository, IUserRatingRepository userRatingRepository, IDanhSachTagRepository danhSachTagRepository, IPromotionDetailRepository promotionDetailRepository, HttpContextBase httpContext, IKhoQuaTangRepository khoQuaTangRepository, IThongBaoKhiCoHangRepository baoKhiCoHangRepository)
        {
            _menuRepository = menuRepository;
            _menuProAddRepository = menuProAddRepository;
            _unitOfWork = unitOfWork;
            _menuImageRepository = menuImageRepository;
            _menuOptionRepository = menuOptionRepository;
            _stockSyncRepository = stockSyncRepository;
            _promotionRepository = promotionRepository;
            _ThietLapRepository = thietLapRepository;
            _userRatingRepository = userRatingRepository;
            _danhSachTagRepository = danhSachTagRepository;
            _promotionDetailRepository = promotionDetailRepository;
            _httpContext = httpContext;
            _khoQuaTangRepository = khoQuaTangRepository;
            _baoKhiCoHangRepository = baoKhiCoHangRepository;
        }
        public ActionResult Index()
        {

            return View("Index");
        }

        public ActionResult BoxSanPham()
        {
            IList<BoxSPTheoMenuModel> models = new List<BoxSPTheoMenuModel>();
            IList<Menu> boxsanpham =
                _menuRepository.GetMany(o => o.ok && o.Style == "danh-muc-san-pham" && o.idControl == 1 && o.ShowMenuHome).OrderBy(o => o.SapxepDanhMuc).ToList();
            foreach (var danhmuccon in boxsanpham)
            {
                int CountSP = 0;
                IList<Menu> sanPhams =
                  _menuRepository.GetSanPhamTheoDanhMuc(danhmuccon.id_).OrderByDescending(o => o.sDate).OrderBy(o => o.SapXepSanPham).Take(10).ToList();

                IList<Menu> sanPhamNoiBats =
                _menuRepository.GetSanPhamNoiBatHome(danhmuccon.id_).OrderByDescending(o => o.sDate).Where(o => o.Option1 == true).OrderBy(o => o.SapXepSanPham).Take(10).ToList();

                IList<Menu> sanPhamBanChays =
               _menuRepository.GetSanPhamBanChayHome(danhmuccon.id_).OrderByDescending(o => o.sDate).OrderBy(o => o.SoLanXem).Take(10).ToList();
                CountSP = sanPhams.Count();
                //lay danh sach menu level 2 ra menu con
                if (CountSP > 0) //neu danh muc co san pham thi moi hien thi ra man hinh 
                {
                    IList<Menu> Danhsachmenucon = _menuRepository.GetDanhMucConCuaMenuTop(danhmuccon.id_).OrderBy(o => o.SapxepDanhMuc).Where(o => o.ShowMenuHome == true).ToList();
                    IList<SanPhamTheoDanhMuc> sanPhamTheoDanhMucs = new List<SanPhamTheoDanhMuc>();
                    IList<SanPhamTheoDanhMuc> sanPhamNoiBatHomes = new List<SanPhamTheoDanhMuc>();
                    IList<SanPhamTheoDanhMuc> sanPhamBanChayHomes = new List<SanPhamTheoDanhMuc>();
                    foreach (var sanpham in sanPhams)
                    {
                        SanPhamTheoDanhMuc sp = new SanPhamTheoDanhMuc()
                        {
                            HasOnHand = sanpham.HasOnHand,
                            HasValue = sanpham.HasValue,
                            IconMenu = sanpham.IconMenu,
                            Img = sanpham.Img,
                            LevelMenu = sanpham.LevelMenu,
                            NameProduct = sanpham.NameProduct,
                            PriceOffPro = sanpham.PriceOffPro,
                            PricePro = sanpham.PricePro,
                            idControl = sanpham.idControl,
                            id_ = sanpham.id_,
                            ok = sanpham.ok,
                            sDate = DateTime.Now,
                            Link = sanpham.Link
                        };
                        sanPhamTheoDanhMucs.Add(sp);
                    }
                    foreach (var sanphamnoibat in sanPhamNoiBats)
                    {
                        SanPhamTheoDanhMuc spnb = new SanPhamTheoDanhMuc()
                        {
                            HasOnHand = sanphamnoibat.HasOnHand,
                            HasValue = sanphamnoibat.HasValue,
                            IconMenu = sanphamnoibat.IconMenu,
                            Img = sanphamnoibat.Img,
                            LevelMenu = sanphamnoibat.LevelMenu,
                            NameProduct = sanphamnoibat.NameProduct,
                            PriceOffPro = sanphamnoibat.PriceOffPro,
                            PricePro = sanphamnoibat.PricePro,
                            idControl = sanphamnoibat.idControl,
                            id_ = sanphamnoibat.id_,
                            ok = sanphamnoibat.ok,
                            sDate = DateTime.Now,
                            Link = sanphamnoibat.Link
                        };
                        sanPhamNoiBatHomes.Add(spnb);
                    }
                    foreach (var sanphambanchay in sanPhamBanChays)
                    {
                        SanPhamTheoDanhMuc spbc = new SanPhamTheoDanhMuc()
                        {
                            HasOnHand = sanphambanchay.HasOnHand,
                            HasValue = sanphambanchay.HasValue,
                            IconMenu = sanphambanchay.IconMenu,
                            Img = sanphambanchay.Img,
                            LevelMenu = sanphambanchay.LevelMenu,
                            NameProduct = sanphambanchay.NameProduct,
                            PriceOffPro = sanphambanchay.PriceOffPro,
                            PricePro = sanphambanchay.PricePro,
                            idControl = sanphambanchay.idControl,
                            id_ = sanphambanchay.id_,
                            ok = sanphambanchay.ok,
                            sDate = DateTime.Now,
                            Link = sanphambanchay.Link
                        };
                        sanPhamBanChayHomes.Add(spbc);
                    }
                    models.Add(new BoxSPTheoMenuModel()
                    {
                        DanhMucSanPham = danhmuccon,
                        SanPhams = sanPhamTheoDanhMucs,
                        SanPhamBanChayHomes = sanPhamBanChayHomes,
                        SanPhamNoiBatHomes = sanPhamNoiBatHomes,
                        DanhSachMenuCon = Danhsachmenucon //menu level 2
                    });
                }
            }
            return View("BoxSanPham", models);
        }

        public ActionResult SanPhamKhuyenMai()
        {
            //da tro thanh san pham moi ve
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductNewInHome();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            SanPhamMoiVeModel model = new SanPhamMoiVeModel()
            {
                Products = products
            };
            return View("SanPhamKhuyenMai", model);
        }

        public ActionResult SanPhamBanChay()
        {
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductSeller();
            PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();
            IList<ProductSlishow> productSlishows = new List<ProductSlishow>();
            int pageSize = (products.Count / 2);
            for (int i = 1; i <= 2; i++)
            {
                var p = products.Skip((i - 1) * pageSize)
                    .Take(pageSize).ToList();

                if (p.Any())
                {
                    productSlishows.Add(new ProductSlishow()
                    {
                        Products = p
                    });
                }

            }


            BanChayModel model = new BanChayModel()
            {
                ProductSlishows = productSlishows,
                KhuyenMai = promotion
            };

            return View("SanPhamBanChay", model);
        }

        public ActionResult HinhAnhKhac(string splink)
        {
            int idsanpham = _menuRepository.GetidDanhMucByLink(splink);
            IList<MenuImage> images =
                _menuImageRepository.GetImagesKhacs(idsanpham).OrderByDescending(o => o.date).Take(4).ToList();
            return View("HinhAnhKhac", images);
        }

        public ActionResult ChiTiet(string splink)
        {
            // int idsanpham = _menuRepository.GetidDanhMucByLink(splink);
            Menu sanPhamDangXem = _menuRepository.Get(o => o.Link.Equals(splink.Trim().ToLower()));


            sanPhamDangXem.SoLanXem = sanPhamDangXem.SoLanXem + 1;
            _menuRepository.Update(sanPhamDangXem);
            _unitOfWork.Commit();

            PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();

            //lay duong dan cua san pham
            ViewData["duongdancha"] = _menuRepository.GetTenDanhMucChaById(sanPhamDangXem.id_);
            //lay danh sach ma vach va show ra

            IList<MenuOption> menuOptions =
                _menuOptionRepository.GetBarcode(sanPhamDangXem.id_).OrderByDescending(o => o.SDate).ToList();

            //show thuong hieu san pham
            IList<Menu> thuonghieus = _menuRepository.GetThuongHieu(sanPhamDangXem.id_).ToList();
            //kiem tra san pham do co hasvalue bang 0 hay 1 de hien thi tuy chon mau mui
            //ViewData["hasvalue0or1"] = _menuRepository.CheckHasvalueTreOrFalse(idsanpham);
            ChiTietModel model = new ChiTietModel()
            {
                SanPham = sanPhamDangXem,
                MenuOptions = menuOptions,
                ThuongHieu = thuonghieus,
                CountMaVach = _menuOptionRepository.CountMaVachByIdSanPham(sanPhamDangXem.id_),
                Promotion = promotion
            };
            //check san pham co noi dung hay khong
            ViewData["viewnoidung1"] = sanPhamDangXem.Content;// _menuRepository.CheckContent1(idsanpham);
            ViewData["viewnoidung2"] = sanPhamDangXem.Content1;
            ViewData["viewnoidung3"] = sanPhamDangXem.Content2;
            ViewData["viewnoidung4"] = sanPhamDangXem.Content3;
            ViewData["viewnoidung5"] = sanPhamDangXem.Content4;
            return View("ChiTiet", model);
        }

        public ActionResult SanPhamMuaCung(int idsanpham, int idnhacungcap)
        {
            //san pham mua cung la san pham cung thuong hieu
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetSanPhamMuaCungHasProMotion(idsanpham, idnhacungcap);
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            SanPhamMuaCungModel model = new SanPhamMuaCungModel()
            {
                Products = products,
                //KhuyenMai = promotion
            };

            return View("SanPhamMuaCung", model);
        }

        public ActionResult SanPhamCungLoai(int iddanhmuc)
        {

            //lay danh sach san pham cung loai la san pham cung danh muc
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetSanPhamCungLoaiHasProMotion(iddanhmuc);
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            SanPhamMuaCungModel model = new SanPhamMuaCungModel()
            {
                Products = products,
                //KhuyenMai = promotion
            };

            return View("SanPhamCungLoai", model);
        }

        public ActionResult DanhSach(int? page, string splink, string sortOrder, string currentFilter)
        {
            int pageNumber = (page ?? 1);
            var allMenus = _menuRepository.GetAllMenuCache().Where(o => o.Style == "danh-muc-san-pham").ToList();

            Menu danhMuc = allMenus.FirstOrDefault(o => o.Link.Equals(splink));

            IList<int> idsCon = _menuRepository.LatDanhSachIdCuaDanhMuc(danhMuc.id_);
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.LayTatCaSanPhamCuaListDanhMua(idsCon);

            //lay duong dan cua san pham
            var duongDanCha = allMenus.FirstOrDefault(o => o.id_ == danhMuc.idControl);
            ViewData["duongdancha"] = duongDanCha != null ? duongDanCha.NameProduct : "";

            ViewData["duongdancon"] = danhMuc.NameProduct;
            ViewData["linkdanhmuccha"] = duongDanCha != null ? duongDanCha.Link : "";


            ViewBag.CurrentSort = sortOrder;
            ViewBag.MoiSortParm = sortOrder == "moivedesc" ? "moiveasc" : "moivedesc";
            ViewBag.GiaSortParm = sortOrder == "giaasc" ? "giadesc" : "giaasc";
            ViewBag.DanhGiaSortParm = sortOrder == "danhgiadesc" ? "danhgiaasc" : "danhgiadesc";
            ViewBag.MuaNhieuParm = sortOrder == "muanhieudesc" ? "muanhieuasc" : "muanhieudesc";


            switch (sortOrder)
            {
                case "giadesc":
                    products = products.OrderByDescending(p => p.PricePro).ToList();  // giá cao nhất
                    break;
                case "giaasc":
                    products = products.OrderBy(p => p.PricePro).ToList();  // giá thấp nhất
                    break;
                case "muanhieudesc":
                    IList<ProductFrontPageMapping.ProductBox> productOrderMuaNhieuDesc = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);

                        productOrderMuaNhieuDesc =
                         context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_MuaNhieuDesc @idanhmuc", idanhmuc).ToList();
                    }
                    products = productOrderMuaNhieuDesc.ToList(); // sản phẩm mua nhiều nhất
                    break;
                case "muanhieuasc":
                    IList<ProductFrontPageMapping.ProductBox> productOrderMuaNhieuASC = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);
                        productOrderMuaNhieuASC =
                       context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_MuaNhieuAsc @idanhmuc", idanhmuc).ToList();

                    }
                    products = productOrderMuaNhieuASC.ToList(); // sản phẩm mua ít nhất
                    break;
                case "moivedesc":
                    products = products.OrderByDescending(p => p.sDate).ToList(); // sản phẩm mới về
                    break;
                case "moiveasc":
                    products = products.OrderBy(p => p.sDate).ToList(); // sản phẩm cũ (ngược lại với sản phẩm mới về)
                    break;
                case "danhgiadesc":
                    IList<ProductFrontPageMapping.ProductBox> productOrderDanhGiaDesc = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);

                        productOrderDanhGiaDesc =
                         context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_DanhGiaDesc @idanhmuc", idanhmuc).ToList();
                    }
                    products = productOrderDanhGiaDesc.ToList(); // đánh giá nhiều nhất
                    break;
                case "danhgiaasc":
                    IList<ProductFrontPageMapping.ProductBox> productOrderDanhGiaASC = new List<ProductFrontPageMapping.ProductBox>();
                    using (var context = new ShopDataContex())
                    {
                        SqlParameter idanhmuc = new SqlParameter("idanhmuc", danhMuc.id_);

                        productOrderDanhGiaASC =
                         context.Database.SqlQuery<ProductFrontPageMapping.ProductBox>("exec Product_DanhGiaASC @idanhmuc", idanhmuc).ToList();
                    }
                    products = productOrderDanhGiaASC.ToList(); // đánh giá ít nhất
                    break;
                default:
                    products = products.OrderByDescending(p => p.sDate).ToList(); // default=sản phẩm mới về
                    break;
            }
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();

            products = PromotionMapping.GetPromotion(promotions, products);

            DanhSach model = new DanhSach()
            {
                DanhMuc = danhMuc,
                SanPhamTheoDanhMucs = products.ToPagedList(pageNumber, pageSize),
            };
            return View(model);
        }

        IList<int> LayIdCuaDanhMucCon(int parentId, IList<Menu> allMenu)
        {
            IList<int> ids = new List<int>() { parentId };
            if (allMenu.Any())
            {
                foreach (var menu in allMenu)
                {
                    if (menu.idControl == parentId)
                        ids.Add(menu.id_);

                    foreach (var l2 in allMenu)
                    {
                        if (l2.idControl == menu.id_)
                            ids.Add(l2.id_);

                        foreach (var l3 in allMenu)
                        {
                            if (l3.idControl == l2.id_)
                                ids.Add(l3.id_);

                        }

                    }
                }
            }
            return ids;
        }

        public ActionResult SanPhamMoiVe(int danhmucid)
        {
            //san pham dang khuyen mai
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductNewCungDanhMuc(danhmucid);
            // PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            SanPhamMoiVeThuocDanhMuc model = new SanPhamMoiVeThuocDanhMuc()
            {
                Products = products,
                //KhuyenMai = promotion
            };
            PromotionMapping.GetPromotion(promotions, products);
            return View("SanPhamMoiVe", model);
        }

        public ActionResult TimKiem(string keyword, int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Menu> sanPhams = _menuRepository.TimKiem(keyword.Trim().ToLower()).Where(o => o.HasValue && o.DungSai && o.ok).ToList();
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
                soluongtimduoc = sanPhams.Count()
            };

            if (pageNumber == 1)
            {
                YKienKhachHangModel.TukhoaTimkiemAdd(keyword);
            }

            ViewData["key"] = keyword;
            return View("TimKiem", model);
        }

        public ActionResult SanPhamHotInBoxSP()
        {
            IList<BoxSPTheoMenuModel> models = new List<BoxSPTheoMenuModel>();
            IList<Menu> boxsanpham =
                _menuRepository.GetMany(o => o.ok == true && o.Style == "danh-muc-san-pham" && o.idControl == 1).ToList();
            foreach (var danhmuccon in boxsanpham)
            {
                int CountSP = 0;
                IList<Menu> sanPhams =
                  _menuRepository.GetSanPhamHotThuocDanhMuc(danhmuccon.id_).OrderByDescending(o => o.sDate).ToList();
                CountSP = sanPhams.Count();
                //lay danh sach menu level 2 ra menu con
                if (CountSP > 0) //neu danh muc co san pham thi moi hien thi ra man hinh 
                {
                    IList<Menu> Danhsachmenucon = _menuRepository.GetDanhMucConCuaMenuTop(danhmuccon.id_).ToList();
                    IList<SanPhamTheoDanhMuc> sanPhamTheoDanhMucs = new List<SanPhamTheoDanhMuc>();
                    foreach (var sanpham in sanPhams)
                    {

                        SanPhamTheoDanhMuc sp = new SanPhamTheoDanhMuc()
                        {
                            HasOnHand = sanpham.HasOnHand,
                            HasValue = sanpham.HasValue,
                            IconMenu = sanpham.IconMenu,
                            Img = sanpham.Img,
                            LevelMenu = sanpham.LevelMenu,
                            NameProduct = sanpham.NameProduct,
                            PriceOffPro = sanpham.PriceOffPro,
                            PricePro = sanpham.PricePro,
                            idControl = sanpham.idControl,
                            id_ = sanpham.id_,
                            ok = sanpham.ok,
                            sDate = DateTime.Now,

                        };
                        sanPhamTheoDanhMucs.Add(sp);

                    }
                    models.Add(new BoxSPTheoMenuModel()
                    {
                        DanhMucSanPham = danhmuccon,
                        SanPhams = sanPhamTheoDanhMucs,
                        DanhSachMenuCon = Danhsachmenucon //menu level 2
                    });
                }

            }
            return View("SanPhamHotInBoxSP", models);
        }

        public ActionResult SanPhamBanChayInBoxSP()
        {
            IList<BoxSPTheoMenuModel> models = new List<BoxSPTheoMenuModel>();
            IList<Menu> boxsanpham =
                _menuRepository.GetMany(o => o.ok == true && o.Style == "danh-muc-san-pham" && o.idControl == 1).ToList();
            foreach (var danhmuccon in boxsanpham)
            {
                int CountSP = 0;
                IList<Menu> sanPhams =
                  _menuRepository.GetSanPhamBanChayThuocDanhMuc(danhmuccon.id_).OrderByDescending(o => o.sDate).ToList();
                CountSP = sanPhams.Count();
                //lay danh sach menu level 2 ra menu con
                if (CountSP > 0) //neu danh muc co san pham thi moi hien thi ra man hinh 
                {
                    IList<Menu> Danhsachmenucon = _menuRepository.GetDanhMucConCuaMenuTop(danhmuccon.id_).ToList();
                    IList<SanPhamTheoDanhMuc> sanPhamTheoDanhMucs = new List<SanPhamTheoDanhMuc>();
                    foreach (var sanpham in sanPhams)
                    {

                        SanPhamTheoDanhMuc sp = new SanPhamTheoDanhMuc()
                        {
                            HasOnHand = sanpham.HasOnHand,
                            HasValue = sanpham.HasValue,
                            IconMenu = sanpham.IconMenu,
                            Img = sanpham.Img,
                            LevelMenu = sanpham.LevelMenu,
                            NameProduct = sanpham.NameProduct,
                            PriceOffPro = sanpham.PriceOffPro,
                            PricePro = sanpham.PricePro,
                            idControl = sanpham.idControl,
                            id_ = sanpham.id_,
                            ok = sanpham.ok,
                            sDate = DateTime.Now,

                        };
                        sanPhamTheoDanhMucs.Add(sp);

                    }
                    models.Add(new BoxSPTheoMenuModel()
                    {
                        DanhMucSanPham = danhmuccon,
                        SanPhams = sanPhamTheoDanhMucs,
                        DanhSachMenuCon = Danhsachmenucon //menu level 2
                    });
                }

            }
            return View("SanPhamBanChayInBoxSP", models);
        }

        public ActionResult TuyChonMauMui(string splink)
        {
            int idsanpham = _menuRepository.GetidDanhMucByLink(splink);
            IList<MenuOption> menuOptions =
             _menuOptionRepository.GetImgMauMui(idsanpham).OrderByDescending(o => o.SDate).ToList();
            ChiTietModel model = new ChiTietModel()
            {
                MenuOptions = menuOptions,
                //kiem tra xem san pham do thuoc mau hay mui
                FlagMauMui = _menuOptionRepository.CheckMauOrMui(idsanpham),


            };

            return View("TuyChonMauMui", model);
        }

        public ActionResult TinhTrangSanPham(string Barcode, string splink)
        {
            int idsanpham = _menuRepository.GetidDanhMucByLink(splink);
            //lay barcode dau tien de kiem tra du lieu dau vao null
            string mavach = _menuOptionRepository.GetBarcodeFromIdSanPham(idsanpham);

            if (Barcode == null)
            {
                Barcode = mavach; // barcode dau tien
            }
            TinhTrangSanPhamModel model = new TinhTrangSanPhamModel()
            { //_stockSyncRepository.KiemTraTonKhoChiNhanhPhuNhuan("8809481143839")
                CheckOnhandPhuNhan = _stockSyncRepository.KiemTraTonKhoChiNhanhPhuNhuan(Barcode),

                CheckOnhandQ3 = _stockSyncRepository.KiemTraTonKhoChiNhanhQ3(Barcode),

                CheckOnhandDaNang = _stockSyncRepository.KiemTraTonKhoChiNhanhDN(Barcode),

                CheckOnhandHaNoi = _stockSyncRepository.KiemTraTonKhoChiNhanhHaNoi(Barcode)

            };
            return View("TinhTrangSanPham", model);
        }

        public ActionResult ProductHomeBox()
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
                                                                                        }).ToList()
                                                                                 : new List
                                                                                       <
                                                                                       ProductFrontPageMapping.
                                                                                       CategoryBoxHomeLv1>();
            // Lay danh muc cha
            IList<ProductFrontPageMapping.CategoryBoxHomeLv1> categorys =
                categorysAll.Where(p => p.ShowMenuHome && p.LevelMenu == 1).ToList();

            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<ProductFrontPageMapping.CategoryBoxHome> model = new List<ProductFrontPageMapping.CategoryBoxHome>();

            if (categorys.Any())
            {
                foreach (var l1 in categorys)
                {
                    IList<ProductFrontPageMapping.CategoryBoxHomeChild> childItems = new List<ProductFrontPageMapping.CategoryBoxHomeChild>();
                    IList<ProductFrontPageMapping.CategoryBoxHomeLv1> childs = categorysAll.Where(o => o.idControl == l1.id_ && o.LevelMenu == 2).Take(5).OrderBy(o => o.SapxepDanhMuc).ToList(); //khuon mat, mat,moi
                    if (childs.Any())
                    {
                        int index = 0;
                        foreach (var child in childs)
                        {
                            IList<int> idsCon =
                                categorysAll.Where(o => o.idControl == child.id_ && o.LevelMenu == 3).Select(o => o.id_).ToList();
                            idsCon.Add(child.id_); //lay tat ca id cua thang con cua tung thang add vao idscon  va chinh no
                            IList<ProductFrontPageMapping.ProductShow> dataProduct = new List<ProductFrontPageMapping.ProductShow>();
                            if (index == 0)
                            {
                                dataProduct = _menuRepository.LaySanPhamCuaListDanhMucTrangChu(idsCon);
                                dataProduct = PromotionMapping.GetPromotionProductShow(promotions, dataProduct);
                            }
                            childItems.Add(new ProductFrontPageMapping.CategoryBoxHomeChild()
                            {
                                Id = child.id_,
                                Link = child.Link,
                                Name = child.CategoryName,
                                Products = dataProduct
                            });
                            index++;
                        }
                    }
                    model.Add(new ProductFrontPageMapping.CategoryBoxHome()
                    {
                        Id = l1.id_,
                        Link = l1.Link,
                        Name = l1.CategoryName,
                        Icon = l1.IconMenu,
                        Childs = childItems
                    });
                }
            }

            return View("ProductBox", new SanPhamViewModel.ProductBoxViewmodel
            {
                Categories = model,
                // Promotion = promotion
            });
        }

        public ActionResult Detail(string splink)
        {
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand("update Menu set GiaThiTruong = CASE WHEN GiaThiTruong IS NULL OR GiaThiTruong = 0 THEN (ISNULL(PricePro, 0) +(ISNULL(PricePro, 0) * (ABS(Checksum(NewID()) % 10)+ 10)/100)) WHEN GiaThiTruong < PricePro THEN (ISNULL(PricePro, 0) +(ISNULL(PricePro, 0) * (ABS(Checksum(NewID()) % 10)+ 10)/100)) ELSE GiaThiTruong END  where idControl=11 and Link ={0}", splink);
            }
            ProductFrontPageMapping.ProductForViewDetail product = _menuRepository.GetForViewDetailByLink(splink);
            //  kiem tra san pham do co qua tang hay khong
            ViewBag.check = _khoQuaTangRepository.Get(o => o.IdMenu.Equals(product.ProductId));
            ProductFrontPageMapping.Brand brand = _menuRepository.GetBrandById(product.BrandId);
            product.BrandName = brand != null ? brand.Name : "";

            IList<ProductFrontPageMapping.Attribute> attributes =
                _menuRepository.GetAttributeByProductId(product.ProductId);
            if (!attributes.Any()) attributes = new List<ProductFrontPageMapping.Attribute>();

            IList<ProductStockSyncMapping.TonKho> tonKhos = _stockSyncRepository.GetTonKhoCuaSanPham(product.ProductId);

            #region lay ton kho trong chi nhanh
            IList<SanPhamViewModel.BranchItem> branchItems = GetBrands();
            if (attributes.Any())
            {
                attributes[0].Selected = true;
                string firstBarcode = attributes[0].Barcode;
                if (firstBarcode.IndexOf("CB") != -1 || firstBarcode.IndexOf("cb") != -1)
                {
                    //  Nếu sản phẩm đó là combo thì gán onhand = 1 luôn
                    foreach (var b in branchItems)
                    {
                        var item = tonKhos.FirstOrDefault(o => o.Barcode.Equals(firstBarcode) && o.Idbranch == b.Id);
                        if (item != null)
                        {
                            b.Quantity = 1;
                        }
                    }
                }
                else
                {
                    foreach (var b in branchItems)
                    {
                        var item = tonKhos.FirstOrDefault(o => o.Barcode.Equals(firstBarcode) && o.Idbranch == b.Id);
                        if (item != null)
                        {
                            b.Quantity = item.OnHand;
                        }
                    }
                }
            }
            #endregion
            // tính khuyến mãi
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();

            if (promotions.Any())
            {
                foreach (var pr in promotions)
                {
                    var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == product.ProductId);
                    if (promotionDetail != null)
                    {
                        product.HasPromotion = true;
                        product.PricePromotion = (int)promotionDetail.PriceDiscount;
                        product.Discount = (short)promotionDetail.Percent;
                        break;
                    }
                }
            }

            var selected = attributes.FirstOrDefault(o => o.Selected);
            Menu bearcumMenu = _menuRepository.GetBreadcrumbDanhMuc(product.ProductId);
            SanPhamViewModel.DetailPageViewModel model = new SanPhamViewModel.DetailPageViewModel()
            {
                Product = product,
                Attributes = attributes,
                BranchItems = branchItems,
                ProductAvailable = branchItems.Any(o => o.Quantity > 0),
                CurrentBarcode = selected != null ? selected.Barcode : "",
                ThietLap = _ThietLapRepository.GetAll().FirstOrDefault(),
                LuotDanhGia = _userRatingRepository.GetLuotReView(splink) < 200 ? _userRatingRepository.GetLuotReView(splink) + 200 : _userRatingRepository.GetLuotReView(splink),
                DanhSachTags = _danhSachTagRepository.GetTenTagViewDetail(splink),
                TenDMbreadcrumb = bearcumMenu.NameProduct,
                Linkbreadcrumb = bearcumMenu.Link,
            };

            #region cookie de load ra danh sach san pham ban dang xem

            //tạo cookie de load ra danh sach san pham ban dang xem

            IList<int> productIds = new List<int>();
            CookieHelper cookieHelper = new CookieHelper("product_watched");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["product_watched"]);
                if (!string.IsNullOrEmpty(json))
                    productIds = json.Split(',').Select(o => Convert.ToInt32(o)).ToList();
            }
            //kiem tra san pham do khac san pham da xem roi thi moi luu cookie, ngược lại không lưu
            if (!productIds.Any(o => o == product.ProductId))
                productIds.Add(product.ProductId);
            cookieHelper.SetViewedCookie(string.Join(",", productIds));

            #endregion
            #region cookie để lưu danh sách sản phẩm xem cùng theo thuật toán

            //tạo cookie để lưu danh sách sản phẩm xem cùng theo thuật toán
            IList<int> productxemcungIds = new List<int>();
            CookieHelper cookieHelperxemcung = new CookieHelper("product_xemcung");

            if (cookieHelperxemcung.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelperxemcung.GetCookie().Values["product_xemcung"]);
                if (!string.IsNullOrEmpty(json))
                    productxemcungIds = json.Split(',').Select(o => Convert.ToInt32(o)).ToList();
            }
            //không cần kiểm tra trùng id
            productxemcungIds.Add(product.ProductId);
            cookieHelper.SetViewedCookieXemCung(string.Join(",", productxemcungIds));

            #endregion
            //chi co admin đăng nhập mới thấy đc số lượng còn hàng
            ShopUser efmvcUser = _httpContext.User.GetShopUser();
            model.IsAdmin = efmvcUser != null && efmvcUser.UserId != 0;

            ViewData["sao"] = _userRatingRepository.GetSao(splink);
            return View("Detail", model);
        }

        [HttpPost]
        public JsonResult KiemTraTonKhoTheoMaVach(SanPhamViewModel.CheckQuantity model)
        {
            IList<SanPhamViewModel.BranchItem> branchItems = GetBrands();
            IList<ProductStockSyncMapping.TonKho> tonKhos = _stockSyncRepository.GetTonKhoCuaSanPham(model.ProductId);
            ShopUser efmvcUser = _httpContext.User.GetShopUser();
            foreach (var b in branchItems)
            {
                var item = tonKhos.FirstOrDefault(o => o.Barcode.Equals(model.Barcode) && o.Idbranch == b.Id);
                if (item != null)
                {
                    b.Quantity = item.OnHand;
                }
            }

            string html = RenderPartialViewToString("BoxBrandQuantity", new SanPhamViewModel.CheckBrandQuantityModel
            {
                BranchItems = branchItems,
                IsAdmin = efmvcUser != null && efmvcUser.UserId != 0
            });

            return Json(new
            {
                html = html,
                available = branchItems.Any(o => o.Quantity > 0)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult KiemTraTonKhoTheoSoLuong(SanPhamViewModel.CheckQuantity model)
        {
            IList<SanPhamViewModel.BranchItem> branchItems = GetBrands();
            IList<ProductStockSyncMapping.TonKho> tonKhos = _stockSyncRepository.GetTonKhoCuaSanPham(model.ProductId);
            foreach (var b in branchItems)
            {
                var item = tonKhos.FirstOrDefault(o => o.Barcode.Equals(model.Barcode) && o.Idbranch == b.Id);
                if (item != null)
                {
                    b.Quantity = item.OnHand;
                }
            }
            int max = branchItems.Max(o => o.Quantity);

            return Json(new
            {
                available = model.Quantity <= max
            }, JsonRequestBehavior.AllowGet);
        }

        IList<SanPhamViewModel.BranchItem> GetBrands()
        {
            IList<SanPhamViewModel.BranchItem> branchItems = new List<SanPhamViewModel.BranchItem>();
            branchItems.Add(new SanPhamViewModel.BranchItem()
            {
                Id = 14364,
                Name = "71 Đường số 3, Cư Xá Đô Thành, P.4, Q.3",
                Region = "HCM"
            });
            branchItems.Add(new SanPhamViewModel.BranchItem()
            {
                Id = 48611,
                Name = "98B Phan Đăng Lưu, P.3, Q. Phú Nhuận",
                Region = "HCM"
            });
            branchItems.Add(new SanPhamViewModel.BranchItem()
            {
                Id = 48609,
                Name = "152 Phố Chùa Bộc, Q. Đống Đa, Hà Nội",
                Region = "HN"
            });
            branchItems.Add(new SanPhamViewModel.BranchItem()
            {
                Id = 48610,
                Name = "268 Trưng Nữ Vương, Q. Hải Châu, Đà Nẵng",
                Region = "ĐN"
            });
            return branchItems;
        }

        public ActionResult TaiSaoChonBG()
        {
            Menu taisao =
                _menuRepository.GetMany(o => o.Style == "ly-do-mua-tai-BG" && o.idControl == 114).FirstOrDefault();
            return View("TaiSaoChonBG", taisao);
        }

        public ActionResult ChiTietTuHeThong(string barcode)
        {
            string splink = _menuOptionRepository.GetLinkSpByBarCode(barcode);
            if (splink == "" || splink == null)
            {
                return RedirectToAction("Loi");
            }
            else
            {
                ProductFrontPageMapping.ProductForViewDetail product = _menuRepository.GetForViewDetailByLink(splink);
                ProductFrontPageMapping.Brand brand = _menuRepository.GetBrandById(product.BrandId);
                product.BrandName = brand != null ? brand.Name : "";

                IList<ProductFrontPageMapping.Attribute> attributes =
                    _menuRepository.GetAttributeByProductId(product.ProductId);

                IList<ProductStockSyncMapping.TonKho> tonKhos = _stockSyncRepository.GetTonKhoCuaSanPham(product.ProductId);

                #region lay ton kho trong chi nhanh

                IList<SanPhamViewModel.BranchItem> branchItems = GetBrands();

                if (attributes.Any())
                {
                    attributes[0].Selected = true;
                    string firstBarcode = attributes[0].Barcode;
                    foreach (var b in branchItems)
                    {
                        var item = tonKhos.FirstOrDefault(o => o.Barcode.Equals(firstBarcode) && o.Idbranch == b.Id);
                        if (item != null)
                        {
                            b.Quantity = item.OnHand;
                        }
                    }
                }
                #endregion

                PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();

                if (promotion != null)
                {
                    product.HasPromotion = promotion.ProductIdsList.Contains(product.ProductId);
                    if (product.HasPromotion)
                    {
                        product.Discount = promotion.Discount;
                        product.PricePromotion = product.Price - (product.Price * promotion.Discount / 100);
                    }
                }


                SanPhamViewModel.DetailPageViewModel model = new SanPhamViewModel.DetailPageViewModel()
                {
                    Product = product,
                    Attributes = attributes,
                    BranchItems = branchItems,
                    ProductAvailable = branchItems.Any(o => o.Quantity > 0),
                    CurrentBarcode = attributes.FirstOrDefault(o => o.Selected).Barcode

                };

                return View("ChiTietTuHeThong", model);
            }


        }

        public ActionResult Loi()
        {
            return View("Loi");
        }

        public ActionResult Promotion(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<ProductFrontPageMapping.ProductBox> products = new List<ProductFrontPageMapping.ProductBox>();
            if (promotions.Count != 0)
            {
                // nếu có sản phẩm trong danh sách khuyễn mãi mới show ra màn hình
                products = _promotionDetailRepository.GetDanhSachKhuyenmai(promotions.FirstOrDefault().id_);
                PromotionMapping.GetPromotion(promotions, products);
                return View("Promotion", products.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không có khuyến mãi show thông tin khuyến mãi ra
                return View("ThongBaoKhuyenMai");
            }

        }

        public ActionResult ThongBaoKhuyenMai()
        {
            return View("ThongBaoKhuyenMai");
        }
        public ActionResult ChuongTrinhTichDiem()
        {
            return View("ChuongTrinhTichDiem");
        }

        public ActionResult SanPhamKhiCoKhuyenMai()
        {
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            SanPhamViewModel.BoxSanPhamKhuyenMaiViewModel model = new SanPhamViewModel.BoxSanPhamKhuyenMaiViewModel()
            {

            };
            if (promotions.Any())
            {
                model.Products = _promotionDetailRepository.GetProductByPromotionId(promotions.FirstOrDefault().id_);
                model.HasPromotion = true;
                model.End = promotions.FirstOrDefault().EndDayTime;
            }

            return View("SanPhamKhiCoKhuyenMai", model);
        }

        public static string FormatJSonWebsite()
        {
            StringBuilder jSon = new StringBuilder();

            jSon = jSon.Append("<script type=\"application/ld+json\">{\"@context\": \"http://schema.org\",\"@type\":\"Website\",\"description\": \"beautygarden.vn - Beauty Garden - Mỹ phẩm chính hãng\",\"name\": \"beautygarden\",\"image\": \"https://beautygarden.vn/images/logo-HVNet.png\"}</script>");
            jSon = jSon.Append("<script type=\"application/ld+json\">{\"@context\":\"http://schema.org\",\"@type\":\"Organization\",\"name\":\"Beautygarden\",\"url\":\"https://beautygarden.vn/\",\"logo\":\"https://beautygarden.vn/images/logomoi.png\",\"sameAs\":[\"https://www.facebook.com/BeautyGardenCosmetics\",\"\",\"https://www.instagram.com/beautygarden.cosmetics\"],\"contactPoint\": [{ \"@type\": \"ContactPoint\",\"telephone\": \"+84 0911384114\",\"contactType\": \"customer service\",\"contactOption\": \"TollFree\",\"areaServed\": \"VN\"},{\"@type\": \"ContactPoint\",\"telephone\": \"+84 0911384114\",\"contactType\": \"sales\",\"contactOption\": \"TollFree\",\"areaServed\": \"VN\"}]}</script>");
            jSon = jSon.Append("<script type=\"application/ld+json\">{\"@context\": \"http://schema.org\",\"@type\": \"LocalBusiness\",\"priceRange\":\"$$\",\"image\":\"https://beautygarden.vn/images/logomoi.png\",\"name\" : \"CÔNG TY TNHH HVNET\",\"telephone\": \"+84 0912 999 847\",\"address\": {\"@type\": \"PostalAddress\",\"streetAddress\": \"255 Bình Lợi, P.13, Q.Bình Thạnh\",\"addressLocality\": \"Hà Nội\"}}</script>");

            return jSon.ToString();
        }

        public static string FormatJsonProductDetailHenry(string splink)
        {

            string NoteNew = "";
            string Note = Shop.Web.Model.YKienKhachHangModel.GetContentbylink(splink);
            if (Note == "" || Note == null)
            {
                NoteNew = splink;
            }
            else
            {
                NoteNew = Note.Replace("style=\"text-align: justify;\"", "");
            }
            StringBuilder jSon = new StringBuilder();
            jSon = jSon.Append("<script type=\"application/ld+json\">{")
                .Append("\"@context\" : \"http://schema.org\",")
                .Append("\"@type\" : \"hentry\",")
                .Append("\"entry-content\" :\"" + NoteNew + "\",")
                .Append("\"bookmark\" :\"https://beautygarden.vn/" + splink + ".html\"")
                .Append("}")
                .Append("</script>");
            return jSon.ToString();
        }

        public static string FormatJsonProductDetailProduct(string splink)
        {
            string NoteNew = "";
            string Note = Shop.Web.Model.YKienKhachHangModel.GetContentbylink(splink);
            if (Note == "" || Note == null)
            {
                NoteNew = splink;
            }
            else
            {
                NoteNew = Note.Replace("style=\"text-align: justify;\"", "");

            }
            Menu sanpham = Shop.Web.Model.YKienKhachHangModel.GetProductbyLink(splink);
            double rating = Shop.Web.Model.YKienKhachHangModel.GetRatingbyLink(splink);
            int ratingCount = Shop.Web.Model.YKienKhachHangModel.GetRatingCountbyLink(splink);

            StringBuilder jSon = new StringBuilder();
            jSon = jSon.Append("<script type=\"application/ld+json\">{")
            .Append("\"@context\": \"http://schema.org\",")
            .Append("\"@type\": \"Product\",")
            .Append("\"aggregateRating\": {")
            .Append("\"@type\": \"AggregateRating\",")
            .Append("\"ratingValue\": \"" + rating + "\",")
            .Append("\"reviewCount\": \"" + ratingCount + "\"},")

            .Append("\"description\": \"" + NoteNew + "\",")
            .Append("\"name\": \"" + sanpham.NameProduct + "\",")
            .Append("\"image\": \"https://beautygarden.vn/Upload/Files/" + sanpham.Img + "\",")
            .Append("\"offers\": {")
            .Append("\"@type\": \"Offer\",")
            .Append("\"availability\": \"https://schema.org/InStock\",")
            .Append("\"price\": \"" + sanpham.PricePro + "\",")
            .Append("\"priceCurrency\": \"VND\"},")

            .Append("\"review\": [")

            .Append("{\"@type\": \"Review\",")
            .Append("\"author\":\"Đồng Thanh Sơn\",")
            .Append("\"datePublished\": \"" + Convert.ToDateTime(sanpham.sDate).ToString("yyyy-MM-dd") + "\",")
            .Append("\"description\":\"tôi đánh giá cao thông tin của bạn đã cung cấp, tks ad\",")
            .Append("\"name\": \"tôi đánh giá cao thông tin của bạn đã cung cấp, tks ad\",")
            .Append("\"reviewRating\": {")
                .Append("\"@type\": \"Rating\",")
                .Append("\"bestRating\": \"5\",")
                .Append("\"ratingValue\": \"" + rating + "\",")
                .Append("\"worstRating\": \"4\"")
                .Append("}}")

            .Append(",{\"@type\": \"Review\",")
            .Append("\"author\":\"Đồng Thanh Sơn\",")
            .Append("\"datePublished\": \"" + Convert.ToDateTime(sanpham.sDate).ToString("yyyy-MM-dd") + "\",")
            .Append("\"description\": \"tôi đánh giá cao thông tin của bạn đã cung cấp, tks ad\",")
            .Append("\"name\": \"tôi đánh giá cao thông tin của bạn đã cung cấp, tks ad\",")
            .Append("\"reviewRating\": {")
                .Append("\"@type\": \"Rating\",")
                .Append("\"bestRating\": \"5\",")
                .Append("\"ratingValue\": \"" + rating + "\",")
                .Append("\"worstRating\": \"4\"")
                .Append("}}")

            .Append(",{\"@type\": \"Review\",")
           .Append("\"author\":\"Đồng Thanh Sơn\",")
            .Append("\"datePublished\": \"" + Convert.ToDateTime(sanpham.sDate).ToString("yyyy-MM-dd") + "\",")
            .Append("\"description\":  \"tôi đánh giá cao thông tin của bạn đã cung cấp, tks ad\",")
            .Append("\"name\":  \"tôi đánh giá cao thông tin của bạn đã cung cấp, tks ad\",")
            .Append("\"reviewRating\": {")
                .Append("\"@type\": \"Rating\",")
                .Append("\"bestRating\": \"5\",")
                .Append("\"ratingValue\": \"" + rating + "\",")
                .Append("\"worstRating\": \"4\"")
                .Append("}}")

            .Append("]")
            .Append("}</script>");

            return jSon.ToString();
        }

        //public static string FormatJsonProductDetailEvent(string splink)
        //{
        //    Menu sanpham = Shop.Web.Model.YKienKhachHangModel.GetProductbyLink(splink);
        //    StringBuilder jSon = new StringBuilder();

        //    jSon = jSon.Append("<script type=\"application/ld+json\">{")
        //        .Append("\"@context\" : \"http://schema.org\",")
        //        .Append("\"@type\" : \"Event\",")
        //        .Append("\"name\" : \"Beauty Garden - Mỹ phẩm chính hãng\",")
        //        .Append("\"url\" : \"http://beautygarden.vn\",")
        //        .Append("\"description\" : \"Beauty Garden - Mỹ phẩm chính hãng\",")
        //        .Append("\"startDate\" : \"2017-06-01T08:00\",")
        //        .Append("\"endDate\" : \"2017-12-28T23:59\",")
        //        .Append("\"image\": \"http://beautygarden.vn/Upload/Files/" + sanpham.Img + "\",")
        //        .Append("\"location\" :");

        //    jSon = jSon.Append("{")
        //        .Append("\"@type\" : \"Place\",")
        //        .Append("\"url\" : \"http://beautygarden.vn/" + sanpham.Link + ".html\",")
        //        .Append("\"name\" : \"" + sanpham.NameProduct + "\",")
        //        .Append("\"image\":\"http://beautygarden.vn/Upload/Files/" + sanpham.Img + "\",")
        //        .Append("\"address\" : {")
        //        .Append("\"@type\" : \"PostalAddress\",")
        //        .Append("\"addressLocality\" : \"255 Bình Lợi, P.13, Q.Bình Thạnh\",")
        //        .Append("\"addressRegion\" : \"HCM\",")
        //        .Append("\"postalCode\" : \"700000\" }")
        //        .Append("},");

        //    jSon = jSon.Append("\"offers\" : {")
        //       .Append("\"@type\" : \"Offer\",")
        //       .Append("\"availability\" : \"http://schema.org/LimitedAvailability\",")
        //       .Append("\"price\" :\"" + sanpham.PricePro + "\",")
        //       .Append("\"priceCurrency\" : \"" + sanpham.PricePro + "\",")
        //       .Append("\"validFrom\" : \"2017-06-01T08:00\",")
        //       .Append("\"url\" : \"http://beautygarden.vn/\" },")

        //       .Append("\"performer\": [ {")
        //       .Append("\"@type\": \"Person\",")
        //       .Append("\"image\": \"http://beautygarden.com/Upload/Files/" + sanpham.Img + "\",")
        //       .Append("\"name\": \"" + sanpham.NameProduct + "\" } ]}")
        //       .Append("</script>");

        //    return jSon.ToString();
        //}
        [HttpGet]
        public ActionResult ShowDataFromMenuLevel2(int iddanhmuc)
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
                Products = dataProduct.ToList()
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
                Categories = model
            };
            return PartialView("ProductBoxChild", data);
        }

        public ActionResult PopupSanPhamDaXem()
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
                            where productIds.Contains(m.id_) && m.idControl == 11 && m.HasOnHand && m.ok && m.HasValue
                            select new ProductFrontPageMapping.ProductBox
                            {
                                id_ = m.id_,
                                Img = m.Img,
                                NameProduct =
                                               m.NameProduct,
                                PricePro = m.PricePro,
                                Link = m.Link
                            }).Take(10).ToList();
            }

            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);

            SanPhamMoiVeModel model = new SanPhamMoiVeModel()
            {
                Products = products,
            };
            return View("PopupSanPhamDaXem", model);
        }

        [HttpPost]
        public JsonResult AddKhiCoHang(int ProductId, string name, string phone, string email)
        {
            ThongBaoKhiCoHang model = new ThongBaoKhiCoHang()
                                      {
                                          ProductId = ProductId,
                                          Created = DateTime.Now,
                                          Email = email,
                                          FullName = name,
                                          Phone = phone
                                      };
            _baoKhiCoHangRepository.Add(model);
            _unitOfWork.Commit();
            return Json(new {JsonRequestBehavior.AllowGet });
        }

        // Code Remarketing FaceBook
        public static string RemarkettingFacebookViewContent(string splink)
        {
            Menu data = Shop.Web.Model.YKienKhachHangModel.GetProductbyLink(splink);
            StringBuilder JavaScript = new StringBuilder();
            JavaScript = JavaScript.Append("<script>")
                .Append("fbq('track', 'ViewContent', {")
                .Append("content_name:'" + data.NameProduct + "',")
                .Append("content_type:'product',")
                .Append("content_ids:['BGdetail_" + data.id_ + "'],")
                .Append("value:" + data.PricePro + ",")
                .Append("currency:'VND'")
                .Append("});")
                .Append("</script>");
            return JavaScript.ToString();
        }
    }

}

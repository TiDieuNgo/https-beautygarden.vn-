using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.Helper;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    public class SanPhamXemCungController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ISanPhamXemCungRepository _phamXemCungRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SanPhamXemCungController(IMenuRepository menuRepository, IPromotionRepository promotionRepository, ISanPhamXemCungRepository phamXemCungRepository, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
            _phamXemCungRepository = phamXemCungRepository;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            //danh sach san pham xem cung duoc code theo thuat toan
            //step 1: lay cookie cua san pham da xem chay for tach ra tung id va luu vao db
            //chu y: lan xem dau tien khong xay ra su kien gi: tuc la khong luu vao db hay update
            IList<int> productIds = new List<int>();
            IList<int> DanhsachSpGoiY = new List<int>();
            IList<ProductFrontPageMapping.ProductBox> products = new List<ProductFrontPageMapping.ProductBox>();
            CookieHelper cookieHelper = new CookieHelper("product_xemcung");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["product_xemcung"]);
                if (!string.IsNullOrEmpty(json))
                    productIds = json.Split(',').Select(o => Convert.ToInt32(o)).ToList();
            }

            if (productIds.Any() && productIds.Count >= 2)
            {
                int idsp = 0, idsptiep = 0;
                //san pham xem dau tien khong xu ly, bat dau xu ly tu san pham thu 2
                for (int i = 0; i < productIds.Count; i++)
                {
                    idsp = productIds[productIds.Count - 2];
                    idsptiep = productIds[productIds.Count - 1];
                }
                SanPhamXemCung sanPham = new SanPhamXemCung()
                {
                    IdSanPham = idsp,
                    IdSanPhamTiepTheo = idsptiep,
                    SoLanXem = 1
                };

                //kiem tra neu nhu idsp cua dong nay = idsp cua dong kia, idsptiep cua dong nay bang idsptiep cua dong kia
                //==> thi update so lan xem len chứ không lưu
                SanPhamXemCung dataXemCung = _phamXemCungRepository.Getdatatrung(idsp, idsptiep);
                int sltrung = _phamXemCungRepository.GetslTrung(idsp, idsptiep);
                if (sltrung != 0)
                {
                    //neu trung thi update dong do len
                    using (var context = new ShopDataContex())
                    {
                        context.Database.ExecuteSqlCommand("update SanPhamXemCungs set SoLanXem=SoLanXem+1 where SanPhamXemCungId={0}", dataXemCung.SanPhamXemCungId);
                    }
                }
                else
                {
                    //them moi
                    _phamXemCungRepository.Add(sanPham);
                    _unitOfWork.Commit();
                }
                //lay danh sach id cua idsanpham va idsanphamtieptheo kiem tra voi thang idsptiep khi nguoi dung xem
                //neu id duoi DB co chua thang nguoi dung xem thi lay danh sach id theo thuat toan

                DanhsachSpGoiY = _phamXemCungRepository.danhsachIds(idsptiep);
                if (DanhsachSpGoiY.Any())
                {
                    var allMenus = _menuRepository.GetAllMenuCache();
                    products = (from m in allMenus
                                where DanhsachSpGoiY.Contains(m.id_) && m.idControl == 11 && m.ok && m.HasValue
                                select new ProductFrontPageMapping.ProductBox
                                {
                                    id_ = m.id_,
                                    Img = m.Img,
                                    NameProduct = m.NameProduct,
                                    PricePro = m.PricePro,
                                    Link = m.Link,
                                    HasOnHand = m.HasOnHand,
                                    NgayHetHang = m.NgayHetHang
                                }).Take(8).ToList();
                }
            }

            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            SanPhamCungXemModel model = new SanPhamCungXemModel()
            {
                Products = products,
            };
            return View("sanphamxemcung", model);
        }
    }
}

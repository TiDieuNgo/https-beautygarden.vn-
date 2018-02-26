using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Extensions;
using Shop.Web.Core.Models;
using Shop.Web.ViewModels;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class FormDatHangController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDetailMenuCommentRepository _detailMenuCommentRepository;
        private readonly IDetailMenuCommentItemRepository _detailMenuCommentItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMenuOptionRepository _menuOptionRepository;
        private readonly IPromotionDetailRepository _promotionDetailRepository;

        public FormDatHangController(IMenuRepository menuRepository, IDetailMenuCommentRepository detailMenuCommentRepository, IDetailMenuCommentItemRepository detailMenuCommentItemRepository, IUnitOfWork unitOfWork, IMenuOptionRepository menuOptionRepository, IPromotionDetailRepository promotionDetailRepository)
        {
            _menuRepository = menuRepository;
            _detailMenuCommentRepository = detailMenuCommentRepository;
            _detailMenuCommentItemRepository = detailMenuCommentItemRepository;
            _unitOfWork = unitOfWork;
            _menuOptionRepository = menuOptionRepository;
            _promotionDetailRepository = promotionDetailRepository;
        }

        public ActionResult Index()
        {
            FormOrderSalePage model = new FormOrderSalePage()
            {
                Barcode = Request.QueryString.Get("barcode"),
                Name = "",
                Note = "",
                Phone = "",
                Adress = ""
            };
            return View("formdathang", model);
        }

        public ActionResult Submit()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Submit(FormOrderSalePage model)
        {
            // lấy giá và tên sản phẩm từ barcode
            if (string.IsNullOrEmpty(model.Barcode))
            {
                return RedirectToAction("Index");
            }
            else
            {
                int idsanpham = _menuOptionRepository.Get(o => o.Barcode.Equals(model.Barcode)).IdMenu;
                Menu sanpham = _menuRepository.Get(o => o.id_ == idsanpham);

                #region Insert DetailMenuComment

                DetailMenuComment order = new DetailMenuComment()
                {
                    id_Menu = 9999,
                    idBrand = 48611, // đẩy hết về Phú Nhuận
                    Name = model.Name,
                    Link = model.Phone.Trim().TrimEnd().TrimStart(),
                    Code = "Đặt hàng từ Website",
                    Content = model.Note,
                    HuongDanSuDung = "",
                    GiaoHang = model.Adress,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now
                };
                _detailMenuCommentRepository.Add(order);

                #endregion

                IList<string> str = new List<string>();
                IList<KHLHProduct> khlhProducts = new List<KHLHProduct>();
                DetailMenuCommentItem detail = new DetailMenuCommentItem();
                //kiem tra khuyen mai
                string giakm = "";
                PromotionDetail HasSalePage = _promotionDetailRepository.CheckKhuyenMaiSalePage(model.Barcode);
                if (HasSalePage != null)
                {
                    // co khuyen mai
                    giakm = HasSalePage.PriceDiscount.ToString();
                    #region Insert DetailMenuCommentItem
                    // co khuyen mai
                    detail = new DetailMenuCommentItem()
                    {
                        id_Menu = order.id_,
                        Name = sanpham.NameProduct,
                        Link = idsanpham + "",
                        Price = sanpham.PricePro + "",
                        PriceOf = giakm,
                        Content = string.Format("Mã đơn hàng<#{0}#>", order.id_),
                        Number = 1,
                        BarCode = model.Barcode,
                        sDate = DateTime.Now,
                        sDateOk = DateTime.Now,
                        Img = sanpham.Img
                    };
                    khlhProducts.Add(new KHLHProduct()
                    {
                        Code = detail.BarCode,
                        GiaWeb = int.Parse(detail.PriceOf),
                        NgayTao = detail.sDate,
                        SL = 1, //1 là số lượng sản phẩm đặt hàng
                        LinkImage = "https://beautygarden.vn/Upload/Files/" + sanpham.Img
                    });
                }
                else
                {
                    // không co khuyen mai
                    detail = new DetailMenuCommentItem()
                    {
                        id_Menu = order.id_,
                        Name = sanpham.NameProduct,
                        Link = idsanpham + "",
                        Price = sanpham.PricePro + "",
                        PriceOf = sanpham.PricePro + "",
                        Content = string.Format("Mã đơn hàng<#{0}#>", order.id_),
                        Number = 1,
                        BarCode = model.Barcode,
                        sDate = DateTime.Now,
                        sDateOk = DateTime.Now,
                        Img = sanpham.Img
                    };
                    khlhProducts.Add(new KHLHProduct()
                    {
                        Code = detail.BarCode,
                        GiaWeb = int.Parse(detail.PriceOf),
                        NgayTao = detail.sDate,
                        SL = 1, //1 là số lượng sản phẩm đặt hàng
                        LinkImage = "https://beautygarden.vn/Upload/Files/" + sanpham.Img
                    });
                }
                _detailMenuCommentItemRepository.Add(detail);
                str.Add(string.Format("{0}({1})", model.Barcode, 1)); //1 là số lượng sản phẩm đặt hàng
                #endregion

                #region insert KH_LH
                string dh = string.Format("{0}#,{1}", order.id_, string.Join(",", str));

                string sql =
                    string.Format(
                        "INSERT INTO [bg.hvnet.vn].dbo.KH_LH (NguoiNhap,Nguon,Ten,Phone,DiaChi,GhiChu,idTinh,TrangThai,DonHang,idShop) values (" +
                        " '{0}',{1},N'{2}','{3}',N'{4}',N'{5}',{6},{7},'{8}',{9} " +
                        ")", "system", 1,
                        order.Name,
                        order.Link,
                        order.GiaoHang,
                        order.Content,
                        1,
                        0,
                        dh,
                        order.idBrand
                    );

                int id = _detailMenuCommentRepository.InsertIntoKHLH(sql, order.Link);

                #endregion

                #region Insert KH_LH_product
                if (khlhProducts.Any() && id != 0)
                {
                    foreach (var khlhProduct in khlhProducts)
                    {
                        khlhProduct.IdKH = id;
                    }
                    _detailMenuCommentRepository.InsertKHLHProduct(khlhProducts);
                }

                #endregion

                _unitOfWork.Commit();
                return RedirectToAction("DatHangThanhCong", new { barcode = model.Barcode });
            }
        }

        public ActionResult GiaSalePage()
        {
            string barcode = Request.QueryString.Get("barcode");
            int Idmenu = 0;
            decimal gia = 0;
            decimal giakm = 0;
            MenuOption menuOption = _menuOptionRepository.Get(o => o.Barcode == barcode);
            // kiem tra barcode đó có khuyến mãi hay không
            PromotionDetail HasSalePage = _promotionDetailRepository.CheckKhuyenMaiSalePage(barcode);
            GiaSalePageModel model = new GiaSalePageModel();
            if (menuOption != null)
            {
                Idmenu = menuOption.IdMenu;
                Menu menu = _menuRepository.Get(o => o.id_ == Idmenu);
                if (HasSalePage != null)
                {
                    // co khuyen mai
                    giakm = HasSalePage.PriceDiscount;
                }
                gia = Convert.ToInt32(menu.PricePro);
                model = new GiaSalePageModel()
                {
                    GiaKM = giakm,
                    HasSalePage = HasSalePage,
                    Gia = gia
                };
            }
            return View("GiaSalepage", model);
        }

        public ActionResult DatHangThanhCong(string barcode)
        {
            ViewBag.barcode = barcode;
            return View("thanhtoanthanhcong");
        }
        //Purchase
        public static string RemarkettingFacebookPurchase(string barcode)
        {
            Menu data = Shop.Web.Model.YKienKhachHangModel.GetMenuByBarcode(barcode);
            StringBuilder JavaScript = new StringBuilder();
            JavaScript = JavaScript.Append("<script>")
                .Append("fbq('track', 'Purchase', {")
                .Append("content_type:'product',")
                .Append("content_ids:['BGdetail_" + barcode + "'],") //  id đơn hàng
                .Append("value:" + data.PricePro + ",") 		// tổng tiền
                .Append("currency:'VND'")
                .Append("});")
                .Append("</script>");
            return JavaScript.ToString();
        }
        //add to cart
        public static string RemarkettingFacebookAddtocart(string barcode)
        {
            Menu data = Shop.Web.Model.YKienKhachHangModel.GetMenuByBarcode(barcode);
            StringBuilder JavaScript = new StringBuilder();
            JavaScript = JavaScript.Append("<script>")
                .Append("fbq('track', 'AddToCart', {")
                .Append("content_type:'product',")
                .Append("content_ids:['" + barcode + "'],")  
                .Append("value:" + data.PricePro + ",")
                .Append("currency:'VND'")
                .Append("});")
                .Append("</script>");
            return JavaScript.ToString();
        }
        public class FormOrderSalePage
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Adress { get; set; }
            public string Note { get; set; }
            public string Barcode { get; set; }
        }
        public class GiaSalePageModel
        {
            public decimal Gia { get; set; }
            public decimal GiaKM { get; set; }
            public PromotionDetail HasSalePage { get; set; }
        }
        public class  BarcodeModel
        {
            public string Barcode { get; set; }
        }
    }
}

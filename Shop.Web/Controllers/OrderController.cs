using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DevExpress.DataAccess.Native.Sql.ConnectionProviders;
using Newtonsoft.Json;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Helper;
using Shop.Web.Models;
using Shop.Web.ViewModels;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class OrderController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductStockSyncRepository _productStockSyncRepository;
        private readonly IDetailMenuCommentRepository _detailMenuCommentRepository;
        private readonly IDetailMenuCommentItemRepository _detailMenuCommentItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKhoQuaTangRepository _khoQuaTangRepository;
        private readonly IMenuOptionRepository _menuOptionRepository;
        public OrderController(IMenuRepository menuRepository, IPromotionRepository promotionRepository, IProductStockSyncRepository productStockSyncRepository, IDetailMenuCommentRepository detailMenuCommentRepository, IDetailMenuCommentItemRepository detailMenuCommentItemRepository, IUnitOfWork unitOfWork, IKhoQuaTangRepository khoQuaTangRepository, IMenuOptionRepository menuOptionRepository)
        {
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
            _productStockSyncRepository = productStockSyncRepository;
            _detailMenuCommentRepository = detailMenuCommentRepository;
            _detailMenuCommentItemRepository = detailMenuCommentItemRepository;
            _unitOfWork = unitOfWork;
            _khoQuaTangRepository = khoQuaTangRepository;
            _menuOptionRepository = menuOptionRepository;
        }

        public ActionResult Index()
        {
            //thanh toan don hang
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }

            IList<ProductFrontPageMapping.ProductShowCart> productShowCarts =
                _menuRepository.GetProductShowCartByBarcode(productCarts.Select(o => o.Barcode).ToList());
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<CartViewModel.CartItem> cartItems = new List<CartViewModel.CartItem>();
            int gia = 0;
            if (productCarts.Any())
            {
                foreach (var o in productCarts)
                {
                    IList<ProductStockSyncMapping.TonKho> tonKhos = _productStockSyncRepository.GetTonKhoCuaSanPham(o.ProductId);
                    var tonkho = tonKhos.FirstOrDefault(t => t.Barcode.Equals(o.Barcode));
                    var item = productShowCarts.FirstOrDefault(p => p.Barcode.Equals(o.Barcode));
                    if (item != null)
                    {
                        #region ghép combo < 80K
                        if (item.Price < 80000 && o.Quantity >= 2)
                        {
                            //nếu số lượng > 2  và giá < 80000 bắt đầu tính giá combo
                            int giatru = (item.Price * 10) / 100;
                            gia = item.Price - giatru;
                        }
                        else
                        {
                            gia = item.Price;
                        }
                        #endregion

                        var cartItem = new CartViewModel.CartItem()
                        {
                            ProductId = o.ProductId,
                            Name = item.NameProduct,
                            Image = item.Img,
                            Link = item.Link,
                            Price = item.Price,
                            PricePromotion = gia,
                            Barcode = o.Barcode,
                            Quantity = o.Quantity,
                            AttributeImg = item.AttributeImg,
                            AttributeName = item.AttributeName,
                            AttributeFlag = item.AttributeFlag,
                            Available = tonkho != null && tonkho.OnHand >= o.Quantity
                        };
                        #region check promotion
                        if (promotions.Any())
                        {
                            foreach (var pr in promotions)
                            {
                                var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == cartItem.ProductId);
                                if (promotionDetail != null)
                                {
                                    cartItem.HasPromotion = true;
                                    cartItem.PricePromotion = (int)promotionDetail.PriceDiscount;
                                    cartItem.Discount = (short)promotionDetail.Percent;
                                    break;
                                }
                            }
                        }

                        #endregion
                        cartItems.Add(cartItem);
                    }
                }
            }
            CartViewModel.OrderModel cartModel = new CartViewModel.OrderModel()
            {
                CartItems = cartItems,
                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion),
                CartForm = new CartViewModel.CartForm()
            };
            return View("Index", cartModel);
        }

        [HttpPost]
        public ActionResult Save(CartViewModel.CartForm form)
        {
            //dien form va luu don hang
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }

            if (!productCarts.Any())
            {
                return RedirectToRoute("Cart");
            }

            #region items
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<ProductFrontPageMapping.ProductShowCart> productShowCarts =
                _menuRepository.GetProductShowCartByBarcode(productCarts.Select(o => o.Barcode).ToList());
            int gia = 0;
            if (!productShowCarts.Any())
            {
                return RedirectToRoute("Cart");
            }
           IList<string> ghiChuQT = new List<string>();
            IList<CartViewModel.CartItem> cartItems = new List<CartViewModel.CartItem>();
            if (productCarts.Any())
            {
                foreach (var o in productCarts)
                {
                    var item = productShowCarts.FirstOrDefault(p => p.Barcode.Equals(o.Barcode));

                    if (item == null)
                    {
                        return RedirectToRoute("Cart");
                    }
                    #region ghép combo < 80K
                    if (item.Price < 80000 && o.Quantity >= 2)
                    {
                        //nếu số lượng > 2  và giá < 80000 bắt đầu tính giá combo
                        int giatru = (item.Price * 10) / 100;
                        gia = item.Price - giatru;
                    }
                    else
                    {
                        gia = item.Price;
                    }
                    #endregion

                    #region insert sản phẩm quà tặng

                    var quatang = _khoQuaTangRepository.Get(p => p.IdMenu == o.ProductId);
                    if (quatang != null)
                    {
                        int idQT = Convert.ToInt32(quatang.IdSanPhamTang);
                        var barcodeqt = _menuOptionRepository.Get(p => p.IdMenu == idQT);

                        var productquatang = _menuRepository.Get(p => p.id_ == idQT);
                        var cartItemquatang = new CartViewModel.CartItem()
                                              {
                                                  ProductId = productquatang.id_,
                                                  Name = productquatang.NameProduct,
                                                  Image = productquatang.Img,
                                                  Price = productquatang.PricePro.HasValue ? productquatang.PricePro.Value : 0,
                                                  PricePromotion = productquatang.PricePro.HasValue ? productquatang.PricePro.Value : 0,
                                                  Barcode = barcodeqt != null ? barcodeqt.Barcode : "",
                                                  Quantity = o.Quantity,
                                                  AttributeImg = "",
                                                  AttributeName = "",
                                                  AttributeFlag = 1,
                                                  Iquatang = true
                                              };
                        ghiChuQT.Add(productquatang.NameProduct);
                        cartItems.Add(cartItemquatang);
                    }

                    #endregion

                    var cartItem = new CartViewModel.CartItem()
                    {
                        ProductId = o.ProductId,
                        Name = item.NameProduct,
                        Image = item.Img,
                        Price = item.Price,
                        PricePromotion = gia,
                        Barcode = o.Barcode,
                        Quantity = o.Quantity,
                        AttributeImg = item.AttributeImg,
                        AttributeName = item.AttributeName,
                        AttributeFlag = item.AttributeFlag
                    };
                    #region check promotion
                    if (promotions.Any())
                    {
                        foreach (var pr in promotions)
                        {
                            var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == cartItem.ProductId);
                            if (promotionDetail != null)
                            {
                                cartItem.HasPromotion = true;
                                cartItem.PricePromotion = (int)promotionDetail.PriceDiscount;
                                cartItem.Discount = (short)promotionDetail.Percent;
                                break;
                            }
                        }
                    }

                    #endregion
                    cartItems.Add(cartItem);
                }
            }
            #endregion

            #region order
            DetailMenuComment order = new DetailMenuComment()
            {
                id_Menu = 9999,
                idBrand = form.BrandId,
                Name = form.Name,
                Link = form.Phone.Trim().TrimEnd().TrimStart(),
                Code = "Đặt hàng từ Website",
                Content = form.Note + (ghiChuQT.Any() ? "<span style=\"color: red;font-weight: bold;\"> - Quà tặng</span> " + string.Join(",", ghiChuQT) : ""),
                HuongDanSuDung = form.Email,
                GiaoHang = form.Address,
                sDate = DateTime.Now,
                sDateOk = DateTime.Now
            };
            _detailMenuCommentRepository.Add(order);

            #endregion

            IList<string> str = new List<string>();

            IList<KHLHProduct> khlhProducts = new List<KHLHProduct>();

            #region detail

            foreach (var dt in cartItems)
            {
                DetailMenuCommentItem detail = new DetailMenuCommentItem()
                {
                    id_Menu = order.id_,
                    Name = dt.Name,
                    Link = dt.ProductId + "",
                    Price = dt.Price + "",
                    PriceOf = dt.PricePromotion + "",
                    Content = string.Format("Mã đơn hàng<#{0}#>", order.id_),
                    Number = dt.Quantity,
                    BarCode = dt.Barcode,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now,
                    Img = dt.Image
                };
                _detailMenuCommentItemRepository.Add(detail);

                if (!dt.Iquatang)
                    str.Add(string.Format("{0}({1})", dt.Barcode, dt.Quantity));

                khlhProducts.Add(new KHLHProduct()
                {
                    Code = detail.BarCode,
                    GiaWeb = int.Parse(detail.PriceOf),
                    NgayTao = detail.sDate,
                    SL = dt.Quantity,
                    LinkImage = "https://beautygarden.vn/Upload/Files/" + dt.Image,
                    Quatang = dt.Iquatang
                });
            }
            #endregion

            #region insert
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

            #region
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
            //sau khi thanh toán thành công thì clear cookie
            cookieHelper.ClearCookie();
            //cookieHelperform.ClearCookie();
            CookieHelper cookieInfo = new CookieHelper("User_Infor");
            string jsonString = JsonConvert.SerializeObject(new LienHeModel
            {
                GiaoHang = form.Address,
                HuongDanSuDung = form.Email,
                Link = form.Phone,
                Name = form.Name
            });
            cookieInfo.SetUserInforCookie(jsonString);
            return RedirectToRoute("OrderSuccess", new { id = order.id_ });
        }

        [HttpPost]
        public ActionResult SaveQuick(CartViewModel.CartForm form)
        {
            if (!ModelState.IsValid)
            {
                Menu menu = _menuRepository.Get(o => o.id_ == form.ProductId);
                return RedirectToRoute("ChiTietSanPham", new { splink = menu.Link });
            }
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            #region cart
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }
            //neu trung id va ma vach thi cong thêm số lượng, ngược lại thì mới add cookie
            bool exist = productCarts.Any(o => o.ProductId == form.ProductId && o.Barcode.Equals(form.Barcode));
            if (exist)
            {
                foreach (var p in productCarts)
                {
                    if (p.ProductId == form.ProductId && p.Barcode.Equals(form.Barcode))
                    {
                        p.Quantity += 1;
                        break;
                    }
                }
            }
            else
            {
                productCarts.Add(new CartViewModel.ProductCart()
                {
                    ProductId = form.ProductId,
                    Barcode = form.Barcode,
                    Quantity = 1
                });
            }
            string jsonString = JsonConvert.SerializeObject(productCarts);
            cookieHelper.SetCartCookie(jsonString);
            #endregion

            #region items
            // PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<ProductFrontPageMapping.ProductShowCart> productShowCarts =
                _menuRepository.GetProductShowCartByBarcode(productCarts.Select(o => o.Barcode).ToList());

            if (!productShowCarts.Any())
            {
                return RedirectToRoute("Cart");
            }
            IList<string> ghiChuQT = new List<string>();
            IList<CartViewModel.CartItem> cartItems = new List<CartViewModel.CartItem>();
            if (productCarts.Any())
            {
                foreach (var o in productCarts)
                {
                    var item = productShowCarts.FirstOrDefault(p => p.Barcode.Equals(o.Barcode));
                    if (item == null)
                    {
                        return RedirectToRoute("Cart");
                    }

                    #region insert sản phẩm quà tặng

                    var quatang = _khoQuaTangRepository.Get(p => p.IdMenu == o.ProductId);
                    if (quatang != null)
                    {
                        int idQT = Convert.ToInt32(quatang.IdSanPhamTang);
                        var barcodeqt = _menuOptionRepository.Get(p => p.IdMenu == idQT);

                        var productquatang = _menuRepository.Get(p => p.id_ == idQT);
                        var cartItemquatang = new CartViewModel.CartItem()
                                              {
                                                  ProductId = productquatang.id_,
                                                  Name = productquatang.NameProduct,
                                                  Image = productquatang.Img,
                                                  Price = productquatang.PricePro.HasValue ? productquatang.PricePro.Value : 0,
                                                  PricePromotion = productquatang.PricePro.HasValue ? productquatang.PricePro.Value : 0,
                                                  Barcode = barcodeqt != null ? barcodeqt.Barcode : "",
                                                  Quantity = 1,
                                                  AttributeImg = "",
                                                  AttributeName = "",
                                                  AttributeFlag = 1,
                                                  Iquatang = true
                                              };
                        ghiChuQT.Add(productquatang.NameProduct);
                        cartItems.Add(cartItemquatang);
                    }

                    #endregion
                    var cartItem = new CartViewModel.CartItem()
                    {
                        ProductId = o.ProductId,
                        Name = item.NameProduct,
                        Image = item.Img,
                        Price = item.Price,
                        PricePromotion = item.Price,
                        Barcode = o.Barcode,
                        Quantity = o.Quantity,
                        AttributeImg = item.AttributeImg,
                        AttributeName = item.AttributeName,
                        AttributeFlag = item.AttributeFlag
                    };

                    #region check promotion
                    if (promotions.Any())
                    {
                        foreach (var pr in promotions)
                        {
                            var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == cartItem.ProductId);
                            if (promotionDetail != null)
                            {
                                cartItem.HasPromotion = true;
                                cartItem.PricePromotion = (int)promotionDetail.PriceDiscount;
                                cartItem.Discount = (short)promotionDetail.Percent;
                                break;
                            }
                        }
                    }

                    #endregion
                    cartItems.Add(cartItem);
                }
            }
            #endregion

            #region Insert DetailMenuComment
            DetailMenuComment order = new DetailMenuComment()
            {
                id_Menu = 9999,
                idBrand = form.BrandId,
                Name = form.Name,
                Link = form.Phone.Trim().TrimEnd().TrimStart(),
                Code = "Đặt hàng từ Website",
                Content = form.Note + (ghiChuQT.Any() ? "<span style=\"color: red;font-weight: bold;\"> - Quà tặng</span> " + string.Join(",", ghiChuQT) : ""),
                HuongDanSuDung = form.Email,
                GiaoHang = form.Address,
                sDate = DateTime.Now,
                sDateOk = DateTime.Now
            };
            _detailMenuCommentRepository.Add(order);

            #endregion

            IList<string> str = new List<string>();

            IList<KHLHProduct> khlhProducts = new List<KHLHProduct>();

            #region Insert DetailMenuCommentItem

            foreach (var dt in cartItems)
            {
                DetailMenuCommentItem detail = new DetailMenuCommentItem()
                {
                    id_Menu = order.id_,
                    Name = dt.Name,
                    Link = dt.ProductId + "",
                    Price = dt.Price + "",
                    PriceOf = dt.PricePromotion + "",
                    Content = string.Format("Mã đơn hàng<#{0}#>", order.id_),
                    Number = dt.Quantity,
                    BarCode = dt.Barcode,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now,
                    Img = dt.Image
                };
                _detailMenuCommentItemRepository.Add(detail);

                if (!dt.Iquatang)
                    str.Add(string.Format("{0}({1})", dt.Barcode, dt.Quantity));

                khlhProducts.Add(new KHLHProduct()
                {
                    Code = detail.BarCode,
                    GiaWeb = int.Parse(detail.PriceOf),
                    NgayTao = detail.sDate,
                    SL = dt.Quantity,
                    LinkImage = "https://beautygarden.vn/Upload/Files/" + dt.Image,
                    Quatang = dt.Iquatang
                });
            }
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
                    //string.Format("Mã đơn hàng<#{0}#>", order.id_),
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
            //sau khi thanh toán thành công thì clear cookie
            cookieHelper.ClearCookie();
            //cookieHelperform.ClearCookie();

            CookieHelper cookieInfo = new CookieHelper("User_Infor");

            string jsonStringinfo = JsonConvert.SerializeObject(new LienHeModel()
            {
                GiaoHang = form.Address,
                HuongDanSuDung = form.Email,
                Link = form.Phone,
                Name = form.Name
            });
            cookieInfo.SetUserInforCookie(jsonStringinfo);

            return RedirectToRoute("OrderSuccess", new { id = order.id_ });
        }

        public ActionResult Success(int id)
        {
            DetailMenuComment order = _detailMenuCommentRepository.GetById(id);
            if (order == null)
                return RedirectToRoute("Home");

            IList<DetailMenuCommentItem> details =
                _detailMenuCommentItemRepository.GetMany(o => o.id_Menu == order.id_).ToList();

            IList<CartViewModel.CartItem> cartItems = (from o in details
                                                       select new CartViewModel.CartItem
                                                       {
                                                           ProductId = Convert.ToInt32(o.Link),
                                                           Name = o.Name,
                                                           Image = o.Img,
                                                           Price = Convert.ToInt32(o.Price),
                                                           PricePromotion = Convert.ToInt32(o.PriceOf),
                                                           Barcode = o.BarCode,
                                                           Quantity = Convert.ToInt32(o.Number)
                                                       }).ToList();

            CartViewModel.OrderModel cartModel = new CartViewModel.OrderModel()
            {
                CartItems = cartItems,
                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion),
                orderIdForremarketting = id, // lấy Id để truyền vào hàm remarketing
                CartForm = new CartViewModel.CartForm()
                {
                    Name = order.Name,
                    Phone = order.Link,
                    Address = order.GiaoHang,
                    Email = order.HuongDanSuDung,
                    Note = order.Content,
                    ShipAddress = order.GiaoHang,
                    BrandName = GetBrands().FirstOrDefault(o => o.Id == order.idBrand).Name
                }
            };
            //gửi mail thanh toán đơn hàng
            // new MailersController().GoiMailThanhToan(cartModel).Deliver();

            return View("Success", cartModel);
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

        public ActionResult ViewOrder(int id)
        {
            DetailMenuComment order = _detailMenuCommentRepository.GetById(id);
            if (order == null)
                return RedirectToRoute("Home");

            IList<DetailMenuCommentItem> details =
                _detailMenuCommentItemRepository.GetMany(o => o.id_Menu == order.id_).ToList();

            IList<CartViewModel.CartItem> cartItems = (from o in details
                                                       select new CartViewModel.CartItem
                                                       {
                                                           ProductId = Convert.ToInt32(o.Link),
                                                           Name = o.Name,
                                                           Image = o.Img,
                                                           Price = Convert.ToInt32(o.Price),
                                                           PricePromotion = Convert.ToInt32(o.PriceOf),
                                                           Barcode = o.BarCode,
                                                           Quantity = Convert.ToInt32(o.Number),
                                                       }).ToList();


            CartViewModel.OrderModel cartModel = new CartViewModel.OrderModel()
            {
                CartItems = cartItems,
                Order = order,

                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion),
                CartForm = new CartViewModel.CartForm()
                {
                    Name = order.Name,
                    Phone = order.Link,
                    Address = order.GiaoHang,
                    Email = order.HuongDanSuDung,
                    Note = order.Content,
                    ShipAddress = order.GiaoHang,
                    BrandName = GetBrands().FirstOrDefault(o => o.Id == order.idBrand).Name
                }
            };

            return View("ViewOrder", cartModel);
        }

        // Code Remarketing FaceBook
        public static string RemarkettingFacebookPurchase(int total, int orderIdForremarketting)
        {
            StringBuilder JavaScript = new StringBuilder();
            JavaScript = JavaScript.Append("<script>")
                .Append("fbq('track', 'Purchase', {")
                .Append("content_type:'product',")
                .Append("content_ids:['BGdetail_" + orderIdForremarketting + "'],")
                .Append("value:" + total + ",")
                .Append("currency:'VND'")
                .Append("});")
                .Append("</script>");
            return JavaScript.ToString();
        }
    }
}

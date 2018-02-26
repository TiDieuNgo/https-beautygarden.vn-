using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Helper;
using Shop.Web.Model;
using Shop.Web.ViewModels;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class CartController : BaseController
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductStockSyncRepository _productStockSyncRepository;
        private readonly IMenuOptionRepository _menuOptionRepository;

        public CartController(IMenuRepository menuRepository, IPromotionRepository promotionRepository, IProductStockSyncRepository productStockSyncRepository, IMenuOptionRepository menuOptionRepository, IDetailMenuRepository detailMenuRepository)
        {
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
            _productStockSyncRepository = productStockSyncRepository;
            _menuOptionRepository = menuOptionRepository;
            _detailMenuRepository = detailMenuRepository;
        }

        public ActionResult Index()
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();

            IList<ProductFrontPageMapping.ProductShowCart> productShowCarts =
                _menuRepository.GetProductShowCartByBarcode(productCarts.Select(o => o.Barcode).ToList());

            IList<CartViewModel.CartItem> cartItems = new List<CartViewModel.CartItem>();
            int gia = 0;
            IList<int> productId = new List<int>();
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
                            Quantity = o.Quantity <= 0 ? 1 : o.Quantity,
                            AttributeImg = item.AttributeImg,
                            AttributeName = item.AttributeName,
                            AttributeFlag = item.AttributeFlag,
                            Available = tonkho != null && tonkho.OnHand >= (o.Quantity <= 0 ? 1 : o.Quantity)
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
                        productId.Add(cartItem.ProductId); // lấy Id để remarketing facebook
                    }
                }
            }
            CartViewModel.CartModel cartModel = new CartViewModel.CartModel()
            {
                CartItems = cartItems,
                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion),
                Available = cartItems.All(o => o.Available),
                ListProductId = productId
            };
            return View("Index", cartModel);
        }

        public PartialViewResult CartView()
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<ProductFrontPageMapping.ProductShowCart> productShowCarts =
                _menuRepository.GetProductShowCartByBarcode(productCarts.Select(o => o.Barcode).ToList());

            IList<CartViewModel.CartItem> cartItems = new List<CartViewModel.CartItem>();
            if (productCarts.Any())
            {
                foreach (var o in productCarts)
                {
                    var item = productShowCarts.FirstOrDefault(p => p.Barcode.Equals(o.Barcode));
                    if (item != null)
                    {
                        var cartItem = new CartViewModel.CartItem()
                        {
                            ProductId = o.ProductId,
                            Name = item.NameProduct,
                            Image = item.Img,
                            Price = item.Price,
                            PricePromotion = item.Price,
                            Barcode = o.Barcode,
                            Quantity = o.Quantity <= 0 ? 1 : o.Quantity,
                            AttributeImg = item.AttributeImg,
                            AttributeName = item.AttributeName,
                            AttributeFlag = item.AttributeFlag,
                            Link = item.Link
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

            CartViewModel.CartModel cartModel = new CartViewModel.CartModel()
            {
                CartItems = cartItems,
                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion)
            };
            return PartialView("ViewPartial", cartModel);
        }

        [HttpPost]
        public JsonResult AddToCart(CartViewModel.ProductCart model)
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();

            #region cart
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }
            //neu trung id va ma vach thi cong thêm số lượng, ngược lại thì mới add cookie
            bool exist = productCarts.Any(o => o.ProductId == model.ProductId && o.Barcode.Equals(model.Barcode));
            if (exist)
            {
                foreach (var p in productCarts)
                {
                    if (p.ProductId == model.ProductId && p.Barcode.Equals(model.Barcode))
                    {
                        p.Quantity += model.Quantity;
                        break;
                    }
                }
            }
            else
            {
                productCarts.Add(model);
            }
            string jsonString = JsonConvert.SerializeObject(productCarts);
            cookieHelper.SetCartCookie(jsonString);
            #endregion

            int gia = 0;
            IList<ProductFrontPageMapping.ProductShowCart> productShowCarts =
                _menuRepository.GetProductShowCartByBarcode(productCarts.Select(o => o.Barcode).ToList());
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            IList<CartViewModel.CartItem> cartItems = new List<CartViewModel.CartItem>();
            if (productCarts.Any())
            {
                foreach (var o in productCarts)
                {
                    var item = productShowCarts.FirstOrDefault(p => p.Barcode.Equals(o.Barcode));
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
                        Price = item.Price,
                        PricePromotion = gia,
                        Barcode = o.Barcode,
                        Quantity = o.Quantity <= 0 ? 1 : o.Quantity,
                        AttributeImg = item.AttributeImg,
                        AttributeName = item.AttributeName,
                        AttributeFlag = item.AttributeFlag,
                        Link = item.Link
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

            CartViewModel.CartModel cartModel = new CartViewModel.CartModel()
            {
                CartItems = cartItems,
                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion)
            };

            string html = RenderPartialViewToString("ViewRender", cartModel);

            return Json(new { html = html, countCart = cartModel.CartItems.Count() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCart(CartViewModel.ProductCart model)
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();

            #region cart
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }

            foreach (var p in productCarts)
            {
                if (p.Barcode.Equals(model.Barcode))
                {
                    p.Quantity = model.Quantity == 0 ? 1 : model.Quantity;
                    break;
                }
            }
            string jsonString = JsonConvert.SerializeObject(productCarts);
            cookieHelper.SetCartCookie(jsonString);
            #endregion


            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCart(string barcode)
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }
            productCarts = productCarts.Where(o => !o.Barcode.Equals(barcode)).ToList();
            string jsonString = JsonConvert.SerializeObject(productCarts);
            cookieHelper.SetCartCookie(jsonString);

            return RedirectToRoute("Cart");
        }

        public JsonResult UpdateCartPopup()
        {

            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }
            PromotionMapping.PromotionCheck promotion = _promotionRepository.GetCurrentPromotion();

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
                            Quantity = o.Quantity <= 0 ? 1 : o.Quantity,
                            AttributeImg = item.AttributeImg,
                            AttributeName = item.AttributeName,
                            AttributeFlag = item.AttributeFlag,
                            Available = tonkho != null && tonkho.OnHand >= (o.Quantity <= 0 ? 1 : o.Quantity)
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
            CartViewModel.CartModel cartModel = new CartViewModel.CartModel()
            {
                CartItems = cartItems,
                Total = cartItems.Sum(o => o.Quantity * o.PricePromotion),
                Available = cartItems.All(o => o.Available)
            };

            string html = RenderPartialViewToString("ViewRender", cartModel);

            return Json(new { html = html, countCart = cartModel.CartItems.Count() }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult CountCart()
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }

            return PartialView("CartCount", productCarts.Count);
        }

        public PartialViewResult CountDropdownCart()
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }

            return PartialView("CartDropdownCount", productCarts.Count);
        }
        public ActionResult QuickAdd(int productId)
        {
            Menu menu = _menuRepository.GetById(productId);
            MenuOption menuOption = _menuOptionRepository.Get(o => o.IdMenu == productId);
            if (menu.BarcodeType == 0)
            {
                IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();

                #region cart
                CookieHelper cookieHelper = new CookieHelper("Cart");

                if (cookieHelper.GetCookie() != null)
                {
                    string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                    productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
                }

                bool exist = productCarts.Any(o => o.ProductId == productId && o.Barcode.Equals(menuOption.Barcode));
                if (exist)
                {
                    foreach (var p in productCarts)
                    {
                        if (p.ProductId == menu.id_ && p.Barcode.Equals(menuOption.Barcode))
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
                        Barcode = menuOption.Barcode,
                        ProductId = menu.id_,
                        Quantity = 1
                    });
                }
                string jsonString = JsonConvert.SerializeObject(productCarts);
                cookieHelper.SetCartCookie(jsonString);
                #endregion

                return RedirectToRoute("Cart");
            }
            else
            {
                return RedirectToRoute("ChiTietSanPham", new { splink = menu.Link });
            }
        }
        public ActionResult ShowTinKhuyenMai()
        {
            IList<DetailMenu> _tinshowgiohang =
                _detailMenuRepository.GetMany(
                    o =>
                    o.id_Menu == 20 && o.ok && o.ShowMenu && o.TinhTrangSP &&
                    o.ShowKhuyenMai == false && o.ShowTinKhuyenMai).OrderByDescending(o => o.sDate).Take(4).ToList();
            return View("ShowTinKhuyenMai", _tinshowgiohang);
        }
        public ActionResult GiaTotMoiNgay()
        {
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();

            IList<ProductFrontPageMapping.ProductBoxDungdeptrai> productBoxDungdeptrais = new List<ProductFrontPageMapping.ProductBoxDungdeptrai>();
            using (var context = new ShopDataContex())
            {
                var products =
                    context.Database.SqlQuery<ProductFrontPageMapping.ProductBoxDungdeptrai>("exec Product_GiaTotMoiNgay").ToList();

                if (products.Any())
                {
                    foreach (var sanpham in products)
                    {

                        ProductFrontPageMapping.ProductBoxDungdeptrai sp = new ProductFrontPageMapping.ProductBoxDungdeptrai
                        {
                            Img = sanpham.Img,
                            NameProduct = sanpham.NameProduct,
                            PricePro = sanpham.PricePro,
                            id_ = sanpham.id_,
                            Link = sanpham.Link
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

                        productBoxDungdeptrais.Add(sp);
                    }
                }

                SanPhamGiaTotMoiNgayModel model = new SanPhamGiaTotMoiNgayModel()
                {
                    Products = productBoxDungdeptrais,
                };

                return View("GiaTotMoiNgay", model);
            }


        }
        public PartialViewResult CountCartMobile()
        {
            IList<CartViewModel.ProductCart> productCarts = new List<CartViewModel.ProductCart>();
            CookieHelper cookieHelper = new CookieHelper("Cart");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["Cart"]);
                productCarts = JsonConvert.DeserializeObject<List<CartViewModel.ProductCart>>(json);
            }

            return PartialView("CartCountMobile", productCarts.Count);
        }
        public ActionResult SanPhamCoQuaTang()
        {
            IList<ProductFrontPageMapping.ProductBox> products = _menuRepository.GetProductQuaTang();
            IList<PromotionMapping.PromotionCheckProduct> promotions = _promotionRepository.GetActives();
            PromotionMapping.GetPromotion(promotions, products);
            QuaTangModel model = new QuaTangModel()
                                    {
                                        Products = products
                                    };
            return View("SanPhamCoQuaTang",model);
        }

       
        // Code Remarketing FaceBook
        public static string RemarkettingFacebookAddtocart(int total, IList<int> productId)
        {
            string productIditem = "";
            string dataJoin = "";
            List<string> dogs = new List<string>();
            if (productId.Any())
            {
                foreach (var id in productId)
                {
                    productIditem = "BGdetail_" + id;
                    dogs.Add(productIditem);
                    dataJoin = string.Join("','", dogs.ToArray());
                }
            }

            StringBuilder JavaScript = new StringBuilder();
            JavaScript = JavaScript.Append("<script>")
                .Append("fbq('track', 'AddToCart', {")
                .Append("content_type:'product',")
                .Append("content_ids:['" + dataJoin + "'],")
                .Append("value:" + total + ",")
                .Append("currency:'VND'")
                .Append("});")
                .Append("</script>");
            return JavaScript.ToString();
        }
    }
}

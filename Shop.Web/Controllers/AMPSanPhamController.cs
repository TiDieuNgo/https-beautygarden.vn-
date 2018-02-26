using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Extensions;
using Shop.Web.Core.Models;
using Shop.Web.ViewModels;


namespace Shop.Web.Controllers
{
     [WhitespaceFilter]
    public class AMPSanPhamController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly HttpContextBase _httpContext;
        private readonly IProductStockSyncRepository _stockSyncRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IDanhSachTagRepository _danhSachTagRepository;
        public readonly IThietLapRepository _ThietLapRepository;
        public AMPSanPhamController(IMenuRepository menuRepository, HttpContextBase httpContext, IProductStockSyncRepository stockSyncRepository, IPromotionRepository promotionRepository, IUserRatingRepository userRatingRepository, IDanhSachTagRepository danhSachTagRepository, IThietLapRepository thietLapRepository)
        {
            _menuRepository = menuRepository;
            _httpContext = httpContext;
            _stockSyncRepository = stockSyncRepository;
            _promotionRepository = promotionRepository;
            _userRatingRepository = userRatingRepository;
            _danhSachTagRepository = danhSachTagRepository;
            _ThietLapRepository = thietLapRepository;
        }

        public ActionResult Index(string linkspamp)
        {
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand("update Menu set GiaThiTruong = CASE WHEN GiaThiTruong IS NULL OR GiaThiTruong = 0 THEN (ISNULL(PricePro, 0) +(ISNULL(PricePro, 0) * (ABS(Checksum(NewID()) % 10)+ 10)/100)) WHEN GiaThiTruong < PricePro THEN (ISNULL(PricePro, 0) +(ISNULL(PricePro, 0) * (ABS(Checksum(NewID()) % 10)+ 10)/100)) ELSE GiaThiTruong END  where idControl=11 and Link ={0}", linkspamp);
            }
            ProductFrontPageMapping.ProductForViewDetail product = _menuRepository.GetForViewDetailByLink(linkspamp);
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

            SanPhamViewModel.DetailPageViewModel model = new SanPhamViewModel.DetailPageViewModel()
            {
                Product = product,
                Attributes = attributes,
                BranchItems = branchItems,
                ProductAvailable = branchItems.Any(o => o.Quantity > 0),
                CurrentBarcode = selected != null ? selected.Barcode : "",
                ThietLap = _ThietLapRepository.GetAll().FirstOrDefault(),
                LuotDanhGia = _userRatingRepository.GetLuotReView(linkspamp) < 200 ? _userRatingRepository.GetLuotReView(linkspamp) + 200 : _userRatingRepository.GetLuotReView(linkspamp),
                DanhSachTags = _danhSachTagRepository.GetTenTagViewDetail(linkspamp)

            };
        

            //chi co admin đăng nhập mới thấy đc số lượng còn hàng
            ShopUser efmvcUser = _httpContext.User.GetShopUser();
            model.IsAdmin = efmvcUser != null && efmvcUser.UserId != 0;

            ViewData["sao"] = _userRatingRepository.GetSao(linkspamp);

            //dung de xai data-src cho hinh anh trong chi tiet
            return View("Detail", model);
           
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
    }
}

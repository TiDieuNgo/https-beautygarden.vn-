using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.ViewModels
{
    public class SanPhamViewModel
    {
       public class BoxSanPhamKhuyenMaiViewModel
       {
           public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
           public bool HasPromotion { get; set; }
           public DateTime End { get; set; }
       }

       public class DetailPageViewModel
       {
           public ProductFrontPageMapping.ProductForViewDetail Product { get; set; }
           public IList<ProductFrontPageMapping.Attribute> Attributes { get; set; }
           public IList<SanPhamViewModel.BranchItem> BranchItems { get; set; }
           public bool ProductAvailable { get; set; }
           public string CurrentBarcode { get; set; }
           public ThietLap ThietLap { get; set; }
           public UserRating UserRating { get; set; }
           public int LuotDanhGia { get; set; }
           public IList<DanhSachTag> DanhSachTags { get; set; }
           public string Contentdatasrc { get; set; }
           public string Content1datasrc { get; set; }
           public string Content2datasrc { get; set; }
           public string Content3datasrc { get; set; }
           public string Content4datasrc { get; set; }
           public bool IsAdmin { get; set; }
           public Comment Comment { get; set; }
           public string TenDMbreadcrumb { get; set; }
           public string Linkbreadcrumb { get; set; }
            //public bool Bestseller { get; set; } // lay chu bestseller gan vao chi tiet cho a Hoang

            public static IList<Menu> GetDsTagsFromDanhMuc(int id)
           {
               using (var context = new ShopDataContex())
               {
                   string sql = string.Format("select * from Menu where id_ in (select idMenuCatelogy from MenuProAdd where idMenuProAdded={0})", id);
                   return context.Database.SqlQuery<Menu>(sql).ToList();
               }
           }
           public static int GetGiaThiTruong(string str)
           {
               if (string.IsNullOrEmpty(str))
               {
                   return 0;
               }
               int giatritruong = 0;
               //string str = "20,000";
               if (str.Length >= 9)
               {
                   return int.Parse(str.Replace(",", ""));
               }
               else
               {
                   str = str.Remove(str.IndexOf(','));
                   giatritruong = int.Parse(str);
               }

               return giatritruong * 1000;
           }
           public static float GetPhanTram(int giatt, int giagoc)
           {
               float phantram = (giatt - giagoc) * 100 / (giagoc == 0 ? 1 : giagoc);
               return phantram;
           }

           public static bool CheckSPBestSeller(int productId)
           {
                using (var context = new ShopDataContex())
                {
                    string sql = string.Format("select Bestseller from Menu where id_ = {0}", productId);
                    return context.Database.SqlQuery<bool>(sql).FirstOrDefault();
                }
            }
       }

       public class BranchItem
       {
           public int Id { get; set; }
           public string Name { get; set; }
           public int Quantity { get; set; }
           public string Region { get; set; }
       }

       public class CheckQuantity
       {
           public string Barcode { get; set; }
           public int ProductId { get; set; }
           public int Quantity { get; set; }
       }
       public class ProductBoxViewmodel
       {
           public IList<ProductFrontPageMapping.CategoryBoxHome> Categories { get; set; }
           public PromotionMapping.PromotionCheck Promotion { get; set; }
       }
       public class TopSeller
       {
           public IList<ProductFrontPageMapping.CategoryBoxHomeChild> MenuAndProducts { get; set; }
           public PromotionMapping.PromotionCheck Promotion { get; set; }
           public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        }
        public class CheckBrandQuantityModel
        {
            public IList<BranchItem> BranchItems { get; set; }
            public bool IsAdmin { get; set; }
        }
      
    }
}
using System.Collections.Generic;
using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class DanhMucSelectModel
    {
        public int DanhMucId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int ParentId { get; set; }
    }

    public class CategoryTreeModel
    {
        public IList<MenuCategoryTree> Categories { get; set; }
        public int ProductId { get; set; }
        public IList<int> SelectedIds { get; set; }

        public static int GetslHientai(int idanhmuc)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select COUNT(*) from Menu where Menu.idControl={0}", idanhmuc);
                return context.Database.SqlQuery<int>(sql).First();
            }
        }
        public static string GetTenSpById(int idsanpham)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select NameProduct from Menu where id_ = {0}", idsanpham);
                return context.Database.SqlQuery<string>(sql).FirstOrDefault();
            }
        }

        public static string GetLinkByIdSanPham(int idsanpham)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select Link from Menu where id_ = {0}", idsanpham);
                return context.Database.SqlQuery<string>(sql).FirstOrDefault();
            }
        }
    }
}
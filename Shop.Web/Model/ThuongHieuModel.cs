using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Model
{
    public class ThuongHieuModel
    {
        public IList<Menu> ThuongHieu { get; set; }
        public IList<ProductFrontPageMapping.BrandMapping> Brands { get; set; }

        public static int GetslHientai(string link)
        {
            using (var context = new ShopDataContex())
            {
                string sql = String.Format("select COUNT(*) from Menu where Menu.IdNhaCungCap=(select id_ from Menu where Menu.Link='{0}') and HasValue=1 and HasOnHand=1 and ok=1", link);
                return context.Database.SqlQuery<int>(sql).First();
            }
        }
    }
}
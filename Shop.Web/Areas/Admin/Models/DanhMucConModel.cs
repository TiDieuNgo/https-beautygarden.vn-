using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class DanhMucConModel
    {
        public PagedList.IPagedList<Menu> DanhMucs { get; set; }
        public Menu Menu { get; set; }

        public static int GetslHientai(int idanhmuc)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select COUNT(*) from Menu where Menu.idControl={0}", idanhmuc);
                return context.Database.SqlQuery<int>(sql).First();
            }
        }
    }
}
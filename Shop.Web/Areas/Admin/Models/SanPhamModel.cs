using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class SanPhamModel
    {
        public PagedList.IPagedList<Menu> Menus { get; set; }
        public int soluongtimduoc { get; set; }
        public int stt { get; set; }
        public int page { get; set; }
    }
}
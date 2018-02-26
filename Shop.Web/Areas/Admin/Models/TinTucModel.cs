using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class TinTucModel
    {
        public PagedList.IPagedList<DetailMenu> DetailMenus { get; set; }
        public int soluongtimduoc { get; set; }
    }

    public class DanhSachTagModelTimKiem
    {
        public PagedList.IPagedList<DanhSachTag> DanhSachTags { get; set; }
        public int soluongtimduoc { get; set; }
    }
    public class TagTinTucModelTimKiem
    {
        public PagedList.IPagedList<TagTinTuc> DanhSachTags { get; set; }
        public int soluongtimduoc { get; set; }
    }
}

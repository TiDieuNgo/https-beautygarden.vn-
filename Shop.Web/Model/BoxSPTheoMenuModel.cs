using System.Collections.Generic;
using Shop.Model;
using Shop.Web.Model;

namespace Shop.Web.Models
{
    public class BoxSPTheoMenuModel
    {
        public IList<SanPhamTheoDanhMuc> SanPhams { get; set; }
        public Menu DanhMucSanPham { get; set; }
        public IList<Menu> DanhSachMenuCon { get; set; }
        public IList<SanPhamTheoDanhMuc> SanPhamNoiBatHomes { get; set; }
        public IList<SanPhamTheoDanhMuc> SanPhamBanChayHomes { get; set; }
    }
}
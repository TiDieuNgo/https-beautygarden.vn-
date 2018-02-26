using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Models
{
    public class DanhMucTienIchViewModel
    {
        public IList<DanhMucTienIch> DanhMucs { get; set; }
        public Menu DanhMuc { get; set; }
    }
}
using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Model
{
    public class BannerKhuyenMaiModel
    {
        public IList<BannerKhuyenMai> BannerKhuyenMais { get; set; }
        public BannerKhuyenMai Banner { get; set; }
    }
}
using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class OrderModel
    {
        public DetailMenuComment DonHang { get; set; }
        public IList<DetailMenuCommentItem> ChiTietDonHangs { get; set; }
    }
}
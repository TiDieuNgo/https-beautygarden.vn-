using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Web.Areas.Admin.Models
{
    public class HeThongCuaHangModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập địa chỉ.")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên shop.")]
        public string TenShop { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình ảnh.")]
        public string HinhAnh { get; set; }
        public DateTime NgayTao { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int SapXep { get; set; }
    }
}
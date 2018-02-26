using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("HeThongCuaHangs")]
    public class HeThongCuaHang
    {
        [Key]
        public int Id { get; set; }
        public string DiaChi { get; set; }
        public string TenShop { get; set; }
        public string HinhAnh { get; set; }
        public DateTime NgayTao { get; set; }
        public int SapXep { get; set; }
    }
}

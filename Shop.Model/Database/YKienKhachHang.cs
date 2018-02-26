using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("YKienKhachHangs")]
    public class YKienKhachHang
    {
        [Key]
        public int id_ { get; set; }
        public string TenKhachHang { get; set; }
        public string HinhDaiDien { get; set; }
        public string NoiDung { get; set; }
        public string MoTa { get; set; }
        public DateTime NgayTao { get; set; }
        public string LinkFaceBook { get; set; }
    }
}

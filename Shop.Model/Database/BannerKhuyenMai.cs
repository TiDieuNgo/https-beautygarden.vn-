using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("BannerKhuyenMais")]
    public class BannerKhuyenMai
    {
        [Key]
        public int Id { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string Ten { get; set; }
        public bool Showhide { get; set; }
        public DateTime NgayTao { get; set; }
    
    }

}

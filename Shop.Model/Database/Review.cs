using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public int id_ { get; set; }
        public string TieuDe { get; set; }
        public string Mota { get; set; }
        public int Sapxep { get; set; }
        public bool Ok { get; set; }
        public string HinhAnh { get; set; }
        public string ChiTiet { get; set; }
        public string NguoiTao { get; set; }
        public DateTime Sdate { get; set; }
        public string Link { get; set; }
        public string SEOtitle { get; set; }
        public string SEODescription { get; set; }
    }
}

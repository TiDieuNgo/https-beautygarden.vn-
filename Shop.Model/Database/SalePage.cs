using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("SalePages")]
    public class SalePage
    {
        [Key]
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string Link { get; set; }
        public string SDT { get; set; }
        public string CodeName { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDecription { get; set; }
        public string KeyWord { get; set; }
        public DateTime NgayTao { get; set; }
        public string HinhAnh { get; set; }
        public string NoiDung1 { get; set; }
        public string NoiDung2 { get; set; }
        public string NoiDung3 { get; set; }
        public string NoiDung4 { get; set; }
        public bool Showhide { get; set; }
    }
}

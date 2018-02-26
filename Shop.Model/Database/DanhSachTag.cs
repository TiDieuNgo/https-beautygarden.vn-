using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shop.Model
{
    [Table("DanhSachTags")]
    public class DanhSachTag
    {
        [Key]
        public int Id { get; set; }
        public string TenTag { get; set; }
        public int IdMenu { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public string SEODescription { get; set; }
        public string SEOtitle { get; set; }
        public string Code { get; set; }
    }
    public  class TagName
    {
        public string TenTag { get; set; }
    }
 
}

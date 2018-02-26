using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("BannerDanhSachs")]
    public class BannerDanhSach
    {
        [Key]
        public int id_ { get; set; }
        public int IdDanhMuc { get; set; }
        public string TenBanner { get; set; }
        public string HinhAnh { get; set; }
        public int ViTri { get; set; }
        public bool BannerQCDanhSach { get; set; }
        public bool BannerQCChiTiet { get; set; }
        public string LinkHttp1 { get; set; }
        public bool Ok { get; set; }
    }


}

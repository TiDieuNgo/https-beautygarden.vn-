using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("DetailMenu")]
    public class DetailMenu
    {
        [Key]
        public int id_ { get; set; }
        public int id_Menu { get; set; }
        public int idControl { get; set; }
        public int idBrand { get; set; }
        public string Ma_Hang { get; set; }
        public string Name { get; set; }
        public string Name02 { get; set; }
        public string NameAdwords { get; set; }
        public string Link { get; set; }
        public string Link02 { get; set; }
        public string LinkHTTP { get; set; }
        public string Code { get; set; }
        public string Img { get; set; }
        public string File { get; set; }
        public string Brochure { get; set; }
        public string Driver { get; set; }
        public string Video { get; set; }

        public string Price { get; set; }
        public string PriceOf { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public string HuongDanSuDung { get; set; }
        public string GiaoHang { get; set; }
        public string Diagram { get; set; }
        public string BaoHanh { get; set; }
        public string KhuyenMai { get; set; }
        public string BanHangChuan { get; set; }
        public double? Number { get; set; }
        public int? sPosition { get; set; }
        public bool ok { get; set; }
        public bool ShowMenu { get; set; }
        public bool ShowKhuyenMai { get; set; }
        public bool ShowIconNew { get; set; }

        public bool ShowIconHot { get; set; }
        public bool TinhTrangSP { get; set; }
        public int idUser { get; set; }
        public int idUserOk { get; set; }
        public DateTime sDateOk { get; set; }
        public DateTime sDate { get; set; }
        public string CodeName { get; set; }
        public bool ShowTinKhuyenMai { get; set; }
        public string SEOtitle { get; set; }
        public string SEODescription { get; set; }
        public int IdSalePage { get; set; }
        public string NguoiTao { get; set; }
    }
    public class TinTucMapping
    {
        public int id_ { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime sDate { get; set; }
        public int DanhMucTinTucId { get; set; }
        public string TenDanhMuc { get; set; }
    }
}

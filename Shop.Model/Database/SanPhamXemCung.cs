using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("SanPhamXemCungs")]
    public class SanPhamXemCung
    {
        [Key]
        public int SanPhamXemCungId { get; set; }
        public int IdSanPham { get; set; }
        public int IdSanPhamTiepTheo { get; set; }
        public int SoLanXem { get; set; }
    }
}

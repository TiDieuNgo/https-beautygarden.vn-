using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("NoiDungTienIchs")]
    public class NoiDungTienIch
    {
        [Key]
        public int id_ { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public int IdDanhMuc { get; set; }
        public string HinhAnh { get; set; }
    }
}

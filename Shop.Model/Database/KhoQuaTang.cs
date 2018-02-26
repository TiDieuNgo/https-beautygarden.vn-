using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("KhoQuaTangs")]
    public class KhoQuaTang
    {
        [Key]
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public string IdSanPhamTang { get; set; }
        public DateTime NgayTao { get; set; }
    }
}

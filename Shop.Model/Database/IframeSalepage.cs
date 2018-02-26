using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("IframeSalepages")]
    public class IframeSalepage
    {
        [Key]
        public int Id { get; set; }
        public string Iframe { get; set; }
        public string IframeGia { get; set; }
        public string Barcode { get; set; }
        public DateTime NgayTao { get; set; }
    }
}

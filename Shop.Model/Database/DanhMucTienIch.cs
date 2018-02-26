using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("DanhMucTienIchs")]
    public class DanhMucTienIch
    {
        [Key]
        public int id_ { get; set; }
        public string TenDanhMuc { get; set; }
        public string IconDanhMuc { get; set; }
        public DateTime NgayTao { get; set; }
        public int Idcontrol { get; set; }
        public bool ok { get; set; }
       public int SapXep { get; set; }
        
    }

}

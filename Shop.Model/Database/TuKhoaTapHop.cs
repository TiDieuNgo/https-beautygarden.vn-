using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("TuKhoaTapHops")]
    public class TuKhoaTapHop
    {
        [Key]
        public int Id { get; set; }
        public int IdTuKhoa { get; set; }
        public string TuKhoa { get; set; }
        public string TuKhoaKhongDau { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}

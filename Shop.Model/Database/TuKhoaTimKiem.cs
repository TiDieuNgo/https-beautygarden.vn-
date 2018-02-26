using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("TuKhoaTimKiems")]
    public class TuKhoaTimKiem
    {
        [Key]
        public int Id { get; set; }
        public string TuKhoa { get; set; }
        public string TuKhoaKhongDau { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
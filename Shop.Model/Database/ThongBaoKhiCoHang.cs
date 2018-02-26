using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("ThongBaoKhiCoHangs")]
    public class ThongBaoKhiCoHang
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FullName { get; set; }
        public DateTime Created { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}

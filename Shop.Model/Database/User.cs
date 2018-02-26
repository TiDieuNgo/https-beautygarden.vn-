using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string HoTen { get; set; }
        [Required]
        public string TenDangNhap { get; set; }
        [Required]
        public string MatKhau { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; }     

    }
}

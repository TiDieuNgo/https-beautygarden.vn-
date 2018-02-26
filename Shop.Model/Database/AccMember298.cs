using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("AccMember298")]
    public class AccMember298
    {
        [Key]
        public int id_ { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public DateTime Birdthday { get; set; }
        public string Address { get; set; }
        public string States { get; set; }
        public int Level{ get; set; }
        public bool Show { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string TenCongTy { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChiCongTy { get; set; }
        public DateTime sDate { get; set; }
        public DateTime sDateOk { get; set; }
        public int idUser { get; set; }
        public int idUserOk { get; set; }
    }
 
}

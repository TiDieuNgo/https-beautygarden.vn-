using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("SEOFooters")]
    public class SEOFooter
    {
        [Key]
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string Link { get; set; }
        public bool Showhide { get; set; }
        public DateTime NgayTao { get; set; }
    }
 
}

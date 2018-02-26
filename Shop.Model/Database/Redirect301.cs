using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("Redirect301s")]
    public class Redirect301
    {
        [Key]
        public int Redirect301Id { get; set; }
        public string LinkCu { get; set; }
        public string LinkMoi { get; set; }
        public DateTime NgayTao { get; set; }
    }
}

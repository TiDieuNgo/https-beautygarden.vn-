using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("TagTinTucs")]
    public class TagTinTuc
    {
        [Key]
        public int Id { get; set; }
        public string TenTag { get; set; }
        public int IdMenu { get; set; }
        public DateTime NgayTao { get; set; }
        public string Link { get; set; }
        public string Seodcription { get; set; }
        public string Seotitle { get; set; }
        public string Code { get; set; }
    }
}

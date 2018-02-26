using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("MenuOption")]
    public class MenuOption
    {
        [Key]
        public int id_ { get; set; }
        public int IdMenu { get; set; }
        public string Barcode { get; set; }
        public string Img { get; set; }
        public string TenLoai { get; set; }
        public int Flag { get; set; }
        public DateTime SDate { get; set; }
        public DateTime sDateOk { get; set; }
        public string NameProduct { get; set; }
        public string IdKiotviet { get; set; }
    }

    public class MenuOptionMapping
    {
        public class OptionShow
        {
            public int id_ { get; set; }
            public string Barcode { get; set; }
            public string Img { get; set; }
            public string TenLoai { get; set; }
            public int Flag { get; set; }
        }
    }
}

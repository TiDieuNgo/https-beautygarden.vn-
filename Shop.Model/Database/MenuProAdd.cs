using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("MenuProAdd")]
    public class MenuProAdd
    {
        [Key]
        public int id_ { get; set; }
        public int idMenuCatelogy { get; set; }
        public int idMenuProAdded { get; set; }
        public int idMenu { get; set; }
        public int sPosition { get; set; }
        public DateTime sDate { get; set; }
        public DateTime sDateOk { get; set; }
        public bool isStyle { get; set; }
        public string Style { get; set; }
        public int idUser { get; set; }
        public int idUserOk { get; set; }
    }
    public class SanPhamTheoDanhMucMapping
    {
        //join 2 bang menu va menuproadd
        public int id_ { get; set; }
        public int LevelMenu { get; set; }
        public bool ok { get; set; }
        public DateTime sDate { get; set; }
        public int PriceOffPro { get; set; }
        public bool HasValue { get; set; }
        public bool HasOnHand { get; set; }
        public Int32? PricePro { get; set; }//khong set gia tri not null
        public string NameProduct { get; set; }
        public string IconMenu { get; set; }//hinh truoc danh muc trong menu
        public string Img { get; set; }
        public int idMenuCatelogy { get; set; }
        public int idMenuProAdded { get; set; }
        public int idControl { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("TagsLink")]
    public class TagsLink
    {
        [Key]
        public int id_ { get; set; }
        public string Link { get; set; }
        public string TenTags { get; set; }
        public DateTime SDate { get; set; }
        public int IdMenu { get; set; }
        public bool ok { get; set; }
        public DateTime sDateOk { get; set; }
        public int idUser { get; set; }
        public int idUserOk { get; set; }
        public int idControl { get; set; }
        //public virtual Menu Menu { get; set; }
    }
    public class TagMapping
    {
        public int id_ { get; set; }
        public string TenTags { get; set; }
        public int idMenu { get; set; }//DanhMucTinTucId
        public string Link { get; set; }
        public string TenSanPham { get; set; }
    }
}

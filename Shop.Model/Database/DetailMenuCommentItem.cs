using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("DetailMenuCommentItem")]
    public class DetailMenuCommentItem
    {
        [Key]
        public int id_ { get; set; }
        public int id_Menu { get; set; }
        public int idControl { get; set; }
        public string Name { get; set; }
        public string Name02 { get; set; }
        public string Link { get; set; }
        public string Link02 { get; set; }
        public string Price { get; set; }
        public string PriceOf { get; set; }
        public string Note { get; set; }

        public string Content { get; set; }
        public double Number { get; set; }
        public int sPosition { get; set; }
        public bool ok { get; set; }
        public int idUser { get; set; }
        public int idUserOk { get; set; }
        public DateTime sDate { get; set; }
        public DateTime sDateOk { get; set; }
        public string BarCode { get; set; }
        public string IdKiotViet { get; set; }
        public string Img { get; set; }
        [ForeignKey("id_Menu")]//dung khoa ngoai moi lien ket duoc
        public virtual DetailMenuComment DonHang { get; set; }
    }

}

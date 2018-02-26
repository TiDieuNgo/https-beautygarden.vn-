using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public int IdSanPham { get; set; }
        public string NoiDungCmt { get; set; }
        public DateTime NgayCmt { get; set; }
        public bool Ok { get; set; }

       
    }
     
 
}

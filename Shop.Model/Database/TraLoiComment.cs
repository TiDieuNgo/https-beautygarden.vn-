using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("TraLoiComments")]
    public class TraLoiComment
    {
        [Key]
        public int TraLoiCommentId { get; set; }
        public int CommentId { get; set; }
        public string NoiDungCmt { get; set; }
        public DateTime NgayCmt { get; set; }
      
    }
}

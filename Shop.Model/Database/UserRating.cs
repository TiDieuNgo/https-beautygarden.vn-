using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("UserRatings")]
    public class UserRating
    {
        [Key]
        public int Id { get; set; }
        public float Rating { get; set; }
        public int IdSanPham { get; set; }
        public string UserAcount { get; set; }
    }
}

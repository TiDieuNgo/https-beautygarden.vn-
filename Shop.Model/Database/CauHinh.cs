using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("CauHinhs")]
    public class CauHinh
    {
        [Key]
        public int Id { get; set; }
        public int Loai { get; set; }
        public string NoiDung { get; set; }
        public string TieuDe { get; set; }
    }
 
}

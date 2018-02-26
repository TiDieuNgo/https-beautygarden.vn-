using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("ThietLaps")]
    public class ThietLap
    {
        public int ThietLapId { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
    }
}

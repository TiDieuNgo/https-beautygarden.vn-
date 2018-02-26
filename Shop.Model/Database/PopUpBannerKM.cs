using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("PopUpBannerKMs")]
    public class PopUpBannerKM
    {
        [Key]
        public int Id { get; set; }
        public string HinhAnh { get; set; }
        public string TieuDe { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public bool Active { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
    }

    public class PopUpBannerKMModel
    {
        public int Id { get; set; }
        public string HinhAnh { get; set; }
        public string TieuDe { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public bool Active { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }
    }

}

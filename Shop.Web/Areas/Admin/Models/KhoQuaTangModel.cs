using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Models
{
    public class KhoQuaTangModel
    {
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public string IdSanPhamTang { get; set; }
        public DateTime NgayTao { get; set; }
        public IList<SelectListItem> SanPhams { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Shop.Web.Areas.Admin.Models
{
    public class TaglinkModel
    {
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
        public IList<SelectListItem> DanhMucs { get; set; }
    }
}
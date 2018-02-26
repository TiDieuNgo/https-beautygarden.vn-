using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class BarcodeModel
    {
        public IList<MenuOptionMapping.OptionShow> OptionShows { get; set; }
        public string Barcode { get; set; }
        public short Type { get; set; }
    }
}
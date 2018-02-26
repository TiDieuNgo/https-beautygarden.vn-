using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class EditMaVachWinDowsOpen
    {
        public IList<MenuOption> DanhSachMaVachs { get; set; }
        public MenuOption MenuOption { get; set; }
        public int Flag { get; set; }
        public int CountMaVach { get; set; }
    }
}
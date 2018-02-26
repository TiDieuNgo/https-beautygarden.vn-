using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Shop.Model;

namespace Shop.Web.ViewModels
{
    public class CategoryFormModel
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }       
    }
    public class MenuLeveModel
    {
        public IList<Menu> Menus { get; set; }
    }
}
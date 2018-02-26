using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("MenuImage")]
    public class MenuImage
    {
        [Key]
        public int id { get; set; }
        public int idMenu { get; set; }
        public string ImageName { get; set; }
        public int sPosition { get; set; }
        public DateTime date { get; set; }

    }

    public class MenuImageMapping
    {
        public class MenuImage
        {
            public int id { get; set; }
            public string ImageName { get; set; }

        }
    }
  
}

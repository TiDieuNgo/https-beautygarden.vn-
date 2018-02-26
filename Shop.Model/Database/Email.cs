using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("Emails")]
    public class Email
    {
        [Key]
        public int id_ { get; set; }
        public string Emails { get; set; }
       public DateTime SDate { get; set; }
    
    }

}

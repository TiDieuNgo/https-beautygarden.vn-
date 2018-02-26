using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
     [Table("Account298")]
    public class Account298
    {
        [Key]
       
         public int id_ { get; set; }
      
        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }
        public int Permission { get; set; }
        public bool Show { get; set; }     

        //tao them 2 field để phân quyền
        public bool IsAdmin { get; set; }
        public string Roles { get; set; }

    }
}

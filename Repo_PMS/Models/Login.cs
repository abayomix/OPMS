using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Repo_PMS.Models
{
    public class Login
    {

        [Display(Name = "User ID")]
        public string UserID { get; set; }

        public string Password { get; set; }


    }
}

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Repo_PMS.Models
{
    public class ChangePasswordVM
    {

        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "New Password")]
        public string NewPasswod { get; set; }


    }
}

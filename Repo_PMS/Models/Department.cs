using System.ComponentModel.DataAnnotations;

namespace Repo_PMS.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DeptName { get; set; }
        [Required]
        [StringLength(100)]
        public string DeptDescription { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? CreatedBy { get; set; }



    }
}

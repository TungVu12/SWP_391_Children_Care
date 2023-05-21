using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Children
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Họ và tên đệm")]
        public string FirstName { get; set; }
        [Required]

        [Display(Name = "Tên")]
        public string LastName { get; set; }
        public DateTime? BirdthDate { get; set; }
        public int Age { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
    }
}

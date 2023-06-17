using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Children
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Họ và tên đệm")]
        public string FirstName { get; set; }


        [Display(Name = "Tên")]
        public string LastName { get; set; }
        public DateTime? BirdthDate { get; set; }
        public int Age { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }


        public string parent_id { get; set; }


    }
}

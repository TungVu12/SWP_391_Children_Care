using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Children
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Họ và tên đệm")]
        public string? FirstName { get; set; }

        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        public DateTime? BirdthDate { get; set; }
        [Display(Name = "Tuổi")]
        public int Age { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Thông tin khác")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Phụ huynh")]
        public string parent_id { get; set; }
        public DateTime? CreateDate { get; set; }


    }
}

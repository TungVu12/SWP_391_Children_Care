using ASP_NET_MVC_Ver1.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Doctor Name")]
        public string doctor_id { get; set; }
        [Required]
        [Display(Name = "Child Name")]
        public string child_id { get; set; }
        [Required]
        [Display(Name = "Parent Name")]
        public string parent_id { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string category { get; set; }
        [Required]
        [Display(Name = "Tên hồ sơ")]
        public string r_title { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string r_content { get; set; }
        public DateTime? create_date { get; set; }
        [Required]
        [Display(Name = "Create Date")]
        public DateTime? booking_date { get; set; }
        [Display(Name = "Slot")]
        public int? slot { get; set; }
        [Required]
        [Display(Name = "Status Booking")]
        public StatusSending status { get; set; }
        [Display(Name = "Desciption")]
        public string Desciption { get; set; }
        [Display(Name = "Examination Status")]
        public StatusExamination examination_status { get; set; }
        [NotMapped]
        public int backup { get; set; }


    }
}

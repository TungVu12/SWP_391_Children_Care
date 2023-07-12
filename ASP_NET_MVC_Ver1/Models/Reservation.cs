using ASP_NET_MVC_Ver1.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Bác sĩ")]
        public string doctor_id { get; set; }
        [Required]
        [Display(Name = "Trẻ em")]
        public string child_id { get; set; }
        [Required]
        [Display(Name = "Phụ huynh")]
        public string parent_id { get; set; }
        [Required]
        [Display(Name = "Khoa khám")]
        public string category { get; set; }
        [Required]
        [Display(Name = "Tên hồ sơ")]
        public string r_title { get; set; }
        [Required]
        [Display(Name = "Thông tin khám chữa bệnh")]
        public string r_content { get; set; }
        public DateTime? create_date { get; set; }
        [Required]
        [Display(Name = "Ngày đặt lịch")]
        public DateTime? booking_date { get; set; }
        [Display(Name = "Ca khám bệnh")]
        public int? slot { get; set; }
        [Required]
        [Display(Name = "Tình trạng đặt lịch")]
        public StatusSending status { get; set; }
        [Display(Name = "Thông tin mô tả")]
        public string Desciption { get; set; }


    }
}

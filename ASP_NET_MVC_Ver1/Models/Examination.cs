using System.ComponentModel.DataAnnotations;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Examination
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Ngày khám bệnh")]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [Display(Name = "Phụ huynh")]
        public string parent_id { get; set; }
        [Required]
        [Display(Name = "Doctor")]
        public string doctor_id { get; set; }
        [Required]
        [Display(Name = "Trẻ em")]
        public string child_id { get; set; }
        [Required]
        [Display(Name = "Lịch khám")]
        public string reservation_id { get; set; }
        public string Detail { get; set; }
        [Required]
        [Display(Name = "Khoa khám")]
        public string category { get; set; }
        [Required]
        [Display(Name = "Thông tin khám chữa bệnh")]
        public string r_content { get; set; }
        [Display(Name = "Thông tin mô tả")]
        public string Desciption { get; set; }
    }
}

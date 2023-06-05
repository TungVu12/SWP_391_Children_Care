using System.ComponentModel.DataAnnotations;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Examination
    {
        [Key]
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ChilderName { get; set; }
        public string ParentName { get; set; }
        public string Sex { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string?Email { get; set; }
        public string Address { get; set; }

        public string Detail { get; set; }
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }
    }
}

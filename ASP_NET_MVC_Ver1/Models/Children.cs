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
        [Required]

        public string parent_id { get; set; }

        public Children(int id, string firstName, string lastName, DateTime? birdthDate, int age, string? description, string address, string parent_id)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirdthDate = birdthDate;
            Age = age;
            Description = description;
            Address = address;
            this.parent_id = parent_id;
        }
    }
}

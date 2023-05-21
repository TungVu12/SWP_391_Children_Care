using System.ComponentModel.DataAnnotations;

namespace ASP_NET_MVC_Ver1.Models
{
    public class User
    {
        [Required]   
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
            
           
    }
}

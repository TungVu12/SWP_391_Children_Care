using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;


namespace ASP_NET_MVC_Ver1.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Creator { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Xml.Linq;


namespace ASP_NET_MVC_Ver1.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string CreateId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [DefaultValue(null)]
        public string Description { get; set; }


    }
}
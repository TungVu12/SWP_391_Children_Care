using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace ASP_NET_MVC_Ver1.Models
{
    public class Category
    {
        [Key]
        public int c_id { get; set; }
        [Required]
        public string creator_id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string content { get; set; }

    }
}
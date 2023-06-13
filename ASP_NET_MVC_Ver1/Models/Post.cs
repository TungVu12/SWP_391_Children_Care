using System.ComponentModel.DataAnnotations;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int CreatorId { get; set; }
        [MaxLength(500)]
        public string Image { get; set; }
    }
}

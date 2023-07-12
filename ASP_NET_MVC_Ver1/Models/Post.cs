using System.ComponentModel.DataAnnotations;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

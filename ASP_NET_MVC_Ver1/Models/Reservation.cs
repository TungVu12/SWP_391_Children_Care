using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Reservation
    {
        [Key]
        public int r_id { get; set; }

        [Required]
        public int u_id { get; set; }

        [Required]
        public string r_title { get; set;}
        [Required]
        public string r_content { get; set;}
        [Required]
        public DateTime create_date { get; set; }

        public Reservation() { }

        public Reservation(int r_id, int u_id, string r_title, string r_content, DateTime create_date)
        {
            this.r_id = r_id;
            this.u_id = u_id;
            this.r_title = r_title;
            this.r_content = r_content;
            this.create_date = create_date;
        }
    }
}

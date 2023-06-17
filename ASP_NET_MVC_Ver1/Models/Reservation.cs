using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Reservation
    {
        [Key]
        public Guid reservation_id { get; set; }

        public string doctor_id { get; set; }

        public string child_id { get; set; }

        public string parent_id { get; set; }

        public string r_title { get; set; }
        public string r_content { get; set; }
        public DateTime create_date { get; set; }

        public int status { get; set; }


    }
}

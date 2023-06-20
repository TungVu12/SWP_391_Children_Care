using ASP_NET_MVC_Ver1.Enum;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_NET_MVC_Ver1.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }

        public string doctor_id { get; set; }

        public string child_id { get; set; }

        public string parent_id { get; set; }

        public string doctor_name { get; set; }

        public string child_name { get; set; }

        public string parent_name { get; set; }

        public string category { get; set; }

        public string r_title { get; set; }
        public string r_content { get; set; }
        public DateTime create_date { get; set; }
        public DateTime booking_date { get; set; }


        public StatusSending status { get; set; }


    }
}

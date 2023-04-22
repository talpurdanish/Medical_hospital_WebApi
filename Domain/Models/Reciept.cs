using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Reciept
    {
        [Key]
        public int RecieptId { get; set; }
        public DateTime RecieptDate { get; set; }
        public string RecieptTime { get; set; } = "";
        public double RecieptDiscount { get; set; }
        //public User AuthorizeBy { get; set; } = new User();
        public int AuthorizedById { get; set; }
        public bool Paid { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int PatientId { get; set; }
        
        public double GrandTotal { get; set; }
        public double Total { get; set; }

    }
}

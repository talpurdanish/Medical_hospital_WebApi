using System.ComponentModel.DataAnnotations;


namespace Domain.Models
{
    public class Appointment
    {
        [Key]
        public int Appointmentid { get; set; }
        [Required]
        public DateTime AppointDate { get; set; }
        [Required]
        public string StartDtTime { get; set; } = "";
        public DateTime? AppointEndDate { get; set; }
        public string EndDtTime { get; set; } = "";
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PatientId { get; set; }

        public int? RecieptId { get; set; }


    }
}

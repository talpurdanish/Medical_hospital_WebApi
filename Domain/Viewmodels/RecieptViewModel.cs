using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class RecieptViewModel :ViewmodelBase
    {


        public int ID { get; set; }

        public string RecieptNumber { get; set; } = "";

        [Required(ErrorMessage = "Patient Id is Required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor Id is Required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor Id is Required")]
        public int AppointmentId { get; set; }

        public string PatientName { get; set; } = "";
        public string PatientNumber { get; set; } = "";

        public string Doctor { get; set; } = "";

        public string Date { get; set; } = "";
        public string Time { get; set; } = "";



        public string AuthorizedBy { get; set; } = "";
        public int AuthorizedById { get; set; }

        public double Discount { get; set; }
        public double Total { get; set; }
        public double GrandTotal { get; set; }

        public string Appointment { get; set; } = "";

        public bool Paid { get; set; }
        public IList<ProcedureViewModel> Procedures { get; set; } = new List<ProcedureViewModel>();
       


    }
}

using Domain.Models;

namespace Domain.Viewmodels
{
    public class AddPrescriptionViewModel
    {

        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Medications { get; set; } = string.Empty;
        public string Tests { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = "";
        public string Remarks { get; set; } = "";
        public string Bp { get; set; } = string.Empty;
        public double Pulse { get; set; }
        public double Bsr { get; set; }
        public double Temp { get; set; }
        public double Wt { get; set; }
        public double Ht { get; set; }

    }
}

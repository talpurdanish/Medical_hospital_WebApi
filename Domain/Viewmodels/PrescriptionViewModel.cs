using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class PrescriptionViewModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Appointmentid { get; set; }
        public string Date { get; set; } = "";

        public string PatientName { get; set; } = "";
        public string PatientNumber { get; set; } = "";
        public int PatientId { get; set; }

        public string StartTime { get; set; } = "";
        public int DoctorId { get; set; }

        public string Doctor { get; set; } = "";

        public string MedString { get; set; } = "";
        public IList<PrescriptionMedicationViewModel> Medstrings { get; set; } = new List<PrescriptionMedicationViewModel>();
        public IList<string> Tests { get; set; } = new List<string>();
        public string Diagnosis { get; set; } = "";
        public string Remarks { get; set; } = "";

        public string Bp { get; set; }= string.Empty;
        public double Pulse { get; set; }
        public double Bsr { get; set; }
        public double Temp { get; set; }
        public double Wt { get; set; }
        public double Ht { get; set; }
        public static PrescriptionViewModel GenerateViewModel(Prescription model)
        {
            PrescriptionViewModel viewModel = new()
            {
                Appointmentid = model.Appointmentid
            };
            return viewModel;
        }

        public static Prescription GenerateModel(PrescriptionViewModel viewmodel)
        {
            Prescription model = new();
            if (viewmodel.ID > 0)
            {
                model.PrescriptionId = viewmodel.ID;
            }
            model.Appointmentid = viewmodel.Appointmentid;
            model.Diagnosis = viewmodel.Diagnosis;
            model.Remarks = viewmodel.Remarks;
            model.Wt = viewmodel.Wt;
            model.Ht = viewmodel.Ht;
            model.Temp = viewmodel.Temp;
            model.Bp = viewmodel.Bp;
            model.Bsr = viewmodel.Bsr;
            model.Pulse = viewmodel.Pulse;

            return model;
        }
    }
}

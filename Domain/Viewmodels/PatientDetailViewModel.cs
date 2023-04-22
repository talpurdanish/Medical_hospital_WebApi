
namespace Domain.Viewmodels
{
    public class PatientDetailViewModel
    {
        public IEnumerable<AppointmentViewModel> Appointments { get; set; }
        public IEnumerable<LabReportViewModel> Labreports { get; set; }
        public IEnumerable<RecieptViewModel> Reciepts { get; set; }
        public IEnumerable<PrescriptionViewModel> Prescriptions { get; set; }
        public PatientViewModel Patient { get; set; }

        public PatientDetailViewModel()
        {
            Appointments = new List<AppointmentViewModel>();
            Labreports = new List<LabReportViewModel>();
            Reciepts = new List<RecieptViewModel>();
            Prescriptions = new List<PrescriptionViewModel>();
            Patient = new PatientViewModel();

        }
    }
}

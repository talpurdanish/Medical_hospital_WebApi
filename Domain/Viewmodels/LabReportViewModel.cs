
using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Domain.Viewmodels
{
    public class LabReportViewModel : ViewmodelBase
    {

        public int Id { get; set; }
        
        [DisplayName("Date")]
        public string ReportDate { get; set; } = "";
        [DisplayName("Time")]
        public string ReportTime { get; set; } = "";
        [Required]
        [DisplayName("Delivery Date")]
        public string ReportDeliveryDate { get; set; } = "";
        [Required]
        [DisplayName("Delivery Time")]
        public string ReportDeliveryTime { get; set; } = "";
        public string TestName { get; set; } = "";
        [Required]
        [DisplayName("Test")]
        public int TestId { get; set; }
        [Required]
        [DisplayName("Patient")]
        public int PatientId { get; set; }
        public string PatientName { get; set; } = "";
        public string PatientNumber { get; set; } = "";

        public string Doctor { get; set; } = "";

        [Required]
        [DisplayName("Doctor")]
        public int DoctorId { get; set; }

        public int ReportNumber { get; set; }
        public string ReportNoString { get; set; } = "";

        public string Status { get; set; } = "";
        public string Gender { get; set; } = "";

        public string PatientAge { get; set; } = "";
        public string PatientGender { get; set; } = "";

        public string DoctorPMDCNo { get; set; } = "";

        public string Note { get; set; } = "";

        public int? PrescriptionId { get; set; }

        public IList<ReportValueViewModel> ReportValues { get; set; } = new List<ReportValueViewModel>();

        public IList<TestParameterViewModel> TestParameters { get; set; } = new List<TestParameterViewModel>();


        public static LabReport GenerateModel(LabReportViewModel viewmodel, int previousReportNo = 0)
        {
            var now = DateTime.Now;
            var nowDate = now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            var nowTime = now.ToString("HH:mm", CultureInfo.InvariantCulture);

            var deliveryDate = DateTime.Parse(viewmodel.ReportDeliveryDate,System.Globalization.CultureInfo.InvariantCulture).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            TimeSpan reportTime = TimeSpan.ParseExact(viewmodel.Id > 0 ? viewmodel.ReportTime : nowTime, new string[] { "hhmm", @"hh\:mm" }, CultureInfo.InvariantCulture);
            TimeSpan deliveryTime = TimeSpan.ParseExact(viewmodel.ReportDeliveryTime, new string[] { "hhmm", @"hh\:mm" }, CultureInfo.InvariantCulture);
            var model = new LabReport
            {
                PatientId = viewmodel.PatientId,
                ReportDate = DateOnly.ParseExact(nowDate, "dd/MM/yyyy", null, DateTimeStyles.None),
                ReportTime = reportTime,
                ReportDeliveryDate = DateOnly.ParseExact(deliveryDate, "dd/MM/yyyy", null, DateTimeStyles.None),
                ReportDeliveryTime = deliveryTime,
                TestId = viewmodel.TestId,
                DoctorId = viewmodel.DoctorId,
                Id = viewmodel.Id,
                ReportNumber = viewmodel.Id > 0 && previousReportNo > 0 ? viewmodel.ReportNumber : previousReportNo + 1,
                Note = viewmodel.Note
            };


            return model;
        }

        public static LabReportViewModel GenerateViewModel(LabReport model)
        {


            var viewmodel = new LabReportViewModel();
            //{
            //    PatientId = model.PatientId,
            //    ReportDate = model.ReportDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //    ReportTime = model.ReportTime.ToString("hh:mm", CultureInfo.InvariantCulture),
            //    ReportDeliveryDate = model.ReportDeliveryDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //    ReportDeliveryTime = model.ReportDeliveryTime.ToString("hh:mm", CultureInfo.InvariantCulture),
            //    TestId = model.TestId,
            //    DoctorId = model.DoctorId,
            //    Id = model.Id,
            //    TestName = model.Test.Name,
            //    PatientName = model.Patient.Name,
            //    PatientNumber = model.Patient.PatientNumber,
            //    Doctor = model.Doctor.Name,
            //    ReportNumber = model.ReportNumber,
            //};


            return viewmodel;
        }
    }
}

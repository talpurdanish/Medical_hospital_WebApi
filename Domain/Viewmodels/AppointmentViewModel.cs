using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class AppointmentViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Appointment Date is Required")]
        [DisplayName("Start Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment Start Time is Required")]
        [DisplayName("Start Time")]
        public string StartTime { get; set; } = "";


        [DisplayName("End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime? AppointmentEndDate { get; set; }


        [DisplayName("End Time")]
        public string EndTime { get; set; } = "";

        public int UserId { get; set; }
        public int PatientId { get; set; }

        public virtual User User { get; set; } = new User();
        public virtual Patient Patient { get; set; } = new Patient();

        public string PatientName { get; set; } = "";
        public string DoctorName { get; set; } = "";



        public static AppointmentViewModel GenerateViewModel(Appointment model)
        {
            var currentDate = DateTime.Now;
            AppointmentViewModel viewModel = new();
            if (model != null)
            {
                viewModel.Id = model.Appointmentid;

                viewModel.AppointmentDate = currentDate;
                viewModel.StartTime = currentDate.Hour + ":" + currentDate.Minute;
                viewModel.UserId = model.UserId;
                viewModel.PatientId = model.PatientId;
            }

            return viewModel;

        }

        public static Appointment GenerateModel(AppointmentViewModel viewmodel)
        {
            Appointment model = new();

            if (viewmodel.Id > 0)
            {
                model.Appointmentid = viewmodel.Id;
            }

            model.AppointDate = viewmodel.AppointmentDate;
            model.StartDtTime = viewmodel.StartTime;
            model.AppointEndDate = viewmodel.AppointmentEndDate;

            model.EndDtTime = viewmodel.EndTime;
            model.UserId = viewmodel.UserId;
            model.PatientId = viewmodel.PatientId;

            return model;
        }

    }
}

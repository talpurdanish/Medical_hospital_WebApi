using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Viewmodels
{
    public class ReportValueViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Maximum Value")]
        public double Value { get; set; }
        [Required]
        [DisplayName("Parameter ID")]
        public int TestParameterId { get; set; }

        [DisplayName("Parameter")]
        public TestParameterViewModel TestParameter { get; set; } = new TestParameterViewModel();
        public int LabReportId { get; set; }
            
        public static ReportValueViewModel GenerateViewModel(ReportValue model)
        {

            ReportValueViewModel viewModel = new();

            viewModel.Value = model.Value;
            viewModel.TestParameterId = model.TestParameterId;
            viewModel.LabReportId = model.LabReportId;

            return viewModel;

        }

        public static ReportValue GenerateModel(ReportValueViewModel viewmodel)
        {
            ReportValue model = new();

            if (viewmodel.Id > 0)
            {
                model.Id = viewmodel.Id;
            }

            model.Value = viewmodel.Value;
            model.TestParameterId = viewmodel.TestParameterId;
            model.LabReportId = viewmodel.LabReportId;

            return model;
        }
    }
}

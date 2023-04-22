using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class TestParameterViewModel
    {

        public int Id { get; set; }
        [Required]
        public string? Name { get; set; } ="";
        [Required]
        [DisplayName("Maximum Value (Male)")]
        public double MaleMaxValue { get; set; }
        [Required]
        [DisplayName("Minimum Value (Male)")]
        public double MaleMinValue { get; set; }
        
        [DisplayName("Maximum Value (Female)")]
        public double FemaleMaxValue { get; set; }
        
        [DisplayName("Minimum Value (Female)")]
        public double FemaleMinValue { get; set; }

        [Required]
        public string? Unit { get; set; } ="";
        public double Value { get; set; }

        public int TestId { get; set; }
        public string? TestName { get; set; } = "";
        [DisplayName("Reference Range")]
        public string? ReferenceRange { get; set; } ="";
        public bool Gender { get; set; }

        public bool Status { get; set; }

        public static TestParameterViewModel GenerateViewModel(TestParameter model)
        {
            TestParameterViewModel viewModel = new();

            viewModel.Name = model.Name;
            viewModel.MaleMaxValue = model.MaleMaxValue;
            viewModel.MaleMinValue = model.MaleMinValue;
            viewModel.FemaleMaxValue = model.FemaleMaxValue;
            viewModel.FemaleMinValue = model.FemaleMinValue;
            viewModel.Unit = model.Unit;
            viewModel.TestId = model.TestId;
            viewModel.Gender = model.Gender;
            viewModel.ReferenceRange = model.ReferenceRange;
            viewModel.TestId = model.TestId;
            viewModel.Id = model.Id;

            return viewModel;

        }

        public static TestParameterViewModel GenerateViewModel(TestParameter model, bool status, double value)
        {

            TestParameterViewModel viewModel = new();

            viewModel.Name = model.Name;
            viewModel.MaleMaxValue = model.MaleMaxValue;
            viewModel.MaleMinValue = model.MaleMinValue;
            viewModel.FemaleMaxValue = model.FemaleMaxValue;
            viewModel.FemaleMinValue = model.FemaleMinValue;
            viewModel.Unit = model.Unit;
            viewModel.TestId = model.TestId;
            viewModel.Gender = model.Gender;
            viewModel.ReferenceRange = model.ReferenceRange;
            viewModel.Status = status;
            viewModel.Value = value;
            viewModel.TestId = model.TestId;
            viewModel.Id = model.Id;
            return viewModel;

        }

        public static TestParameter GenerateModel(TestParameterViewModel viewmodel)
        {
            TestParameter model = new();

            if (viewmodel.Id > 0)
            {
                model.Id = viewmodel.Id;
            }

            model.Name = viewmodel.Name??"";
            model.MaleMaxValue = viewmodel.MaleMaxValue;
            model.MaleMinValue = viewmodel.MaleMinValue;
            model.FemaleMaxValue = viewmodel.FemaleMaxValue;
            model.FemaleMinValue = viewmodel.FemaleMinValue;
            model.Unit = viewmodel.Unit??"";
            model.TestId = viewmodel.TestId;
            model.Gender = viewmodel.Gender;
            model.ReferenceRange = viewmodel.ReferenceRange ?? "";
            return model;
        }


    }
}
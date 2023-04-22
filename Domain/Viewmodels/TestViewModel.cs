using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class TestViewModel
    {

        public int Id { get; set; }
        [Required]
        [DisplayName("Test Name")]
        [StringLength(500, ErrorMessage ="Test Name cannot be longer than 500 chars")]
        public string Name { get; set; } ="";
        [StringLength(1500, ErrorMessage = "Test Name cannot be longer than 1500 chars")]
        public string Description { get; set; } ="";
        
        [DisplayName("Parameters")]
        public IEnumerable<TestParameterViewModel> TestParameters { get; set; } = new List<TestParameterViewModel>();


        public static TestViewModel GenerateViewModel(Test model)
        {

            TestViewModel viewModel = new();

            viewModel.Name = model.Name;
            viewModel.Description = model.Description;
          
            return viewModel;

        }

        public static Test GenerateModel(TestViewModel viewmodel)
        {
            Test model = new();

            if (viewmodel.Id > 0)
            {
                model.Id = viewmodel.Id;
            }

            model.Name = viewmodel.Name;
            model.Description = viewmodel.Description;

            return model;
        }
    }
}

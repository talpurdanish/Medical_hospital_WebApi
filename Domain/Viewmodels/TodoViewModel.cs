using Domain.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class TodoViewModel
    {

        public int id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [DisplayName("Title")]
        [StringLength(250, ErrorMessage = "Title should be less than 250 chars")]
        public string Title { get; set; } = "";

        public string Created { get; set; } = "";
        public bool Completed { get; set; }
        public static TodoViewModel GenerateViewModel(TodoEvent model)
        {

            TodoViewModel viewModel = new();
            viewModel.UserId = model.UserID;
            viewModel.Title = model.Title;
            viewModel.Created = model.Created.ToString("dd MMM yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            viewModel.Completed = model.Completed;
            return viewModel;

        }
        public TodoViewModel() { }

        public static TodoEvent GenerateModel(TodoViewModel viewModel)
        {
            TodoEvent model = new()
            {
                Created = DateTime.Now,
                Completed = false,

                UserID = viewModel.UserId,
                Title = viewModel.Title
            };


            return model;
        }
    }
}

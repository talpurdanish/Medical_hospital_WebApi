using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class MedicationTypeViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Medication Type Name is Required")]
        [DisplayName("Medication Type")]
        [StringLength(500, ErrorMessage = "Medication Type Name should be less than 500 chars")]
        public string Name { get; set; } = "";



        public static MedicationTypeViewModel GenerateViewModel(MedicationType model)
        {

            MedicationTypeViewModel viewModel = new();
            viewModel.ID = model.MedicationTypeID;

            viewModel.Name = model.MedicationTypeName;

            return viewModel;

        }

        public static MedicationType GenerateModel(MedicationTypeViewModel viewmodel)
        {
            MedicationType model = new();

            if (viewmodel.ID > 0)
            {
                model.MedicationTypeID = viewmodel.ID;
            }

            model.MedicationTypeName = viewmodel.Name;
            return model;
        }

    }
}

using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class MedicationViewModel
    {

        [Key]
        public int Code { get; set; }
        [Required(ErrorMessage = "Medication Name is Required")]
        [DisplayName("Medication Name")]
        [StringLength(250, ErrorMessage = "Medication Name should be less than 250 chars")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Medication Brand is Required")]
        [MaxLength(1000, ErrorMessage = "Medication Brand should be less than 1000 chars")]
        public string Brand { get; set; } = "";
        public string Description { get; set; } = "";

        public string Type { get; set; } = string.Empty;
        public int TypeID { get; set; }



        public static MedicationViewModel GenerateViewModel(Medication model)
        {

            MedicationViewModel viewModel = new();
            viewModel.Code = model.Code;

            viewModel.Name = model.Name;
            
            viewModel.Brand = model.Brand;
            viewModel.Description = model.Description;
            viewModel.TypeID = model.MedicationTypeID;

            return viewModel;

        }

        public static Medication GenerateModel(MedicationViewModel viewmodel)
        {
            Medication model = new();

            if (viewmodel.Code > 0)
            {
                model.Code = viewmodel.Code;
            }

            model.Name = viewmodel.Name;


            
            model.Brand = viewmodel.Brand;
            model.Description = viewmodel.Description;
            model.MedicationTypeID = viewmodel.TypeID;

            return model;
        }

    }
}

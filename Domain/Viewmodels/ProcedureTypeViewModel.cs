using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class ProcedureTypeViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Procedure Type Name is Required")]
        [DisplayName("Procedure Type")]
        [StringLength(500, ErrorMessage = "Procedure Type Name should be less than 500 chars")]
        public string Name { get; set; } = "";



        public static ProcedureTypeViewModel GenerateViewModel(ProcedureType model)
        {

            ProcedureTypeViewModel viewModel = new();
            viewModel.ID = model.ProcedureTypeID;

            viewModel.Name = model.ProcedureTypeName;

            return viewModel;

        }

        public static ProcedureType GenerateModel(ProcedureTypeViewModel viewmodel)
        {
            ProcedureType model = new();

            if (viewmodel.ID > 0)
            {
                model.ProcedureTypeID = viewmodel.ID;
            }

            model.ProcedureTypeName = viewmodel.Name;


            return model;
        }

    }
}

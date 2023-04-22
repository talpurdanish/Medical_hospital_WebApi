using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class ProcedureViewModel
    {

        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "ProcedureName is Required")]
        [DisplayName("Procedure Name")]
        [StringLength(250, ErrorMessage = "ProcedureName should be less than 250 chars")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Procedure Cost is Required")]
        [DataType("double")]
        [DisplayName("Procedure Cost")]
        public double Cost { get; set; }

        public string  Type { get; set; } = "";
        public  int TypeID { get; set; }
        public  int SNo { get; set; }


        public static ProcedureViewModel GenerateViewModel(Procedure model)
        {

            ProcedureViewModel viewModel = new();
            viewModel.ID = model.ProcedureID;

            viewModel.Name = model.ProcedureName;
            viewModel.Cost = model.ProcedureCost;
            viewModel.TypeID = model.ProcedureTypeID;

            return viewModel;

        }

        public static Procedure GenerateModel(ProcedureViewModel viewmodel)
        {
            Procedure model = new();

            if (viewmodel.ID > 0)
            {
                model.ProcedureID = viewmodel.ID;
            }

            model.ProcedureName = viewmodel.Name;


            model.ProcedureName = viewmodel.Name;
            model.ProcedureCost = viewmodel.Cost;
            model.ProcedureTypeID = viewmodel.TypeID;

            return model;
        }

    }
}

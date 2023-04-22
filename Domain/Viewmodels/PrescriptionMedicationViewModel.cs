using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Viewmodels
{
    public class PrescriptionMedicationViewModel
    {
        //Dose Parameters
        public int PrescriptionId { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public string Units { get; set; } = "";
        [Required]
        public int Times { get; set; }

        [Required]
        public int MedicationCode { get; set; }
        public string Medication { get; set; } = "";


        public static PrescriptionMedicationViewModel GenerateViewModel(PrescriptionMedication model)
        {

            PrescriptionMedicationViewModel viewModel = new();
            viewModel.Quantity = model.Quantity;
            viewModel.Units = model.Units;
            viewModel.MedicationCode = model.MedicationCode;
            viewModel.Times = model.Times;

            return viewModel;

        }

        public static PrescriptionMedication GenerateModel(PrescriptionMedicationViewModel viewmodel)
        {
            PrescriptionMedication model = new();

            if (viewmodel.PrescriptionId > 0)
            {
                model.PrescriptionId = viewmodel.PrescriptionId;
            }

            model.Quantity = viewmodel.Quantity;
            model.Units = viewmodel.Units;
            model.MedicationCode = viewmodel.MedicationCode;
            model.Times = viewmodel.Times;


            return model;
        }


        public override string ToString()
        {
            var timeStr = Times == 1 ? "a day" : "times/day";
            if (Quantity == 1)
            {
                Units = Units.Substring(0, Units.Length - 1);
            }

            var str = "{0} ............. {1} {2} {3} {4}";
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,str, Medication, Quantity, Units, Times, timeStr);
        }
    }
}

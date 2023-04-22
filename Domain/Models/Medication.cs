using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Medication
    {

        [Key]
        public int Code { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Name { get; set; } = "";
        [Required]
        [MaxLength(1000)]
        public string Brand { get; set; } = "";
        public string Description { get; set; } = "";

        public int MedicationTypeID { get; set; }
        //public MedicationType MedicationType { get; set; } = new MedicationType();
    }
}

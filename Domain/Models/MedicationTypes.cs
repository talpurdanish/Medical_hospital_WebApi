using System.ComponentModel.DataAnnotations;


namespace Domain.Models
{
    public class MedicationType
    {

        [Key]
        public int MedicationTypeID { get; set; }
        [Required]
        public string MedicationTypeName { get; set; } = "";
    }
}

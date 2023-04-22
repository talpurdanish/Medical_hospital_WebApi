using System.ComponentModel.DataAnnotations;


namespace Domain.Models
{
    public class ProcedureType
    {

        [Key]
        public int ProcedureTypeID { get; set; }
        [Required]
        public string ProcedureTypeName { get; set; } = "";
    }
}

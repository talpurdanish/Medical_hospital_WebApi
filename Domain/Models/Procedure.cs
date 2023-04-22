using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Procedure
    {
        [Key]
        public int ProcedureID { get; set; }
        [Required]
        public string ProcedureName { get; set; } = "";
        public double ProcedureCost { get; set; }

        
        public int ProcedureTypeID { get; set; }
    }
}

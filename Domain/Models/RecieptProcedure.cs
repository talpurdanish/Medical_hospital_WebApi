namespace Domain.Models
{
    public class RecieptProcedure
    {
        public int Id { get; set; }

        public int RecieptId { get; set; }
        public int ProcedureId { get; set; }

        //public virtual Reciept Reciept { get; set; } =  new Reciept();
        //public virtual Procedure Procedure { get; set; } =  new Procedure();

    }
}

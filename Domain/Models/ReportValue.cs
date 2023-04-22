namespace Domain.Models
{
    public class ReportValue
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int TestParameterId { get; set; }
        //public virtual TestParameter TestParameter { get; set; } = new TestParameter();

        public int LabReportId { get; set; }
    }
}

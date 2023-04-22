using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FMDC.Context
{
    public class MedicalContextFactory : IDesignTimeDbContextFactory<MedicalContext>
    {
        public MedicalContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MedicalContext>();
            optionsBuilder.UseSqlServer("Data Source=THINKPAD;Initial Catalog=MedicalContext;Integrated Security=True;Encrypt=False");

            return new MedicalContext(optionsBuilder.Options);
        }
    }
}

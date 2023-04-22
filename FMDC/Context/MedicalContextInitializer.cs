using Microsoft.EntityFrameworkCore;

namespace FMDC.Context
{
    public static class MedicalContextInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MedicalContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MedicalContext>>());
            // Look for any movies.

        }
    }
}

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Data;


namespace FMDC.Context
{
    public class MedicalContext : DbContext
    {

        public MedicalContext(DbContextOptions options) : base(options)

        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
      
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedication> PrescriptionMedications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicationType> MedicationTypes { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureType> ProcedureTypes { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Reciept> Reciepts { get; set; }
        public DbSet<RecieptProcedure> RecieptProcedures { get; set; }
        
        public DbSet<TodoEvent> TodoEvents { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestParameter> TestParameters { get; set; }
        public DbSet<LabReport> LabReports { get; set; }
        public DbSet<ReportValue> ReportValues { get; set; }

        //public DbSet<Notification> Notifications{ get; set;}


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
                .HaveColumnType("date");

            configurationBuilder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter, NullableDateOnlyComparer>()
                .HaveColumnType("date");

           
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(etb =>
            {
                etb.HasKey(u => u.UserId);
                etb.Property(u => u.UserId).ValueGeneratedOnAdd();
                etb.Property(u => u.Name).IsRequired().HasMaxLength(64);
                etb.Property(u => u.Created)
                    .HasDefaultValueSql("getdate()");
                etb.ToTable("Users");
            });

            


            modelBuilder.Entity<Province>(etb =>
            {
                etb.HasKey(e => e.ProvinceId);
                etb.Property(e => e.ProvinceId).ValueGeneratedOnAdd();
                etb.Property(e => e.Name).IsRequired().HasMaxLength(256);
                etb.ToTable("Provinces");
            });

            modelBuilder.Entity<City>(etb =>
            {
                etb.HasKey(e => e.CityId);
                etb.Property(e => e.CityId).ValueGeneratedOnAdd();
                etb.Property(e => e.Name).IsRequired().HasMaxLength(256);
                etb.ToTable("Cities");
            });

            modelBuilder.Entity<Medication>(etb =>
            {
                etb.HasKey(e => e.Code);
                etb.Property(e => e.Code).ValueGeneratedOnAdd();
                etb.Property(e => e.Name).IsRequired().HasMaxLength(256);
                etb.Property(e => e.Brand).HasMaxLength(256);
                etb.Property(e => e.Description).HasMaxLength(2048);
                etb.ToTable("Medications");
            });

            modelBuilder.Entity<Prescription>(etb =>
            {
                etb.HasKey(e => e.PrescriptionId);
                etb.Property(e => e.PrescriptionId).ValueGeneratedOnAdd();

                etb.ToTable("Prescriptions");
            });

            modelBuilder.Entity<Appointment>(etb =>
            {
                etb.HasKey(e => e.Appointmentid);
                etb.Property(e => e.Appointmentid).ValueGeneratedOnAdd();

                etb.ToTable("Appointments");
            });

            modelBuilder.Entity<Patient>(etb =>
            {
                etb.HasKey(e => e.PatientId);
                etb.Property(e => e.Name).IsRequired().HasMaxLength(250);
                etb.Property(b => b.Created)
                    .HasDefaultValueSql("getdate()");
                etb.ToTable("Patients");
            });

            modelBuilder.Entity<Reciept>(etb =>
            {
                etb.HasKey(e => e.RecieptId);
                etb.Property(e => e.RecieptId).ValueGeneratedOnAdd();
                etb.Property(b => b.RecieptDate)
                    .HasDefaultValueSql("getdate()");
                etb.ToTable("Reciepts");
            });

            modelBuilder.Entity<RecieptProcedure>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.ToTable("RecieptProcedures");
            });
           
            modelBuilder.Entity<TodoEvent>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.Property(e => e.Id).ValueGeneratedOnAdd();

                etb.ToTable("TodoEvents");
            });

          


        }
      
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }

    /// <summary>
    /// Compares <see cref="DateOnly" />.
    /// </summary>
    public class DateOnlyComparer : ValueComparer<DateOnly>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public DateOnlyComparer() : base(
            (d1, d2) => d1 == d2 && d1.DayNumber == d2.DayNumber,
            d => d.GetHashCode())
        {
        }
    }

    /// <summary>
    /// Converts <see cref="DateOnly?" /> to <see cref="DateTime?"/> and vice versa.
    /// </summary>
    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public NullableDateOnlyConverter() : base(
            d => d == null
                ? null
                : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
            d => d == null
                ? null
                : new DateOnly?(DateOnly.FromDateTime(d.Value)))
        { }
    }

    /// <summary>
    /// Compares <see cref="DateOnly?" />.
    /// </summary>
    public class NullableDateOnlyComparer : ValueComparer<DateOnly?>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public NullableDateOnlyComparer() : base(
            (d1, d2) => d1 == d2 && d1.GetValueOrDefault().DayNumber == d2.GetValueOrDefault().DayNumber,
            d => d.GetHashCode())
        {
        }
    }

}

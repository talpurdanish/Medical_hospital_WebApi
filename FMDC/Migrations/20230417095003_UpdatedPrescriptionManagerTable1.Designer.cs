﻿// <auto-generated />
using System;
using FMDC.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FMDC.Migrations
{
    [DbContext(typeof(MedicalContext))]
    [Migration("20230417095003_UpdatedPrescriptionManagerTable1")]
    partial class UpdatedPrescriptionManagerTable1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Appointment", b =>
                {
                    b.Property<int>("Appointmentid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Appointmentid"));

                    b.Property<DateTime>("AppointDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("AppointEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EndDtTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int?>("RecieptId")
                        .HasColumnType("int");

                    b.Property<string>("StartDtTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Appointmentid");

                    b.ToTable("Appointments", (string)null);
                });

            modelBuilder.Entity("Domain.Models.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.HasKey("CityId");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Cities", (string)null);
                });

            modelBuilder.Entity("Domain.Models.LabReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int?>("PrescriptionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ReportDeliveryDate")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("ReportDeliveryTime")
                        .HasColumnType("time");

                    b.Property<int>("ReportNumber")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ReportTime")
                        .HasColumnType("time");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LabReports");
                });

            modelBuilder.Entity("Domain.Models.Medication", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("MedicationTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Code");

                    b.ToTable("Medications", (string)null);
                });

            modelBuilder.Entity("Domain.Models.MedicationType", b =>
                {
                    b.Property<int>("MedicationTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedicationTypeID"));

                    b.Property<string>("MedicationTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MedicationTypeID");

                    b.ToTable("MedicationTypes");
                });

            modelBuilder.Entity("Domain.Models.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PatientId"));

                    b.Property<string>("Address")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("BloodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CNIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("DateofBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MRNoID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("PatientNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PhoneType")
                        .HasColumnType("int");

                    b.Property<byte[]>("Picture")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("PatientId");

                    b.HasIndex("CNIC")
                        .IsUnique();

                    b.ToTable("Patients", (string)null);
                });

            modelBuilder.Entity("Domain.Models.Prescription", b =>
                {
                    b.Property<int>("PrescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrescriptionId"));

                    b.Property<int>("Appointmentid")
                        .HasColumnType("int");

                    b.Property<string>("Bp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Bsr")
                        .HasColumnType("float");

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Ht")
                        .HasColumnType("float");

                    b.Property<double>("Pulse")
                        .HasColumnType("float");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Temp")
                        .HasColumnType("float");

                    b.Property<double>("Wt")
                        .HasColumnType("float");

                    b.HasKey("PrescriptionId");

                    b.ToTable("Prescriptions", (string)null);
                });

            modelBuilder.Entity("Domain.Models.PrescriptionMedication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicationCode")
                        .HasColumnType("int");

                    b.Property<int>("PrescriptionId")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<int>("Times")
                        .HasColumnType("int");

                    b.Property<string>("Units")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PrescriptionMedications");
                });

            modelBuilder.Entity("Domain.Models.Procedure", b =>
                {
                    b.Property<int>("ProcedureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProcedureID"));

                    b.Property<double>("ProcedureCost")
                        .HasColumnType("float");

                    b.Property<string>("ProcedureName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcedureTypeID")
                        .HasColumnType("int");

                    b.HasKey("ProcedureID");

                    b.ToTable("Procedures");
                });

            modelBuilder.Entity("Domain.Models.ProcedureType", b =>
                {
                    b.Property<int>("ProcedureTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProcedureTypeID"));

                    b.Property<string>("ProcedureTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProcedureTypeID");

                    b.ToTable("ProcedureTypes");
                });

            modelBuilder.Entity("Domain.Models.Province", b =>
                {
                    b.Property<int>("ProvinceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProvinceId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("ProvinceId");

                    b.ToTable("Provinces", (string)null);
                });

            modelBuilder.Entity("Domain.Models.Reciept", b =>
                {
                    b.Property<int>("RecieptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecieptId"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorizedById")
                        .HasColumnType("int");

                    b.Property<double>("GrandTotal")
                        .HasColumnType("float");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RecieptDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<double>("RecieptDiscount")
                        .HasColumnType("float");

                    b.Property<string>("RecieptTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RecieptId");

                    b.ToTable("Reciepts", (string)null);
                });

            modelBuilder.Entity("Domain.Models.RecieptProcedure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProcedureId")
                        .HasColumnType("int");

                    b.Property<int>("RecieptId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RecieptProcedures", (string)null);
                });

            modelBuilder.Entity("Domain.Models.ReportValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LabReportId")
                        .HasColumnType("int");

                    b.Property<int>("TestParameterId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ReportValues");
                });

            modelBuilder.Entity("Domain.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Domain.Models.TestParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("FemaleMaxValue")
                        .HasColumnType("float");

                    b.Property<double>("FemaleMinValue")
                        .HasColumnType("float");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<double>("MaleMaxValue")
                        .HasColumnType("float");

                    b.Property<double>("MaleMinValue")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceRange")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("TestParameters");
                });

            modelBuilder.Entity("Domain.Models.TodoEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TodoEvents", (string)null);
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CNIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("DateofBirth")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("PMDCNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PhoneType")
                        .HasColumnType("int");

                    b.Property<byte[]>("Picture")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.Models.City", b =>
                {
                    b.HasOne("Domain.Models.Province", "Province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Domain.Models.TestParameter", b =>
                {
                    b.HasOne("Domain.Models.Test", null)
                        .WithMany("TestParameters")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.Province", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("Domain.Models.Test", b =>
                {
                    b.Navigation("TestParameters");
                });
#pragma warning restore 612, 618
        }
    }
}

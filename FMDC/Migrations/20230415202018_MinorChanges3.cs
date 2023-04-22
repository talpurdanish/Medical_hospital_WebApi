using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMDC.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabReports_Patients_PatientId",
                table: "LabReports");

            migrationBuilder.DropForeignKey(
                name: "FK_LabReports_Tests_TestId",
                table: "LabReports");

            migrationBuilder.DropForeignKey(
                name: "FK_LabReports_Users_DoctorId",
                table: "LabReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Appointments_Appointmentid",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportValues_LabReports_LabReportId",
                table: "ReportValues");

            migrationBuilder.DropIndex(
                name: "IX_ReportValues_LabReportId",
                table: "ReportValues");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_Appointmentid",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_LabReports_DoctorId",
                table: "LabReports");

            migrationBuilder.DropIndex(
                name: "IX_LabReports_PatientId",
                table: "LabReports");

            migrationBuilder.DropIndex(
                name: "IX_LabReports_TestId",
                table: "LabReports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReportValues_LabReportId",
                table: "ReportValues",
                column: "LabReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_Appointmentid",
                table: "Prescriptions",
                column: "Appointmentid");

            migrationBuilder.CreateIndex(
                name: "IX_LabReports_DoctorId",
                table: "LabReports",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabReports_PatientId",
                table: "LabReports",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LabReports_TestId",
                table: "LabReports",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabReports_Patients_PatientId",
                table: "LabReports",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabReports_Tests_TestId",
                table: "LabReports",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabReports_Users_DoctorId",
                table: "LabReports",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Appointments_Appointmentid",
                table: "Prescriptions",
                column: "Appointmentid",
                principalTable: "Appointments",
                principalColumn: "Appointmentid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportValues_LabReports_LabReportId",
                table: "ReportValues",
                column: "LabReportId",
                principalTable: "LabReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

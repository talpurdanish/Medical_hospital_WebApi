using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMDC.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPrescriptionManagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationCode",
                table: "PrescriptionMedications");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionMedications_MedicationCode",
                table: "PrescriptionMedications");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionMedications_PrescriptionId",
                table: "PrescriptionMedications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_MedicationCode",
                table: "PrescriptionMedications",
                column: "MedicationCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedications_PrescriptionId",
                table: "PrescriptionMedications",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_Medications_MedicationCode",
                table: "PrescriptionMedications",
                column: "MedicationCode",
                principalTable: "Medications",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedications_Prescriptions_PrescriptionId",
                table: "PrescriptionMedications",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMDC.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_MedicationTypes_MedicationTypeID",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_RecieptProcedures_Procedures_ProcedureId",
                table: "RecieptProcedures");

            migrationBuilder.DropForeignKey(
                name: "FK_RecieptProcedures_Reciepts_RecieptId",
                table: "RecieptProcedures");

            migrationBuilder.DropForeignKey(
                name: "FK_Reciepts_Users_UserId",
                table: "Reciepts");

            migrationBuilder.DropIndex(
                name: "IX_Reciepts_UserId",
                table: "Reciepts");

            migrationBuilder.DropIndex(
                name: "IX_RecieptProcedures_ProcedureId",
                table: "RecieptProcedures");

            migrationBuilder.DropIndex(
                name: "IX_RecieptProcedures_RecieptId",
                table: "RecieptProcedures");

            migrationBuilder.DropIndex(
                name: "IX_Medications_MedicationTypeID",
                table: "Medications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reciepts_UserId",
                table: "Reciepts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecieptProcedures_ProcedureId",
                table: "RecieptProcedures",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_RecieptProcedures_RecieptId",
                table: "RecieptProcedures",
                column: "RecieptId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_MedicationTypeID",
                table: "Medications",
                column: "MedicationTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_MedicationTypes_MedicationTypeID",
                table: "Medications",
                column: "MedicationTypeID",
                principalTable: "MedicationTypes",
                principalColumn: "MedicationTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecieptProcedures_Procedures_ProcedureId",
                table: "RecieptProcedures",
                column: "ProcedureId",
                principalTable: "Procedures",
                principalColumn: "ProcedureID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecieptProcedures_Reciepts_RecieptId",
                table: "RecieptProcedures",
                column: "RecieptId",
                principalTable: "Reciepts",
                principalColumn: "RecieptId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reciepts_Users_UserId",
                table: "Reciepts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMDC.Migrations
{
    /// <inheritdoc />
    public partial class MinorChanges1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_ProcedureTypes_ProcedureTypeID",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_ProcedureTypeID",
                table: "Procedures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Procedures_ProcedureTypeID",
                table: "Procedures",
                column: "ProcedureTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_ProcedureTypes_ProcedureTypeID",
                table: "Procedures",
                column: "ProcedureTypeID",
                principalTable: "ProcedureTypes",
                principalColumn: "ProcedureTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

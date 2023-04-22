using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMDC.Migrations
{
    /// <inheritdoc />
    public partial class AddedVitalsInPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Bp",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Bsr",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ht",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Pulse",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Temp",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Wt",
                table: "Prescriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bp",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Bsr",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Ht",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Pulse",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Temp",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Wt",
                table: "Prescriptions");
        }
    }
}

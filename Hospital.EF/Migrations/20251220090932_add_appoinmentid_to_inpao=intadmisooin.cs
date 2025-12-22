using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.EF.Migrations
{
    /// <inheritdoc />
    public partial class add_appoinmentid_to_inpaointadmisooin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Inpatient_Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inpatient_Admissions_AppointmentId",
                table: "Inpatient_Admissions",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inpatient_Admissions_Appointments_AppointmentId",
                table: "Inpatient_Admissions",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inpatient_Admissions_Appointments_AppointmentId",
                table: "Inpatient_Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Inpatient_Admissions_AppointmentId",
                table: "Inpatient_Admissions");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Inpatient_Admissions");
        }
    }
}

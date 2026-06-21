using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class MigracionAjuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Paciente");

            migrationBuilder.AddColumn<string>(
                name: "MedicoId",
                table: "Paciente",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_MedicoId",
                table: "Paciente",
                column: "MedicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paciente_AspNetUsers_MedicoId",
                table: "Paciente",
                column: "MedicoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paciente_AspNetUsers_MedicoId",
                table: "Paciente");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_MedicoId",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "MedicoId",
                table: "Paciente");

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Paciente",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Paciente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}

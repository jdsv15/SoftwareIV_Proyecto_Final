using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class RelacionPacienteUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Paciente",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_UsuarioId",
                table: "Paciente",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paciente_AspNetUsers_UsuarioId",
                table: "Paciente",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paciente_AspNetUsers_UsuarioId",
                table: "Paciente");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_UsuarioId",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Paciente");
        }
    }
}

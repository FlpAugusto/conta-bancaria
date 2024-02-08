using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class thirdmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Transacoes");

            migrationBuilder.AddColumn<int>(
                name: "ContaDestino_Id",
                table: "Transacoes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoOperacao",
                table: "Transacoes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContaDestino_Id",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "TipoOperacao",
                table: "Transacoes");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Transacoes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

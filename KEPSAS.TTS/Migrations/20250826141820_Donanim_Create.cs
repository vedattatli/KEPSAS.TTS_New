using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KEPSAS.TTS.Migrations
{
    /// <inheritdoc />
    public partial class Donanim_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtananKullanici",
                table: "Donanimlar");

            migrationBuilder.AlterColumn<string>(
                name: "AtananKullaniciId",
                table: "Donanimlar",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donanimlar_AtananKullaniciId",
                table: "Donanimlar",
                column: "AtananKullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donanimlar_AspNetUsers_AtananKullaniciId",
                table: "Donanimlar",
                column: "AtananKullaniciId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donanimlar_AspNetUsers_AtananKullaniciId",
                table: "Donanimlar");

            migrationBuilder.DropIndex(
                name: "IX_Donanimlar_AtananKullaniciId",
                table: "Donanimlar");

            migrationBuilder.AlterColumn<string>(
                name: "AtananKullaniciId",
                table: "Donanimlar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AtananKullanici",
                table: "Donanimlar",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

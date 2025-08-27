using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KEPSAS.TTS.Migrations
{
    /// <inheritdoc />
    public partial class Talep_Enhanced_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OlusturanKullaniciId",
                table: "Talepler",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonanimId",
                table: "Talepler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HedefKullaniciId",
                table: "Talepler",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HedefSicilNo",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAdresi",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IptalNedeni",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tip",
                table: "Talepler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "YazilimAdi",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_DonanimId",
                table: "Talepler",
                column: "DonanimId");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_HedefKullaniciId",
                table: "Talepler",
                column: "HedefKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_OlusturanKullaniciId",
                table: "Talepler",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_Tip",
                table: "Talepler",
                column: "Tip");

            migrationBuilder.AddForeignKey(
                name: "FK_Talepler_AspNetUsers_AtananKullaniciId",
                table: "Talepler",
                column: "AtananKullaniciId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Talepler_AspNetUsers_HedefKullaniciId",
                table: "Talepler",
                column: "HedefKullaniciId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Talepler_AspNetUsers_OlusturanKullaniciId",
                table: "Talepler",
                column: "OlusturanKullaniciId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Talepler_Donanimlar_DonanimId",
                table: "Talepler",
                column: "DonanimId",
                principalTable: "Donanimlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talepler_AspNetUsers_AtananKullaniciId",
                table: "Talepler");

            migrationBuilder.DropForeignKey(
                name: "FK_Talepler_AspNetUsers_HedefKullaniciId",
                table: "Talepler");

            migrationBuilder.DropForeignKey(
                name: "FK_Talepler_AspNetUsers_OlusturanKullaniciId",
                table: "Talepler");

            migrationBuilder.DropForeignKey(
                name: "FK_Talepler_Donanimlar_DonanimId",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_DonanimId",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_HedefKullaniciId",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_OlusturanKullaniciId",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_Tip",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "DonanimId",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "HedefKullaniciId",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "HedefSicilNo",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "IpAdresi",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "IptalNedeni",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "YazilimAdi",
                table: "Talepler");

            migrationBuilder.AlterColumn<string>(
                name: "OlusturanKullaniciId",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

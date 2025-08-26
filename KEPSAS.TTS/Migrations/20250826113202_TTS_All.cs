using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KEPSAS.TTS.Migrations
{
    /// <inheritdoc />
    public partial class TTS_All : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OlusturanKullaniciId",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Durum",
                table: "Talepler",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AtananKullaniciId",
                table: "Talepler",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TalepLoglar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalepId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KullaniciId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eski = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yeni = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Not = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalepLoglar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TalepLoglar_Talepler_TalepId",
                        column: x => x.TalepId,
                        principalTable: "Talepler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_AtananKullaniciId",
                table: "Talepler",
                column: "AtananKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_Durum",
                table: "Talepler",
                column: "Durum");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_OlusturmaTarihi",
                table: "Talepler",
                column: "OlusturmaTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_TalepLoglar_TalepId",
                table: "TalepLoglar",
                column: "TalepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalepLoglar");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_AtananKullaniciId",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_Durum",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_OlusturmaTarihi",
                table: "Talepler");

            migrationBuilder.AlterColumn<string>(
                name: "OlusturanKullaniciId",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Durum",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AtananKullaniciId",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}

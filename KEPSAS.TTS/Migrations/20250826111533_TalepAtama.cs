using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KEPSAS.TTS.Migrations
{
    /// <inheritdoc />
    public partial class TalepAtama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AtamaTarihi",
                table: "Talepler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AtananKullaniciId",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SonIslemTarihi",
                table: "Talepler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TalepEkler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalepId = table.Column<int>(type: "int", nullable: false),
                    DosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Boyut = table.Column<long>(type: "bigint", nullable: false),
                    YuklemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalepEkler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TalepEkler_Talepler_TalepId",
                        column: x => x.TalepId,
                        principalTable: "Talepler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalepEkler_TalepId",
                table: "TalepEkler",
                column: "TalepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalepEkler");

            migrationBuilder.DropColumn(
                name: "AtamaTarihi",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "AtananKullaniciId",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "SonIslemTarihi",
                table: "Talepler");
        }
    }
}

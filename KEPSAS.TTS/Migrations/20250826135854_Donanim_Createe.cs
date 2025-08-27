using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KEPSAS.TTS.Migrations
{
    /// <inheritdoc />
    public partial class Donanim_Createe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donanimlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AtananKullaniciId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AtananKullanici = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlinmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donanimlar", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donanimlar");
        }
    }
}

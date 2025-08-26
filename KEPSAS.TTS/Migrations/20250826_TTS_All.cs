using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

public partial class TTS_All : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Talepler yeni alanlar
        migrationBuilder.AddColumn<string>(
            name: "AtananKullaniciId",
            table: "Talepler",
            type: "nvarchar(450)",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "AtamaTarihi",
            table: "Talepler",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "SonIslemTarihi",
            table: "Talepler",
            type: "datetime2",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Talepler_Durum",
            table: "Talepler",
            column: "Durum");

        migrationBuilder.CreateIndex(
            name: "IX_Talepler_OlusturmaTarihi",
            table: "Talepler",
            column: "OlusturmaTarihi");

        migrationBuilder.CreateIndex(
            name: "IX_Talepler_AtananKullaniciId",
            table: "Talepler",
            column: "AtananKullaniciId");

        migrationBuilder.AddForeignKey(
            name: "FK_Talepler_AspNetUsers_AtananKullaniciId",
            table: "Talepler",
            column: "AtananKullaniciId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        // TalepLoglar
        migrationBuilder.CreateTable(
            name: "TalepLoglar",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                          .Annotation("SqlServer:Identity", "1, 1"),
                TalepId = table.Column<int>(nullable: false),
                Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                KullaniciId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                Tip = table.Column<string>(type: "nvarchar(64)", nullable: false),
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
            name: "IX_TalepLoglar_TalepId",
            table: "TalepLoglar",
            column: "TalepId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "TalepLoglar");

        migrationBuilder.DropForeignKey(
            name: "FK_Talepler_AspNetUsers_AtananKullaniciId",
            table: "Talepler");

        migrationBuilder.DropIndex(name: "IX_Talepler_Durum", table: "Talepler");
        migrationBuilder.DropIndex(name: "IX_Talepler_OlusturmaTarihi", table: "Talepler");
        migrationBuilder.DropIndex(name: "IX_Talepler_AtananKullaniciId", table: "Talepler");

        migrationBuilder.DropColumn(name: "AtananKullaniciId", table: "Talepler");
        migrationBuilder.DropColumn(name: "AtamaTarihi", table: "Talepler");
        migrationBuilder.DropColumn(name: "SonIslemTarihi", table: "Talepler");
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KEPSAS.TTS.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstAndLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdSoyad",
                table: "AspNetUsers",
                newName: "Soyad");

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ad",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "AspNetUsers",
                newName: "AdSoyad");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEM.Backend.Assessment.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalId",
                table: "Wells",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalId",
                table: "Platforms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Platforms");
        }
    }
}

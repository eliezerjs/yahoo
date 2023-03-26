using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebYahoo.Migrations
{
    /// <inheritdoc />
    public partial class AddPercentualVariation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PercentualVariation",
                table: "PriceVariations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentualVariation",
                table: "PriceVariations");
        }
    }
}

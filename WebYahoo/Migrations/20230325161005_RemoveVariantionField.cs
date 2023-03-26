using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebYahoo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVariantionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatedVariation",
                table: "PriceVariations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AccumulatedVariation",
                table: "PriceVariations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Dealership_System.Migrations
{
    /// <inheritdoc />
    public partial class AddSoldToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Cars");
        }
    }
}

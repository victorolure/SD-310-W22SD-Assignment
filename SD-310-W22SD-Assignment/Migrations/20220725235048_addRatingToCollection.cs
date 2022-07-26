using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_310_W22SD_Assignment.Migrations
{
    public partial class addRatingToCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Collection",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Collection");
        }
    }
}

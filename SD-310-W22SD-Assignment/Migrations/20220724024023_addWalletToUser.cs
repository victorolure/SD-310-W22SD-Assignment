using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_310_W22SD_Assignment.Migrations
{
    public partial class addWalletToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Wallet",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wallet",
                table: "User");
        }
    }
}

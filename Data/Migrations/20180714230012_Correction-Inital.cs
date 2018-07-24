using Microsoft.EntityFrameworkCore.Migrations;

namespace TestMakerFree.Data.Migrations
{
    public partial class CorrectionInital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Answers",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Answers",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}

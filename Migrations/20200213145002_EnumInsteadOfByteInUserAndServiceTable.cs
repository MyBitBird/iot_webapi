using Microsoft.EntityFrameworkCore.Migrations;

namespace IOT.Migrations
{
    public partial class EnumInsteadOfByteInUserAndServiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Users",
                nullable: false,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Type",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}

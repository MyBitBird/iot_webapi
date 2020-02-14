using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IOT.Migrations
{
    public partial class SetHashPasswordForInitingSampleUserInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2c256b01-355b-4c66-9b26-b12ff6d8d1c2"),
                columns: new[] { "Password", "RegisterDate" },
                values: new object[] { "PzSfdcHDcbCmM09hHXZK2bgZAFJt4aM1QahUxUROogA=", new DateTime(2020, 2, 14, 15, 50, 29, 956, DateTimeKind.Local).AddTicks(1625) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2c256b01-355b-4c66-9b26-b12ff6d8d1c2"),
                columns: new[] { "Password", "RegisterDate" },
                values: new object[] { "BitBird", new DateTime(2020, 2, 13, 19, 4, 39, 996, DateTimeKind.Local).AddTicks(5234) });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IOT.Migrations
{
    public partial class AddAnAdminUserToUserTableForStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Family", "Name", "ParentUserId", "Password", "RegisterDate", "Status", "Type", "Username" },
                values: new object[] { new Guid("2c256b01-355b-4c66-9b26-b12ff6d8d1c2"), "Malekzadeh", "Meisam", null, "BitBird", new DateTime(2020, 2, 13, 19, 4, 39, 996, DateTimeKind.Local).AddTicks(5234), (short)1, 1, "BitBird" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2c256b01-355b-4c66-9b26-b12ff6d8d1c2"));
        }
    }
}

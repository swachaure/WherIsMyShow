using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereIsMyShow.Migrations
{
    public partial class BookingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTickets",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "58d58f8b-2e37-4238-b9c6-4218671c3d79",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ec06eec6-ad9f-4a51-b5d8-15a960ce471d", "AQAAAAEAACcQAAAAEEIADG4Qhd+WvexVOg+XFko2Cw/XpD2PImmHRB56wvIqUwMnXRRkhU7abM6/WQxg9w==", "807dce6c-ac53-4f95-bc2a-0d8aa64eb507" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfTickets",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "58d58f8b-2e37-4238-b9c6-4218671c3d79",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "839d7802-14ec-4363-b57f-75fc7c81b532", "AQAAAAEAACcQAAAAEO/yHcgTPIph+efOC0KmgE8KbYE9TGIe3bfvVBGOKdNMWjdZqo26PsmytepgrxJsVA==", "56522e5b-5dce-4abd-8903-e00d1175ed93" });
        }
    }
}

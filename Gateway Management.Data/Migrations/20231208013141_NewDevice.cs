using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway_Management.Data.Migrations
{
    public partial class NewDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PeripheralDevices",
                newName: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "PeripheralDevices",
                newName: "Id");
        }
    }
}

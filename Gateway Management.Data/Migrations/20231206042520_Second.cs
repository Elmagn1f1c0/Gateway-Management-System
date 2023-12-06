using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway_Management.Data.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeripheralDevices_Gateways_GatewayId",
                table: "PeripheralDevices");

            migrationBuilder.AlterColumn<int>(
                name: "GatewayId",
                table: "PeripheralDevices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PeripheralDevices_Gateways_GatewayId",
                table: "PeripheralDevices",
                column: "GatewayId",
                principalTable: "Gateways",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeripheralDevices_Gateways_GatewayId",
                table: "PeripheralDevices");

            migrationBuilder.AlterColumn<int>(
                name: "GatewayId",
                table: "PeripheralDevices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PeripheralDevices_Gateways_GatewayId",
                table: "PeripheralDevices",
                column: "GatewayId",
                principalTable: "Gateways",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

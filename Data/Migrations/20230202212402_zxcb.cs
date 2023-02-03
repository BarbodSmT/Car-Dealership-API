using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class zxcb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterial_Service_ServiceId",
                table: "RawMaterial");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "RawMaterial",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterial_Service_ServiceId",
                table: "RawMaterial",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawMaterial_Service_ServiceId",
                table: "RawMaterial");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "RawMaterial",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RawMaterial_Service_ServiceId",
                table: "RawMaterial",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");
        }
    }
}

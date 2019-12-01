using Microsoft.EntityFrameworkCore.Migrations;

namespace Timesheets.Data.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentHeadId",
                table: "Departments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ManHourCost",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "ManagerId",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_DepartmentId",
                table: "ApplicationUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ManagerId",
                table: "ApplicationUsers",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Departments_DepartmentId",
                table: "ApplicationUsers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ApplicationUsers_ManagerId",
                table: "ApplicationUsers",
                column: "ManagerId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Departments_DepartmentId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ApplicationUsers_ManagerId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_DepartmentId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_ManagerId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentHeadId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ManHourCost",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "ApplicationUsers");
        }
    }
}

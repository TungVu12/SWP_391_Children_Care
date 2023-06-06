using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_MVC_Ver1.Migrations
{
    public partial class cate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "u_id",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "child_id",
                table: "Examination",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "doctor_id",
                table: "Examination",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "Examination",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "reservation_id",
                table: "Examination",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "u_id",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "child_id",
                table: "Examination");

            migrationBuilder.DropColumn(
                name: "doctor_id",
                table: "Examination");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "Examination");

            migrationBuilder.DropColumn(
                name: "reservation_id",
                table: "Examination");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_MVC_Ver1.Migrations
{
    public partial class updateChildren : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstNmae",
                table: "Childrens",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Childrens",
                newName: "FirstNmae");
        }
    }
}

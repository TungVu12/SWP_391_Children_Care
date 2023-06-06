using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_MVC_Ver1.Migrations
{
    public partial class cate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "Childrens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "Childrens");
        }
    }
}

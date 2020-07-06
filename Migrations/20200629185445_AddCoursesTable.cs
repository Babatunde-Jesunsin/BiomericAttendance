using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prunedge_User_Administration.Migrations
{
    public partial class AddCoursesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseTitle = table.Column<string>(nullable: true),
                    CourseCode = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    StartingTime = table.Column<DateTime>(nullable: false),
                    EndingTime = table.Column<DateTime>(nullable: false),
                    lecturerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}

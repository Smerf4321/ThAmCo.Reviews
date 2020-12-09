using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Reviews.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    reviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(nullable: false),
                    productId = table.Column<int>(nullable: false),
                    userName = table.Column<string>(nullable: true),
                    reviewContent = table.Column<string>(nullable: true),
                    reviewRating = table.Column<int>(nullable: false),
                    hidden = table.Column<bool>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    dateCreated = table.Column<DateTime>(nullable: false),
                    lastUpdated = table.Column<DateTime>(nullable: false),
                    lastUpdatedStaffEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.reviewId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Review");
        }
    }
}

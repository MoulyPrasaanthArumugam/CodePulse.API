using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class Foo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogSpotCategory",
                columns: table => new
                {
                    blogSpotsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    categoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogSpotCategory", x => new { x.blogSpotsId, x.categoriesId });
                    table.ForeignKey(
                        name: "FK_BlogSpotCategory_BlogSpot_blogSpotsId",
                        column: x => x.blogSpotsId,
                        principalTable: "BlogSpot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogSpotCategory_Category_categoriesId",
                        column: x => x.categoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogSpotCategory_categoriesId",
                table: "BlogSpotCategory",
                column: "categoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogSpotCategory");
        }
    }
}

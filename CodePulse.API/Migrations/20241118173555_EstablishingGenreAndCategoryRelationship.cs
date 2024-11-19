using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class EstablishingGenreAndCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogSpotCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogSpotId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogSpotGenre",
                columns: table => new
                {
                    BlogsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogSpotGenre", x => new { x.BlogsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BlogSpotGenre_BlogSpot_BlogsId",
                        column: x => x.BlogsId,
                        principalTable: "BlogSpot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogSpotGenre_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_BlogSpotId",
                table: "Category",
                column: "BlogSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogSpotGenre_GenresId",
                table: "BlogSpotGenre",
                column: "GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_BlogSpot_BlogSpotId",
                table: "Category",
                column: "BlogSpotId",
                principalTable: "BlogSpot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_BlogSpot_BlogSpotId",
                table: "Category");

            migrationBuilder.DropTable(
                name: "BlogSpotGenre");

            migrationBuilder.DropIndex(
                name: "IX_Category_BlogSpotId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BlogSpotId",
                table: "Category");

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
    }
}

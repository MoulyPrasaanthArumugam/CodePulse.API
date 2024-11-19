using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangingCategoryandBlogSpotRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_BlogSpot_BlogSpotId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_BlogSpotId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BlogSpotId",
                table: "Category");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "BlogSpot",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogSpot_CategoryId",
                table: "BlogSpot",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSpot_Category_CategoryId",
                table: "BlogSpot",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogSpot_Category_CategoryId",
                table: "BlogSpot");

            migrationBuilder.DropIndex(
                name: "IX_BlogSpot_CategoryId",
                table: "BlogSpot");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BlogSpot");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogSpotId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_BlogSpotId",
                table: "Category",
                column: "BlogSpotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_BlogSpot_BlogSpotId",
                table: "Category",
                column: "BlogSpotId",
                principalTable: "BlogSpot",
                principalColumn: "Id");
        }
    }
}

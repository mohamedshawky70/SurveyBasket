using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class PollInheritBaseClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
     
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Poll",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedIn",
                table: "Poll",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Poll",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedIn",
                table: "Poll",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Poll_CreatedById",
                table: "Poll",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Poll_UpdatedById",
                table: "Poll",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Poll_AspNetUsers_CreatedById",
                table: "Poll",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Poll_AspNetUsers_UpdatedById",
                table: "Poll",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poll_AspNetUsers_CreatedById",
                table: "Poll");

            migrationBuilder.DropForeignKey(
                name: "FK_Poll_AspNetUsers_UpdatedById",
                table: "Poll");

            migrationBuilder.DropIndex(
                name: "IX_Poll_CreatedById",
                table: "Poll");

            migrationBuilder.DropIndex(
                name: "IX_Poll_UpdatedById",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "CreatedIn",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "UpdatedIn",
                table: "Poll");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Taken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePoll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poll_AspNetUsers_CreatedById",
                table: "Poll");

            migrationBuilder.DropForeignKey(
                name: "FK_Poll_AspNetUsers_UpdatedById",
                table: "Poll");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Poll",
                table: "Poll");

            migrationBuilder.RenameTable(
                name: "Poll",
                newName: "polls");

            migrationBuilder.RenameIndex(
                name: "IX_Poll_UpdatedById",
                table: "polls",
                newName: "IX_polls_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Poll_Title",
                table: "polls",
                newName: "IX_polls_Title");

            migrationBuilder.RenameIndex(
                name: "IX_Poll_CreatedById",
                table: "polls",
                newName: "IX_polls_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_polls",
                table: "polls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_polls_AspNetUsers_CreatedById",
                table: "polls",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_polls_AspNetUsers_UpdatedById",
                table: "polls",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_polls_AspNetUsers_CreatedById",
                table: "polls");

            migrationBuilder.DropForeignKey(
                name: "FK_polls_AspNetUsers_UpdatedById",
                table: "polls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_polls",
                table: "polls");

            migrationBuilder.RenameTable(
                name: "polls",
                newName: "Poll");

            migrationBuilder.RenameIndex(
                name: "IX_polls_UpdatedById",
                table: "Poll",
                newName: "IX_Poll_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_polls_Title",
                table: "Poll",
                newName: "IX_Poll_Title");

            migrationBuilder.RenameIndex(
                name: "IX_polls_CreatedById",
                table: "Poll",
                newName: "IX_Poll_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Poll",
                table: "Poll",
                column: "Id");

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
    }
}

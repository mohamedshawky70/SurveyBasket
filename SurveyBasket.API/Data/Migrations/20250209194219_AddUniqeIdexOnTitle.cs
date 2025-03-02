using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddUniqeIdexOnTitle : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Poll_Title",
				table: "Poll");

			migrationBuilder.CreateIndex(
				name: "IX_Poll_Title",
				table: "Poll",
				column: "Title",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Poll_Title",
				table: "Poll");

			migrationBuilder.CreateIndex(
				name: "IX_Poll_Title",
				table: "Poll",
				column: "Title");
		}
	}
}

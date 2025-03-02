using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddPoll : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Poll",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Summary = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
					IsPublished = table.Column<bool>(type: "bit", nullable: false),
					StartsAt = table.Column<DateOnly>(type: "date", nullable: false),
					EndsAt = table.Column<DateOnly>(type: "date", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Poll", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Poll_Title",
				table: "Poll",
				column: "Title");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Poll");
		}
	}
}

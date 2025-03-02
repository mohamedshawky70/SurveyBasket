using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddVoteAndVoteAnswer : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "votes",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					PollId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_votes", x => x.Id);
					table.ForeignKey(
						name: "FK_votes_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_votes_polls_PollId",
						column: x => x.PollId,
						principalTable: "polls",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "voteAnswers",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					VoteId = table.Column<int>(type: "int", nullable: false),
					QuestionId = table.Column<int>(type: "int", nullable: false),
					AnswerId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_voteAnswers", x => x.Id);
					table.ForeignKey(
						name: "FK_voteAnswers_answers_AnswerId",
						column: x => x.AnswerId,
						principalTable: "answers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_voteAnswers_questions_QuestionId",
						column: x => x.QuestionId,
						principalTable: "questions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_voteAnswers_votes_VoteId",
						column: x => x.VoteId,
						principalTable: "votes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_voteAnswers_AnswerId",
				table: "voteAnswers",
				column: "AnswerId");

			migrationBuilder.CreateIndex(
				name: "IX_voteAnswers_QuestionId",
				table: "voteAnswers",
				column: "QuestionId");

			migrationBuilder.CreateIndex(
				name: "IX_voteAnswers_VoteId_QuestionId",
				table: "voteAnswers",
				columns: new[] { "VoteId", "QuestionId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_votes_PollId_UserId",
				table: "votes",
				columns: new[] { "PollId", "UserId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_votes_UserId",
				table: "votes",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "voteAnswers");

			migrationBuilder.DropTable(
				name: "votes");
		}
	}
}

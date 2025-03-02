﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Data.Migrations
{
	/// <inheritdoc />
	public partial class RenameRefreshTokenTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "RefreshToken");

			migrationBuilder.CreateTable(
				name: "RefreshTokens",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CreatedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
					ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false),
					RevokedIn = table.Column<DateTime>(type: "datetime2", nullable: true)
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

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "RefreshTokens");

			migrationBuilder.CreateTable(
				name: "RefreshToken",
				columns: table => new
				{
					ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CreatedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
					ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false),
					RevokedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
					Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RefreshToken", x => new { x.ApplicationUserId, x.Id });
					table.ForeignKey(
						name: "FK_RefreshToken_AspNetUsers_ApplicationUserId",
						column: x => x.ApplicationUserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});
		}
	}
}

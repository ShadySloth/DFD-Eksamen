using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database_Benchmarking.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    BodyText = table.Column<string>(type: "text", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AuthorUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Authors_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "Authors",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleGenre",
                columns: table => new
                {
                    ArticlesId = table.Column<string>(type: "text", nullable: false),
                    GenresId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleGenre", x => new { x.ArticlesId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_ArticleGenre_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "Action" },
                    { "22222222-2222-2222-2222-222222222222", "Comedy" },
                    { "33333333-3333-3333-3333-333333333333", "Drama" },
                    { "44444444-4444-4444-4444-444444444444", "Horror" },
                    { "55555555-5555-5555-5555-555555555555", "Fantasy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleGenre_GenresId",
                table: "ArticleGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorUserId",
                table: "Articles",
                column: "AuthorUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleGenre");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}

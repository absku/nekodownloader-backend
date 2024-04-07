using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NekoDownloader.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comics",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Resume = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comics", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<float>(type: "real", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ComicUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Chapters_Comics_ComicUuid",
                        column: x => x.ComicUuid,
                        principalTable: "Comics",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    ChapterUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Pages_Chapters_ChapterUuid",
                        column: x => x.ChapterUuid,
                        principalTable: "Chapters",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ComicUuid",
                table: "Chapters",
                column: "ComicUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ChapterUuid",
                table: "Pages",
                column: "ChapterUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Comics");
        }
    }
}

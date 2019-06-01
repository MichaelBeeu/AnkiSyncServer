using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnkiSyncServer.Migrations
{
    public partial class CreateCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Collections",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 9);

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<long>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    NoteId = table.Column<long>(nullable: false),
                    DeckId = table.Column<long>(nullable: false),
                    Ordinal = table.Column<int>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UpdateSequenceNumber = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Queue = table.Column<int>(nullable: false),
                    Due = table.Column<int>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    Factor = table.Column<int>(nullable: false),
                    Repetitions = table.Column<int>(nullable: false),
                    Lapses = table.Column<int>(nullable: false),
                    Left = table.Column<int>(nullable: false),
                    OriginalDue = table.Column<DateTime>(nullable: false),
                    OriginalDeckId = table.Column<int>(nullable: false),
                    Flags = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Collections",
                nullable: false,
                defaultValue: 9,
                oldClrType: typeof(int));
        }
    }
}

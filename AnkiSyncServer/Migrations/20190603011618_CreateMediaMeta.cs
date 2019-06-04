using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnkiSyncServer.Migrations
{
    public partial class CreateMediaMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MediaDirModified",
                table: "Collections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MediaLastSync",
                table: "Collections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MediaMeta",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    DirectoryModified = table.Column<DateTime>(nullable: false),
                    LastUpdateSequenceNumber = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaMeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaMeta_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaMeta_UserId",
                table: "MediaMeta",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaMeta");

            migrationBuilder.DropColumn(
                name: "MediaDirModified",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "MediaLastSync",
                table: "Collections");
        }
    }
}

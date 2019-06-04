using Microsoft.EntityFrameworkCore.Migrations;

namespace AnkiSyncServer.Migrations
{
    public partial class AddClientIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "ReviewLogs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "Notes",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ReviewLogs");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Notes");
        }
    }
}

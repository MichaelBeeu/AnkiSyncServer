using Microsoft.EntityFrameworkCore.Migrations;

namespace AnkiSyncServer.Migrations
{
    public partial class NotesChangeGuidToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Notes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Notes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

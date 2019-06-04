using Microsoft.EntityFrameworkCore.Migrations;

namespace AnkiSyncServer.Migrations
{
    public partial class NoteRenameUpdateSequenceNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpddateSequenceNumber",
                table: "Notes",
                newName: "UpdateSequenceNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateSequenceNumber",
                table: "Notes",
                newName: "UpddateSequenceNumber");
        }
    }
}

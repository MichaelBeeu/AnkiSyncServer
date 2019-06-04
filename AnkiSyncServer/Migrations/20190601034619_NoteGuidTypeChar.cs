using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnkiSyncServer.Migrations
{
    public partial class NoteGuidTypeChar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Notes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Notes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true);
        }
    }
}

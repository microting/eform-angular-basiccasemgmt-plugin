using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Pn.Migrations
{
    public partial class AddPluginSettingsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated_at",
                table: "CalendarUsers",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_at",
                table: "CalendarUsers",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "PluginConfigurationValues",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginConfigurationValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PluginConfigurationVersions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginConfigurationVersions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PluginConfigurationValues_Id",
                table: "PluginConfigurationValues",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PluginConfigurationVersions_Id",
                table: "PluginConfigurationVersions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PluginConfigurationVersions_Id_Version",
                table: "PluginConfigurationVersions",
                columns: new[] { "Id", "Version" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginConfigurationValues");

            migrationBuilder.DropTable(
                name: "PluginConfigurationVersions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Updated_at",
                table: "CalendarUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_at",
                table: "CalendarUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}

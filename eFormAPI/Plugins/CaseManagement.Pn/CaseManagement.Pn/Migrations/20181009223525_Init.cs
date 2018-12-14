using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Pn.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string autoIDGenStrategy = "SqlServer:ValueGenerationStrategy";
            object autoIDGenStrategyValue = SqlServerValueGenerationStrategy.IdentityColumn;

            // Setup for MySQL Provider
            if (migrationBuilder.ActiveProvider == "Pomelo.EntityFrameworkCore.MySql")
            {
                DbConfig.IsMySQL = true;
                autoIDGenStrategy = "MySql:ValueGenerationStrategy";
                autoIDGenStrategyValue = MySqlValueGenerationStrategy.IdentityColumn;
            }
            migrationBuilder.CreateTable(
                name: "CalendarUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Created_at = table.Column<DateTime>(nullable: true),
                    Updated_at = table.Column<DateTime>(nullable: true),
                    Workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    Version = table.Column<int>(nullable: false),
                    Created_By_User_Id = table.Column<int>(nullable: false),
                    Updated_By_User_Id = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: false),
                    IsVisibleInCalendar = table.Column<bool>(nullable: false),
                    NameInCalendar = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    RelatedEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseManagementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    SelectedTemplateId = table.Column<int>(nullable: true),
                    SelectedTemplateName = table.Column<string>(nullable: true),
                    RelatedEntityGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseManagementSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarUsers_SiteId",
                table: "CalendarUsers",
                column: "SiteId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarUsers");

            migrationBuilder.DropTable(
                name: "CaseManagementSettings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTechnologies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTechnology_Projects_ProjectId",
                table: "ProjectTechnology");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTechnology_Technologies_TechnologyId",
                table: "ProjectTechnology");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTechnology",
                table: "ProjectTechnology");

            migrationBuilder.RenameTable(
                name: "ProjectTechnology",
                newName: "ProjectTechnologies");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTechnology_TechnologyId",
                table: "ProjectTechnologies",
                newName: "IX_ProjectTechnologies_TechnologyId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTechnology_ProjectId",
                table: "ProjectTechnologies",
                newName: "IX_ProjectTechnologies_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTechnologies",
                table: "ProjectTechnologies",
                columns: new[] { "ProjectId", "TechnologyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTechnologies_Projects_ProjectId",
                table: "ProjectTechnologies",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTechnologies_Technologies_TechnologyId",
                table: "ProjectTechnologies",
                column: "TechnologyId",
                principalTable: "Technologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTechnologies_Projects_ProjectId",
                table: "ProjectTechnologies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTechnologies_Technologies_TechnologyId",
                table: "ProjectTechnologies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTechnologies",
                table: "ProjectTechnologies");

            migrationBuilder.RenameTable(
                name: "ProjectTechnologies",
                newName: "ProjectTechnology");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTechnologies_TechnologyId",
                table: "ProjectTechnology",
                newName: "IX_ProjectTechnology_TechnologyId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTechnologies_ProjectId",
                table: "ProjectTechnology",
                newName: "IX_ProjectTechnology_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTechnology",
                table: "ProjectTechnology",
                columns: new[] { "ProjectId", "TechnologyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTechnology_Projects_ProjectId",
                table: "ProjectTechnology",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTechnology_Technologies_TechnologyId",
                table: "ProjectTechnology",
                column: "TechnologyId",
                principalTable: "Technologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

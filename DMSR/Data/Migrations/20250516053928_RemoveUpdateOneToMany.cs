using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMSR.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUpdateOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_logs_doc_managements_DocumentDocId",
                table: "activity_logs");

            migrationBuilder.DropIndex(
                name: "IX_activity_logs_DocumentDocId",
                table: "activity_logs");

            migrationBuilder.DropColumn(
                name: "DocId",
                table: "activity_logs");

            migrationBuilder.DropColumn(
                name: "DocumentDocId",
                table: "activity_logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocId",
                table: "activity_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DocumentDocId",
                table: "activity_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_activity_logs_DocumentDocId",
                table: "activity_logs",
                column: "DocumentDocId");

            migrationBuilder.AddForeignKey(
                name: "FK_activity_logs_doc_managements_DocumentDocId",
                table: "activity_logs",
                column: "DocumentDocId",
                principalTable: "doc_managements",
                principalColumn: "DocId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

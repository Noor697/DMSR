using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMSR.Migrations
{
    /// <inheritdoc />
    public partial class AddFileDataListToDocManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "doc_managements");

            migrationBuilder.AddColumn<string>(
                name: "FileDataList",
                table: "doc_managements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "FileNames",
                table: "doc_managements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "FileTypeList",
                table: "doc_managements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDataList",
                table: "doc_managements");

            migrationBuilder.DropColumn(
                name: "FileNames",
                table: "doc_managements");

            migrationBuilder.DropColumn(
                name: "FileTypeList",
                table: "doc_managements");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "doc_managements",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}

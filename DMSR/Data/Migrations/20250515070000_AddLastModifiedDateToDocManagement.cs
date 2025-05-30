using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMSR.Migrations
{
    /// <inheritdoc />
    public partial class AddLastModifiedDateToDocManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedDate",
                table: "doc_managements",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "LastModifiedDate",
                table: "doc_managements",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}

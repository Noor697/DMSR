using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMSR.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doc_managements",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Document_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Docfile = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Document_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastModifiedDate = table.Column<TimeOnly>(type: "time", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_managements", x => x.DocId);
                });

            migrationBuilder.CreateTable(
                name: "user_managements",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_managements", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Doc_ManagementUser_Management",
                columns: table => new
                {
                    DocumentsDocId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doc_ManagementUser_Management", x => new { x.DocumentsDocId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_Doc_ManagementUser_Management_doc_managements_DocumentsDocId",
                        column: x => x.DocumentsDocId,
                        principalTable: "doc_managements",
                        principalColumn: "DocId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doc_ManagementUser_Management_user_managements_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "user_managements",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doc_ManagementUser_Management_UsersUserId",
                table: "Doc_ManagementUser_Management",
                column: "UsersUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doc_ManagementUser_Management");

            migrationBuilder.DropTable(
                name: "doc_managements");

            migrationBuilder.DropTable(
                name: "user_managements");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class DbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblFolders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FolderPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblFolders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TblFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(MAX)", nullable: false),
                    FoldersID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TblFiles_TblFolders_FoldersID",
                        column: x => x.FoldersID,
                        principalTable: "TblFolders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblFiles_FoldersID",
                table: "TblFiles",
                column: "FoldersID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblFiles");

            migrationBuilder.DropTable(
                name: "TblFolders");
        }
    }
}

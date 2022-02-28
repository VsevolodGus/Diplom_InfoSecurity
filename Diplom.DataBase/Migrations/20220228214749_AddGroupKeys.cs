using Microsoft.EntityFrameworkCore.Migrations;

namespace Diplom.DataBase.Migrations
{
    public partial class AddGroupKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "FileModels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "GroupByKeys",
                columns: table => new
                {
                    PKID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupByKeys", x => x.PKID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileModels_GroupId",
                table: "FileModels",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileModels_GroupByKeys_GroupId",
                table: "FileModels",
                column: "GroupId",
                principalTable: "GroupByKeys",
                principalColumn: "PKID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileModels_GroupByKeys_GroupId",
                table: "FileModels");

            migrationBuilder.DropTable(
                name: "GroupByKeys");

            migrationBuilder.DropIndex(
                name: "IX_FileModels_GroupId",
                table: "FileModels");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "FileModels");
        }
    }
}

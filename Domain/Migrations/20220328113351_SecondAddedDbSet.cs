using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class SecondAddedDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Players_PlayerId",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Board_BoardId",
                table: "Field");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Board");

            migrationBuilder.RenameTable(
                name: "Board",
                newName: "Boards");

            migrationBuilder.RenameIndex(
                name: "IX_Board_PlayerId",
                table: "Boards",
                newName: "IX_Boards_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boards",
                table: "Boards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Players_PlayerId",
                table: "Boards",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Boards_BoardId",
                table: "Field",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Players_PlayerId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Boards_BoardId",
                table: "Field");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Boards",
                table: "Boards");

            migrationBuilder.RenameTable(
                name: "Boards",
                newName: "Board");

            migrationBuilder.RenameIndex(
                name: "IX_Boards_PlayerId",
                table: "Board",
                newName: "IX_Board_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Board",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Players_PlayerId",
                table: "Board",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Board_BoardId",
                table: "Field",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

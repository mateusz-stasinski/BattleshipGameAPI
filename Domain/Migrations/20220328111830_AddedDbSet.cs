using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddedDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Player_PlayerId",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Ship_ShipId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Games_GameId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Ship_Player_PlayerId",
                table: "Ship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ship",
                table: "Ship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.RenameTable(
                name: "Ship",
                newName: "Ships");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.RenameIndex(
                name: "IX_Ship_PlayerId",
                table: "Ships",
                newName: "IX_Ships_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_GameId",
                table: "Players",
                newName: "IX_Players_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ships",
                table: "Ships",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Players_PlayerId",
                table: "Board",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Ships_ShipId",
                table: "Field",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_GameId",
                table: "Players",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_Players_PlayerId",
                table: "Ships",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Players_PlayerId",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Ships_ShipId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_GameId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_Players_PlayerId",
                table: "Ships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ships",
                table: "Ships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Ships",
                newName: "Ship");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameIndex(
                name: "IX_Ships_PlayerId",
                table: "Ship",
                newName: "IX_Ship_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_GameId",
                table: "Player",
                newName: "IX_Player_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ship",
                table: "Ship",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Player_PlayerId",
                table: "Board",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Ship_ShipId",
                table: "Field",
                column: "ShipId",
                principalTable: "Ship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Games_GameId",
                table: "Player",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ship_Player_PlayerId",
                table: "Ship",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

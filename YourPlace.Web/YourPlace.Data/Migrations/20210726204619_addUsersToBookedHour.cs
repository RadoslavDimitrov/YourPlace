using Microsoft.EntityFrameworkCore.Migrations;

namespace YourPlace.Data.Migrations
{
    public partial class addUsersToBookedHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

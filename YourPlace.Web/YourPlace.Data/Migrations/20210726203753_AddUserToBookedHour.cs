using Microsoft.EntityFrameworkCore.Migrations;

namespace YourPlace.Data.Migrations
{
    public partial class AddUserToBookedHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BookedHours",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BookedHours",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedHours_AspNetUsers_UserId",
                table: "BookedHours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

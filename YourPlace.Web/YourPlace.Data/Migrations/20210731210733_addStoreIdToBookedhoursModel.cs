using Microsoft.EntityFrameworkCore.Migrations;

namespace YourPlace.Data.Migrations
{
    public partial class addStoreIdToBookedhoursModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoreId",
                table: "BookedHours",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "BookedHours");
        }
    }
}

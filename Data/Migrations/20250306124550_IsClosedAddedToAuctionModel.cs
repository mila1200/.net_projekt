using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardHaven.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsClosedAddedToAuctionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Auctions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Auctions");
        }
    }
}

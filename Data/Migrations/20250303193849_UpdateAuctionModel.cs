using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardHaven.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuctionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Cards_CardId",
                table: "Auctions");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CardId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "StartPrice",
                table: "Auctions",
                newName: "Set");

            migrationBuilder.RenameColumn(
                name: "CurrentPrice",
                table: "Auctions",
                newName: "Name");

            migrationBuilder.AddColumn<decimal>(
                name: "AskingPrice",
                table: "Auctions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Auctions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auctions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Auctions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskingPrice",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "Set",
                table: "Auctions",
                newName: "StartPrice");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Auctions",
                newName: "CurrentPrice");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Auctions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Condition = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Set = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CardId",
                table: "Auctions",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Cards_CardId",
                table: "Auctions",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

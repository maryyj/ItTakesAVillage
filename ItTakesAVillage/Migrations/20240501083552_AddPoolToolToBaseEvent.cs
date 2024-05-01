#nullable disable

namespace ItTakesAVillage.Migrations;

/// <inheritdoc />
public partial class AddPoolToolToBaseEvent : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "BorrowerId",
            table: "Events",
            type: "nvarchar(450)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Events",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "FromDate",
            table: "Events",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsBorrowed",
            table: "Events",
            type: "bit",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Events",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "ToDate",
            table: "Events",
            type: "date",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Events_BorrowerId",
            table: "Events",
            column: "BorrowerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Events_AspNetUsers_BorrowerId",
            table: "Events",
            column: "BorrowerId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Events_AspNetUsers_BorrowerId",
            table: "Events");

        migrationBuilder.DropIndex(
            name: "IX_Events_BorrowerId",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "BorrowerId",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "FromDate",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "IsBorrowed",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "ToDate",
            table: "Events");
    }
}

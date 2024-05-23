#nullable disable

namespace ItTakesAVillage.Migrations;

/// <inheritdoc />
public partial class AddToolLoanAndToolPool : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
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
            name: "FromDate",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "ToDate",
            table: "Events");

        migrationBuilder.CreateTable(
            name: "ToolLoans",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                ToDate = table.Column<DateOnly>(type: "date", nullable: false),
                BorrowerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                ToolId = table.Column<int>(type: "int", nullable: false),
                ToolPoolId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ToolLoans", x => x.Id);
                table.ForeignKey(
                    name: "FK_ToolLoans_AspNetUsers_BorrowerId",
                    column: x => x.BorrowerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ToolLoans_Events_ToolPoolId",
                    column: x => x.ToolPoolId,
                    principalTable: "Events",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_ToolLoans_BorrowerId",
            table: "ToolLoans",
            column: "BorrowerId");

        migrationBuilder.CreateIndex(
            name: "IX_ToolLoans_ToolPoolId",
            table: "ToolLoans",
            column: "ToolPoolId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ToolLoans");

        migrationBuilder.AddColumn<string>(
            name: "BorrowerId",
            table: "Events",
            type: "nvarchar(450)",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "FromDate",
            table: "Events",
            type: "date",
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
}

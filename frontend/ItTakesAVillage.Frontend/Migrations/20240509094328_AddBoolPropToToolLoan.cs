
#nullable disable

namespace ItTakesAVillage.Migrations;

/// <inheritdoc />
public partial class AddBoolPropToToolLoan : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsReturned",
            table: "ToolLoans",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsReturned",
            table: "ToolLoans");
    }
}

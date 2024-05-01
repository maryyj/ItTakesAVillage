#nullable disable

namespace ItTakesAVillage.Migrations;

/// <inheritdoc />
public partial class AddImagePropToPoolTool : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Image",
            table: "Events",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Image",
            table: "Events");
    }
}

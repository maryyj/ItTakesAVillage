
#nullable disable

namespace ItTakesAVillage.Frontend.Migrations;

/// <inheritdoc />
public partial class ChangeToNullableRelatedEvent : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Events_RelatedEventId",
            table: "Notifications");

        migrationBuilder.AlterColumn<int>(
            name: "RelatedEventId",
            table: "Notifications",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Events_RelatedEventId",
            table: "Notifications",
            column: "RelatedEventId",
            principalTable: "Events",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Events_RelatedEventId",
            table: "Notifications");

        migrationBuilder.AlterColumn<int>(
            name: "RelatedEventId",
            table: "Notifications",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Events_RelatedEventId",
            table: "Notifications",
            column: "RelatedEventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

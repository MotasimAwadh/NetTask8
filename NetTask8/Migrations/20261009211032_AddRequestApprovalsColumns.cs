using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetTask8.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestApprovalsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approval1",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Approval2",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approval1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Approval2",
                table: "Requests");
        }
    }
}

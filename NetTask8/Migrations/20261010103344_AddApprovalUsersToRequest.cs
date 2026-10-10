using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetTask8.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovalUsersToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_CreatorId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "Approval1UserId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Approval2UserId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Approval1UserId",
                table: "Requests",
                column: "Approval1UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Approval2UserId",
                table: "Requests",
                column: "Approval2UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_Approval1UserId",
                table: "Requests",
                column: "Approval1UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_Approval2UserId",
                table: "Requests",
                column: "Approval2UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_CreatorId",
                table: "Requests",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_Approval1UserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_Approval2UserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_CreatorId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_Approval1UserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_Approval2UserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Approval1UserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Approval2UserId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_CreatorId",
                table: "Requests",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

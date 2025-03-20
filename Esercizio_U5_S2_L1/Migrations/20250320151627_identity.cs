using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esercizio_U5_S2_L1.Migrations
{
    /// <inheritdoc />
    public partial class identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Studenti",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Studenti",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_ApplicationUserId",
                table: "Studenti",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Studenti_AspNetUsers_ApplicationUserId",
                table: "Studenti",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studenti_AspNetUsers_ApplicationUserId",
                table: "Studenti");

            migrationBuilder.DropIndex(
                name: "IX_Studenti_ApplicationUserId",
                table: "Studenti");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Studenti");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Studenti");
        }
    }
}

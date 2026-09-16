using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class CompleteBrewingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Uhik",
                table: "Koostisosad",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "Hind",
                table: "Koostisosad",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Kogus",
                table: "Koostisosad",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PartiiId",
                table: "Koostisosad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PartiiFotod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartiiId = table.Column<int>(type: "int", nullable: false),
                    FailiTee = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartiiFotod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartiiFotod_Partiid_PartiiId",
                        column: x => x.PartiiId,
                        principalTable: "Partiid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PruulimisLogid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartiiId = table.Column<int>(type: "int", nullable: false),
                    Kuupaev = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kasutaja = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kirjeldus = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PruulimisLogid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PruulimisLogid_Partiid_PartiiId",
                        column: x => x.PartiiId,
                        principalTable: "Partiid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partiid_OluId",
                table: "Partiid",
                column: "OluId");

            migrationBuilder.CreateIndex(
                name: "IX_Maitsmised_PartiiId",
                table: "Maitsmised",
                column: "PartiiId");

            migrationBuilder.CreateIndex(
                name: "IX_Koostisosad_PartiiId",
                table: "Koostisosad",
                column: "PartiiId");

            migrationBuilder.CreateIndex(
                name: "IX_PartiiFotod_PartiiId",
                table: "PartiiFotod",
                column: "PartiiId");

            migrationBuilder.CreateIndex(
                name: "IX_PruulimisLogid_PartiiId",
                table: "PruulimisLogid",
                column: "PartiiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Koostisosad_Partiid_PartiiId",
                table: "Koostisosad",
                column: "PartiiId",
                principalTable: "Partiid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maitsmised_Partiid_PartiiId",
                table: "Maitsmised",
                column: "PartiiId",
                principalTable: "Partiid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partiid_Olud_OluId",
                table: "Partiid",
                column: "OluId",
                principalTable: "Olud",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koostisosad_Partiid_PartiiId",
                table: "Koostisosad");

            migrationBuilder.DropForeignKey(
                name: "FK_Maitsmised_Partiid_PartiiId",
                table: "Maitsmised");

            migrationBuilder.DropForeignKey(
                name: "FK_Partiid_Olud_OluId",
                table: "Partiid");

            migrationBuilder.DropTable(
                name: "PartiiFotod");

            migrationBuilder.DropTable(
                name: "PruulimisLogid");

            migrationBuilder.DropIndex(
                name: "IX_Partiid_OluId",
                table: "Partiid");

            migrationBuilder.DropIndex(
                name: "IX_Maitsmised_PartiiId",
                table: "Maitsmised");

            migrationBuilder.DropIndex(
                name: "IX_Koostisosad_PartiiId",
                table: "Koostisosad");

            migrationBuilder.DropColumn(
                name: "Kogus",
                table: "Koostisosad");

            migrationBuilder.DropColumn(
                name: "PartiiId",
                table: "Koostisosad");

            migrationBuilder.AlterColumn<string>(
                name: "Uhik",
                table: "Koostisosad",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "Hind",
                table: "Koostisosad",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");
        }
    }
}

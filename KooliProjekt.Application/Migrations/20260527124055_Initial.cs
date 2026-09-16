using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Koostisosad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimetus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uhik = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Hind = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kirjeldus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koostisosad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maitsmised",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartiiId = table.Column<int>(type: "int", nullable: false),
                    Kuupaev = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Degusteerija = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Hinne = table.Column<int>(type: "int", nullable: false),
                    Kommentaar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maitsmised", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Olud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kirjeldus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Tuup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Alkoholiprotsent = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Olud", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partiid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OluId = table.Column<int>(type: "int", nullable: false),
                    Kood = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Kuupaev = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kirjeldus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Tulemus = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partiid", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Koostisosad");

            migrationBuilder.DropTable(
                name: "Maitsmised");

            migrationBuilder.DropTable(
                name: "Olud");

            migrationBuilder.DropTable(
                name: "Partiid");
        }
    }
}

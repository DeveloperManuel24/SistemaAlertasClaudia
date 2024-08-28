using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaAlertasBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class LecturaAlerta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertaEntitys",
                columns: table => new
                {
                    AlertaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    SensorEntidadSensorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertaEntitys", x => x.AlertaId);
                    table.ForeignKey(
                        name: "FK_AlertaEntitys_SensorEntitys_SensorEntidadSensorId",
                        column: x => x.SensorEntidadSensorId,
                        principalTable: "SensorEntitys",
                        principalColumn: "SensorId");
                });

            migrationBuilder.CreateTable(
                name: "LecturaEntitys",
                columns: table => new
                {
                    ReadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Unity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ph_parameter = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    orp_parameter = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    turbidez_parameter = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    SensorEntidadSensorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturaEntitys", x => x.ReadId);
                    table.ForeignKey(
                        name: "FK_LecturaEntitys_SensorEntitys_SensorEntidadSensorId",
                        column: x => x.SensorEntidadSensorId,
                        principalTable: "SensorEntitys",
                        principalColumn: "SensorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertaEntitys_SensorEntidadSensorId",
                table: "AlertaEntitys",
                column: "SensorEntidadSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturaEntitys_SensorEntidadSensorId",
                table: "LecturaEntitys",
                column: "SensorEntidadSensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertaEntitys");

            migrationBuilder.DropTable(
                name: "LecturaEntitys");
        }
    }
}

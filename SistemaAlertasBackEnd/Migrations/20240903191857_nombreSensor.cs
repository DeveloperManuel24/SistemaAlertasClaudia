﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaAlertasBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class nombreSensor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreSensor",
                table: "SensorEntitys",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreSensor",
                table: "SensorEntitys");
        }
    }
}

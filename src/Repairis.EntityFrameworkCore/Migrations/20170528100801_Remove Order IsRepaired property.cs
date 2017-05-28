using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repairis.Migrations
{
    public partial class RemoveOrderIsRepairedproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRepaired",
                table: "Orders");

            migrationBuilder.AlterColumn<decimal>(
                name: "SupplierPrice",
                table: "SpareParts",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SupplierPrice",
                table: "SpareParts",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<bool>(
                name: "IsRepaired",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }
    }
}

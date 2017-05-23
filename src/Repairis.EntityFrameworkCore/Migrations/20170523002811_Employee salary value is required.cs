using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repairis.Migrations
{
    public partial class Employeesalaryvalueisrequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryValue",
                table: "EmployeeInfo",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryValue",
                table: "EmployeeInfo",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}

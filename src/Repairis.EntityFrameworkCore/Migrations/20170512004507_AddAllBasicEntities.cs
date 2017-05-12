using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Repairis.Migrations
{
    public partial class AddAllBasicEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AbpUsers",
                maxLength: 26,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AbpUsers",
                maxLength: 550,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerInfoId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeInfoId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "AbpUsers",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhoneNumber",
                table: "AbpUsers",
                maxLength: 26,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(maxLength: 2048, nullable: true),
                    CustomerType = table.Column<int>(nullable: false),
                    CustomerUserId = table.Column<long>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInfo_AbpUsers_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    EmployeeUserId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SalaryIsFlat = table.Column<bool>(nullable: false),
                    SalaryValue = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeInfo_AbpUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandName = table.Column<string>(maxLength: 200, nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceCategoryName = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<int>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 2048, nullable: true),
                    RecommendedPrice = table.Column<decimal>(nullable: true),
                    SparePartCode = table.Column<string>(maxLength: 200, nullable: true),
                    SparePartName = table.Column<string>(maxLength: 200, nullable: false),
                    StockQuantity = table.Column<int>(nullable: false),
                    StockStatus = table.Column<int>(nullable: false),
                    SupplierPrice = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpareParts_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<int>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceCategoryId = table.Column<int>(nullable: false),
                    DeviceModelName = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceModels_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceModels_DeviceCategories_DeviceCategoryId",
                        column: x => x.DeviceCategoryId,
                        principalTable: "DeviceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceCategoryId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IssueName = table.Column<string>(maxLength: 200, nullable: false),
                    RecommendedPrice = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_DeviceCategories_DeviceCategoryId",
                        column: x => x.DeviceCategoryId,
                        principalTable: "DeviceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceModelId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SerialNumber = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceModels_DeviceModelId",
                        column: x => x.DeviceModelId,
                        principalTable: "DeviceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SparePartCompatibility",
                columns: table => new
                {
                    DeviceModelId = table.Column<int>(nullable: false),
                    SparePartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartCompatibility", x => new { x.DeviceModelId, x.SparePartId });
                    table.ForeignKey(
                        name: "FK_SparePartCompatibility_DeviceModels_DeviceModelId",
                        column: x => x.DeviceModelId,
                        principalTable: "DeviceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SparePartCompatibility_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalEquipment = table.Column<string>(nullable: true),
                    AdditionalNotes = table.Column<string>(nullable: true),
                    AssignedEmployeeId = table.Column<long>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeviceId = table.Column<int>(nullable: false),
                    DevicePickupDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsRepaired = table.Column<bool>(nullable: false),
                    IsUrgent = table.Column<bool>(nullable: false),
                    IsWarrantyComplaint = table.Column<bool>(nullable: false),
                    IssueDescription = table.Column<string>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    OrderRepairedDate = table.Column<DateTime>(nullable: true),
                    OrderStatus = table.Column<int>(nullable: false),
                    RepairPrice = table.Column<decimal>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WarrantyExpirationDate = table.Column<DateTime>(nullable: true),
                    WorkDoneDescripton = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_EmployeeInfo_AssignedEmployeeId",
                        column: x => x.AssignedEmployeeId,
                        principalTable: "EmployeeInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_CustomerInfo_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssueOrderMappings",
                columns: table => new
                {
                    OrderId = table.Column<long>(nullable: false),
                    IssueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueOrderMappings", x => new { x.OrderId, x.IssueId });
                    table.ForeignKey(
                        name: "FK_IssueOrderMappings_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssueOrderMappings_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SparePartOrderMappings",
                columns: table => new
                {
                    OrderId = table.Column<long>(nullable: false),
                    SparePartId = table.Column<int>(nullable: false),
                    PricePerItem = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartOrderMappings", x => new { x.OrderId, x.SparePartId });
                    table.ForeignKey(
                        name: "FK_SparePartOrderMappings_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SparePartOrderMappings_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_CustomerInfoId",
                table: "AbpUsers",
                column: "CustomerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_EmployeeInfoId",
                table: "AbpUsers",
                column: "EmployeeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfo_CustomerUserId",
                table: "CustomerInfo",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInfo_EmployeeUserId",
                table: "EmployeeInfo",
                column: "EmployeeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandName",
                table: "Brands",
                column: "BrandName");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCategories_DeviceCategoryName",
                table: "DeviceCategories",
                column: "DeviceCategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_BrandId",
                table: "DeviceModels",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_DeviceCategoryId",
                table: "DeviceModels",
                column: "DeviceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_DeviceModelName",
                table: "DeviceModels",
                column: "DeviceModelName");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_DeviceCategoryId",
                table: "Issues",
                column: "DeviceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueOrderMappings_IssueId",
                table: "IssueOrderMappings",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedEmployeeId",
                table: "Orders",
                column: "AssignedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeviceId",
                table: "Orders",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_BrandId",
                table: "SpareParts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartCompatibility_SparePartId",
                table: "SparePartCompatibility",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartOrderMappings_SparePartId",
                table: "SparePartOrderMappings",
                column: "SparePartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_CustomerInfo_CustomerInfoId",
                table: "AbpUsers",
                column: "CustomerInfoId",
                principalTable: "CustomerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_EmployeeInfo_EmployeeInfoId",
                table: "AbpUsers",
                column: "EmployeeInfoId",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_CustomerInfo_CustomerInfoId",
                table: "AbpUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_EmployeeInfo_EmployeeInfoId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "IssueOrderMappings");

            migrationBuilder.DropTable(
                name: "SparePartCompatibility");

            migrationBuilder.DropTable(
                name: "SparePartOrderMappings");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "EmployeeInfo");

            migrationBuilder.DropTable(
                name: "CustomerInfo");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "DeviceModels");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "DeviceCategories");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_CustomerInfoId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_EmployeeInfoId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CustomerInfoId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeInfoId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "SecondaryPhoneNumber",
                table: "AbpUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AbpUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 26,
                oldNullable: true);
        }
    }
}

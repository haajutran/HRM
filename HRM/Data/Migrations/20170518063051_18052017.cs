using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRM.Data.Migrations
{
    public partial class _18052017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentCode = table.Column<int>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    CitizenID = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<string>(nullable: true),
                    DepartmentCode = table.Column<int>(nullable: false),
                    DepartmentID = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    HomeTown = table.Column<string>(nullable: true),
                    OutOfWork = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    PlaceOfProvide = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    TempAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FamilyRelation",
                columns: table => new
                {
                    FamilyRelationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Occupation = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Relation = table.Column<string>(nullable: true),
                    WorkPlace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyRelation", x => x.FamilyRelationId);
                    table.ForeignKey(
                        name: "FK_FamilyRelation_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentID",
                table: "Employee",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyRelation_EmployeeId",
                table: "FamilyRelation",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyRelation");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}

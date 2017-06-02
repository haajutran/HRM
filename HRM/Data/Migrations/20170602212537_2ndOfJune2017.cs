﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRM.Data.Migrations
{
    public partial class _2ndOfJune2017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayPerHour = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    CitizenID = table.Column<string>(maxLength: 9, nullable: false),
                    City = table.Column<string>(nullable: true),
                    ContractID = table.Column<int>(nullable: true),
                    DateOfBirth = table.Column<string>(nullable: false),
                    DateOfJoining = table.Column<DateTime>(nullable: false),
                    DepartmentCode = table.Column<int>(nullable: false),
                    DepartmentTitle = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    EmployeeCode = table.Column<int>(nullable: false),
                    ExitDate = table.Column<DateTime>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    HomeTown = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 13, nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    PlaceOfProvide = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    TempAddress = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_Contract_ContractID",
                        column: x => x.ContractID,
                        principalTable: "Contract",
                        principalColumn: "ContractID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentCode = table.Column<int>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_Department_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
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

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    SalaryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    Earned = table.Column<long>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: true),
                    PayPerHour = table.Column<long>(nullable: false),
                    RecordDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.SalaryID);
                    table.ForeignKey(
                        name: "FK_Salary_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTask",
                columns: table => new
                {
                    DepartmentTaskID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    WorkHours = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTask", x => x.DepartmentTaskID);
                    table.ForeignKey(
                        name: "FK_DepartmentTask_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentTask_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTitle",
                columns: table => new
                {
                    DepartmentTitleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<int>(nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTitle", x => x.DepartmentTitleID);
                    table.ForeignKey(
                        name: "FK_DepartmentTitle_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentTitle_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_EmployeeID",
                table: "Department",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTask_DepartmentID",
                table: "DepartmentTask",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTask_EmployeeID",
                table: "DepartmentTask",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTitle_DepartmentID",
                table: "DepartmentTitle",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTitle_EmployeeID",
                table: "DepartmentTitle",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ContractID",
                table: "Employee",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyRelation_EmployeeId",
                table: "FamilyRelation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_EmployeeID",
                table: "Salary",
                column: "EmployeeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentTask");

            migrationBuilder.DropTable(
                name: "DepartmentTitle");

            migrationBuilder.DropTable(
                name: "FamilyRelation");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Contract");
        }
    }
}

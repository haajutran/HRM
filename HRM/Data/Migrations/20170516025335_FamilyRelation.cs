using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRM.Data.Migrations
{
    public partial class FamilyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DepartmentID = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<int>(nullable: false),
                    Family = table.Column<int>(nullable: false),
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
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "FamilyRelation");
        }
    }
}

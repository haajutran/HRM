using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HRM.Data;

namespace HRM.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170602092206_2ndOfJune2017")]
    partial class _2ndOfJune2017
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HRM.Models.AppUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HRM.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentCode");

                    b.Property<string>("DepartmentName")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<int?>("EmployeeID");

                    b.HasKey("DepartmentID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("HRM.Models.DepartmentTask", b =>
                {
                    b.Property<int>("DepartmentTaskID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("Description");

                    b.Property<int?>("EmployeeID");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("WorkHours");

                    b.HasKey("DepartmentTaskID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("DepartmentTask");
                });

            modelBuilder.Entity("HRM.Models.DepartmentTitle", b =>
                {
                    b.Property<int>("DepartmentTitleID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DepartmentID");

                    b.Property<string>("Description");

                    b.Property<int?>("EmployeeID");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("DepartmentTitleID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("DepartmentTitle");
                });

            modelBuilder.Entity("HRM.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Address");

                    b.Property<string>("Avatar");

                    b.Property<string>("CitizenID")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.Property<string>("City");

                    b.Property<string>("DateOfBirth")
                        .IsRequired();

                    b.Property<DateTime>("DateOfJoining");

                    b.Property<int>("DepartmentCode");

                    b.Property<string>("DepartmentTitle");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("EmployeeCode");

                    b.Property<DateTime>("ExitDate");

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<string>("Gender");

                    b.Property<string>("HomeTown");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PlaceOfBirth");

                    b.Property<string>("PlaceOfProvide");

                    b.Property<string>("Region");

                    b.Property<string>("TempAddress")
                        .IsRequired();

                    b.HasKey("EmployeeID");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("HRM.Models.FamilyRelation", b =>
                {
                    b.Property<int>("FamilyRelationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("DateOfBirth");

                    b.Property<string>("Description");

                    b.Property<int>("EmployeeId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Occupation");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Relation");

                    b.Property<string>("WorkPlace");

                    b.HasKey("FamilyRelationId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("FamilyRelation");
                });

            modelBuilder.Entity("HRM.Models.Salary", b =>
                {
                    b.Property<int>("SalaryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<long>("Earned");

                    b.Property<int?>("EmployeeID");

                    b.Property<long>("PayPerHour");

                    b.Property<string>("RecordDate");

                    b.HasKey("SalaryID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Salary");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HRM.Models.Department", b =>
                {
                    b.HasOne("HRM.Models.Employee")
                        .WithMany("Departments")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("HRM.Models.DepartmentTask", b =>
                {
                    b.HasOne("HRM.Models.Department", "Department")
                        .WithMany("DepartmentTasks")
                        .HasForeignKey("DepartmentID");

                    b.HasOne("HRM.Models.Employee", "Employee")
                        .WithMany("DepartmentTasks")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("HRM.Models.DepartmentTitle", b =>
                {
                    b.HasOne("HRM.Models.Department", "Department")
                        .WithMany("DepartmentTitles")
                        .HasForeignKey("DepartmentID");

                    b.HasOne("HRM.Models.Employee", "Employee")
                        .WithMany("DepartmentTitles")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("HRM.Models.FamilyRelation", b =>
                {
                    b.HasOne("HRM.Models.Employee", "Employee")
                        .WithMany("FamilyRelations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HRM.Models.Salary", b =>
                {
                    b.HasOne("HRM.Models.Employee", "Employee")
                        .WithMany("SalaryRecords")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HRM.Models.AppUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HRM.Models.AppUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HRM.Models.AppUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

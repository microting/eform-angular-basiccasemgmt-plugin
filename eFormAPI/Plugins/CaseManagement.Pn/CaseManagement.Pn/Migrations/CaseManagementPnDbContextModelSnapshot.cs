﻿// <auto-generated />
using System;
using CaseManagement.Pn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CaseManagement.Pn.Migrations
{
    [DbContext(typeof(CaseManagementPnDbContext))]
    partial class CaseManagementPnDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            string autoIDGenStrategy = "SqlServer:ValueGenerationStrategy";
            object autoIDGenStrategyValue = SqlServerValueGenerationStrategy.IdentityColumn;
            if (DbConfig.IsMySQL)
            {
                autoIDGenStrategy = "MySql:ValueGenerationStrategy";
                autoIDGenStrategyValue = MySqlValueGenerationStrategy.IdentityColumn;
            }

            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

            modelBuilder.Entity("CaseManagement.Pn.Infrastructure.Data.Entities.CalendarUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

                    b.Property<string>("Color");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedByUserId");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsVisibleInCalendar");

                    b.Property<string>("LastName");

                    b.Property<string>("NameInCalendar");

                    b.Property<int>("RelatedEntityId");

                    b.Property<int>("SiteId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("UpdatedByUserId");

                    b.Property<int>("Version");

                    b.Property<string>("WorkflowState")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("SiteId")
                        .IsUnique();

                    b.ToTable("CalendarUsers");
                });

            modelBuilder.Entity("CaseManagement.Pn.Infrastructure.Data.Entities.CalendarUserVersions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

                    b.Property<int>("CalendarUserId");

                    b.Property<string>("Color");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedByUserId");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsVisibleInCalendar");

                    b.Property<string>("LastName");

                    b.Property<string>("NameInCalendar");

                    b.Property<int>("RelatedEntityId");

                    b.Property<int>("SiteId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("UpdatedByUserId");

                    b.Property<int>("Version");

                    b.Property<string>("WorkflowState")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("CalendarUserVersions");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.PluginConfigurationValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedByUserId");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("UpdatedByUserId");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.Property<string>("WorkflowState")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("PluginConfigurationValues");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.PluginConfigurationValueVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedByUserId");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("UpdatedByUserId");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.Property<string>("WorkflowState")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("PluginConfigurationValueVersions");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.PluginGroupPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedByUserId");

                    b.Property<int>("GroupId");

                    b.Property<int>("PermissionId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("UpdatedByUserId");

                    b.Property<int>("Version");

                    b.Property<string>("WorkflowState")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.ToTable("PluginGroupPermissions");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.PluginPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation(autoIDGenStrategy, autoIDGenStrategyValue);

                    b.Property<string>("ClaimName");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CreatedByUserId");

                    b.Property<string>("PermissionName");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("UpdatedByUserId");

                    b.Property<int>("Version");

                    b.Property<string>("WorkflowState")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("PluginPermissions");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.PluginGroupPermission", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.PluginPermission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

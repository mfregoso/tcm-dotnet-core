﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCM.Models.Entities;

namespace TCM.Web.Migrations
{
    [DbContext(typeof(ClubDataContext))]
    [Migration("20190424040538_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TCM.Models.Entities.Club", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(8);

                    b.Property<bool>("Exists");

                    b.Property<int?>("MembershipCount");

                    b.HasKey("Id");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("TCM.Models.Entities.MetricsHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClubId")
                        .HasMaxLength(8);

                    b.Property<int?>("Goals");

                    b.Property<int?>("Members");

                    b.Property<string>("MonthEnd")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("MetricsHistory");
                });

            modelBuilder.Entity("TCM.Models.Entities.MetricsHistory", b =>
                {
                    b.HasOne("TCM.Models.Entities.Club")
                        .WithMany("MetricsHistory")
                        .HasForeignKey("ClubId");
                });
#pragma warning restore 612, 618
        }
    }
}

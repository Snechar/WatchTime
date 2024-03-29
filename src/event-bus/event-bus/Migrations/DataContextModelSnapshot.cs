﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using event_bus.Context;

namespace event_bus.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("event_bus.Models.FailedRequest", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Channel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RequestDTOID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RequestDTOID");

                    b.ToTable("failedRequests");
                });

            modelBuilder.Entity("event_bus.Models.RequestDTO", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("RequestDTO");
                });

            modelBuilder.Entity("event_bus.Models.SuccessfulRequests", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Channel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RequestDTOID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RequestDTOID");

                    b.ToTable("successfulRequests");
                });

            modelBuilder.Entity("event_bus.Models.FailedRequest", b =>
                {
                    b.HasOne("event_bus.Models.RequestDTO", "RequestDTO")
                        .WithMany()
                        .HasForeignKey("RequestDTOID");

                    b.Navigation("RequestDTO");
                });

            modelBuilder.Entity("event_bus.Models.SuccessfulRequests", b =>
                {
                    b.HasOne("event_bus.Models.RequestDTO", "RequestDTO")
                        .WithMany()
                        .HasForeignKey("RequestDTOID");

                    b.Navigation("RequestDTO");
                });
#pragma warning restore 612, 618
        }
    }
}

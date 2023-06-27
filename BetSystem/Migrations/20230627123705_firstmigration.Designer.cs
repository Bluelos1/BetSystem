﻿// <auto-generated />
using System;
using BetSystem.BetSystemDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BetSystem.Migrations
{
    [DbContext(typeof(BetDbContext))]
    [Migration("20230627123705_firstmigration")]
    partial class firstmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BetSystem.Model.BetOnEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int>("AmountToPay")
                        .HasColumnType("integer");

                    b.Property<int>("BetOnResult")
                        .HasColumnType("integer");

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int?>("EventResultId")
                        .HasColumnType("integer");

                    b.Property<int>("Interest")
                        .HasColumnType("integer");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("EventResultId");

                    b.HasIndex("TeamId");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("BetSystem.Model.EventResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BetOnResult")
                        .HasColumnType("integer");

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("TeamId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("BetSystem.Model.SportEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("BetSystem.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("SportEventId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SportEventId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("BetSystem.Model.BetOnEvent", b =>
                {
                    b.HasOne("BetSystem.Model.SportEvent", "Event")
                        .WithMany("Bets")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BetSystem.Model.EventResult", null)
                        .WithMany("Bets")
                        .HasForeignKey("EventResultId");

                    b.HasOne("BetSystem.Model.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BetSystem.Model.EventResult", b =>
                {
                    b.HasOne("BetSystem.Model.SportEvent", "Event")
                        .WithMany("EventResults")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BetSystem.Model.Team", "Team")
                        .WithMany("Results")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BetSystem.Model.Team", b =>
                {
                    b.HasOne("BetSystem.Model.SportEvent", null)
                        .WithMany("Teams")
                        .HasForeignKey("SportEventId");
                });

            modelBuilder.Entity("BetSystem.Model.EventResult", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("BetSystem.Model.SportEvent", b =>
                {
                    b.Navigation("Bets");

                    b.Navigation("EventResults");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("BetSystem.Model.Team", b =>
                {
                    b.Navigation("Results");
                });
#pragma warning restore 612, 618
        }
    }
}

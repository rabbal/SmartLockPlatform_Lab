﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLockPlatform.Infrastructure.Context;

#nullable disable

namespace SmartLockPlatform.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230602190518_V2023_06_02_2234")]
    partial class V2023_06_02_2234
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("Lock_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("LockRight_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("Member_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("MemberGroup_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("Role_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("RolePermission_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("Site_HiLo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("User_HiLo")
                .IncrementsBy(10);

            modelBuilder.Entity("Membership", b =>
                {
                    b.Property<long>("MemberGroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("MembersId")
                        .HasColumnType("bigint");

                    b.HasKey("MemberGroupId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("Membership", "customers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FriendlyName")
                        .HasColumnType("text");

                    b.Property<string>("Xml")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Identity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "User_HiLo");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FailedLoginCount")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStampToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.ToTable("User", "customers");

                    b.HasData(
                        new
                        {
                            Id = -1L,
                            FailedLoginCount = 0,
                            IsActive = true,
                            IsAdmin = true,
                            LockoutEnabled = true,
                            PasswordHash = "AQAAAAEAACcQAAAAEGXzevuZegI9XRHQNl7MRxQR/3b7SzCDoI6/ZDsMoAq/7iH+vsOiDnaW8TnqUvUtDQ==",
                            SecurityStampToken = "381049F7-A730-49A8-A196-5F5BACB4F4DB"
                        });
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Lock", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "Lock_HiLo");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("SiteId")
                        .HasColumnType("bigint");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasIndex("Uuid")
                        .IsUnique()
                        .HasDatabaseName("UIX_Lock_Uuid");

                    b.ToTable("Lock", "customers");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.LockRight", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "LockRight_HiLo");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<long?>("LockId")
                        .HasColumnType("bigint");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("LockId");

                    b.ToTable("LockRight", "customers");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "Member_HiLo");

                    b.Property<string>("Alias")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("BlockedReason")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("SiteId")
                        .HasColumnType("bigint");

                    b.Property<bool>("TotpEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("TotpToken")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.HasIndex("UserId");

                    b.ToTable("Member", "customers");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.MemberGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "MemberGroup_HiLo");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("SiteId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("MemberGroup", "customers");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "Role_HiLo");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("SiteId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("SiteId", "Name")
                        .IsUnique()
                        .HasDatabaseName("UIX_Role_SiteId_Name");

                    b.ToTable("Role", "customers");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Site", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "Site_HiLo");

                    b.Property<string>("CreatedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedByIP")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long?>("ModifiedByUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("UIX_Site_Name");

                    b.HasIndex("OwnerId");

                    b.ToTable("Site", "customers");
                });

            modelBuilder.Entity("Membership", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Sites.MemberGroup", null)
                        .WithMany()
                        .HasForeignKey("MemberGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLockPlatform.Domain.Sites.Member", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Identity.User", b =>
                {
                    b.OwnsOne("SmartLockPlatform.Domain.Identity.Email", "Email", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("character varying(128)")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("UIX_User_Email");

                            b1.ToTable("User", "customers");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.HasData(
                                new
                                {
                                    UserId = -1L,
                                    Value = "admin@example.com"
                                });
                        });

                    b.OwnsOne("SmartLockPlatform.Domain.Identity.FirstName", "FirstName", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("FirstName");

                            b1.HasKey("UserId");

                            b1.ToTable("User", "customers");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.HasData(
                                new
                                {
                                    UserId = -1L,
                                    Value = "super"
                                });
                        });

                    b.OwnsOne("SmartLockPlatform.Domain.Identity.LastName", "LastName", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("LastName");

                            b1.HasKey("UserId");

                            b1.ToTable("User", "customers");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.HasData(
                                new
                                {
                                    UserId = -1L,
                                    Value = "admin"
                                });
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("FirstName")
                        .IsRequired();

                    b.Navigation("LastName")
                        .IsRequired();
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Lock", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Sites.Site", null)
                        .WithMany("Locks")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.LockRight", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Sites.MemberGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartLockPlatform.Domain.Sites.Lock", null)
                        .WithMany("Rights")
                        .HasForeignKey("LockId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("SmartLockPlatform.Domain.Sites.Timeframe", "Timeframe", b1 =>
                        {
                            b1.Property<long>("LockRightId")
                                .HasColumnType("bigint");

                            b1.HasKey("LockRightId");

                            b1.ToTable("LockRight", "customers");

                            b1.WithOwner()
                                .HasForeignKey("LockRightId");

                            b1.OwnsOne("SmartLockPlatform.Domain.Sites.Timeframe+TimeframeDate", "Date", b2 =>
                                {
                                    b2.Property<long>("TimeframeLockRightId")
                                        .HasColumnType("bigint");

                                    b2.Property<DateOnly>("From")
                                        .HasColumnType("date");

                                    b2.Property<DateOnly?>("To")
                                        .HasColumnType("date");

                                    b2.HasKey("TimeframeLockRightId");

                                    b2.ToTable("LockRight", "customers");

                                    b2.WithOwner()
                                        .HasForeignKey("TimeframeLockRightId");
                                });

                            b1.OwnsOne("SmartLockPlatform.Domain.Sites.Timeframe+TimeframeDays", "Days", b2 =>
                                {
                                    b2.Property<long>("TimeframeLockRightId")
                                        .HasColumnType("bigint");

                                    b2.Property<bool>("Friday")
                                        .HasColumnType("boolean");

                                    b2.Property<bool>("Monday")
                                        .HasColumnType("boolean");

                                    b2.Property<bool>("Saturday")
                                        .HasColumnType("boolean");

                                    b2.Property<bool>("Sunday")
                                        .HasColumnType("boolean");

                                    b2.Property<bool>("Thursday")
                                        .HasColumnType("boolean");

                                    b2.Property<bool>("Tuesday")
                                        .HasColumnType("boolean");

                                    b2.Property<bool>("Wednesday")
                                        .HasColumnType("boolean");

                                    b2.HasKey("TimeframeLockRightId");

                                    b2.ToTable("LockRight", "customers");

                                    b2.WithOwner()
                                        .HasForeignKey("TimeframeLockRightId");
                                });

                            b1.OwnsOne("SmartLockPlatform.Domain.Sites.Timeframe+TimeframeTime", "Time", b2 =>
                                {
                                    b2.Property<long>("TimeframeLockRightId")
                                        .HasColumnType("bigint");

                                    b2.Property<TimeOnly>("From")
                                        .HasColumnType("time without time zone");

                                    b2.Property<TimeOnly>("To")
                                        .HasColumnType("time without time zone");

                                    b2.HasKey("TimeframeLockRightId");

                                    b2.ToTable("LockRight", "customers");

                                    b2.WithOwner()
                                        .HasForeignKey("TimeframeLockRightId");
                                });

                            b1.Navigation("Date")
                                .IsRequired();

                            b1.Navigation("Days")
                                .IsRequired();

                            b1.Navigation("Time")
                                .IsRequired();
                        });

                    b.Navigation("Group");

                    b.Navigation("Timeframe")
                        .IsRequired();
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Member", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Sites.Site", null)
                        .WithMany("Members")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SmartLockPlatform.Domain.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.MemberGroup", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Sites.Site", null)
                        .WithMany("Groups")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Role", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Sites.Member", null)
                        .WithMany("Roles")
                        .HasForeignKey("MemberId");

                    b.HasOne("SmartLockPlatform.Domain.Sites.Site", null)
                        .WithMany("Roles")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsMany("SmartLockPlatform.Domain.Sites.RolePermission", "Permissions", b1 =>
                        {
                            b1.Property<long>("RoleId")
                                .HasColumnType("bigint");

                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            NpgsqlPropertyBuilderExtensions.UseHiLo(b1.Property<long>("Id"), "RolePermission_HiLo");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("RoleId", "Id");

                            b1.ToTable("RolePermission", "customers");

                            b1.WithOwner()
                                .HasForeignKey("RoleId");
                        });

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Site", b =>
                {
                    b.HasOne("SmartLockPlatform.Domain.Identity.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Lock", b =>
                {
                    b.Navigation("Rights");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Member", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("SmartLockPlatform.Domain.Sites.Site", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Locks");

                    b.Navigation("Members");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
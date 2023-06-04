using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartLockPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2023_06_02_2234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customers");

            migrationBuilder.CreateSequence(
                name: "Lock_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "LockRight_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Member_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "MemberGroup_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Role_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "RolePermission_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Site_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "User_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true),
                    Xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailedLoginCount = table.Column<int>(type: "integer", nullable: false),
                    SecurityStampToken = table.Column<string>(type: "text", nullable: false),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Site_User_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "customers",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lock",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Uuid = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SiteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lock_Site_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "customers",
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    BlockedReason = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Alias = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TotpEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TotpToken = table.Column<string>(type: "text", nullable: true),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SiteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_Site_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "customers",
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Member_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "customers",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberGroup",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SiteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberGroup_Site_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "customers",
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MemberId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SiteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "customers",
                        principalTable: "Member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Role_Site_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "customers",
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LockRight",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    Timeframe_Date_From = table.Column<DateOnly>(type: "date", nullable: false),
                    Timeframe_Date_To = table.Column<DateOnly>(type: "date", nullable: true),
                    Timeframe_Time_From = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Timeframe_Time_To = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Timeframe_Days_Sunday = table.Column<bool>(type: "boolean", nullable: false),
                    Timeframe_Days_Monday = table.Column<bool>(type: "boolean", nullable: false),
                    Timeframe_Days_Tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    Timeframe_Days_Wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    Timeframe_Days_Thursday = table.Column<bool>(type: "boolean", nullable: false),
                    Timeframe_Days_Friday = table.Column<bool>(type: "boolean", nullable: false),
                    Timeframe_Days_Saturday = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedByIP = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LockRight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LockRight_Lock_LockId",
                        column: x => x.LockId,
                        principalSchema: "customers",
                        principalTable: "Lock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LockRight_MemberGroup_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "customers",
                        principalTable: "MemberGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                schema: "customers",
                columns: table => new
                {
                    MemberGroupId = table.Column<long>(type: "bigint", nullable: false),
                    MembersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => new { x.MemberGroupId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_Membership_MemberGroup_MemberGroupId",
                        column: x => x.MemberGroupId,
                        principalSchema: "customers",
                        principalTable: "MemberGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Membership_Member_MembersId",
                        column: x => x.MembersId,
                        principalSchema: "customers",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.Id });
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "customers",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "customers",
                table: "User",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "CreatedByIP", "CreatedByUserId", "CreatedDateTime", "FailedLoginCount", "IsActive", "IsAdmin", "LockoutEnabled", "LockoutEnd", "ModifiedByIP", "ModifiedByUserId", "ModifiedDateTime", "PasswordHash", "SecurityStampToken" },
                values: new object[] { -1L, "admin@example.com", "super", "admin", null, null, null, 0, true, true, true, null, null, null, null, "AQAAAAEAACcQAAAAEGXzevuZegI9XRHQNl7MRxQR/3b7SzCDoI6/ZDsMoAq/7iH+vsOiDnaW8TnqUvUtDQ==", "381049F7-A730-49A8-A196-5F5BACB4F4DB" });

            migrationBuilder.CreateIndex(
                name: "IX_Lock_SiteId",
                schema: "customers",
                table: "Lock",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "UIX_Lock_Uuid",
                schema: "customers",
                table: "Lock",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LockRight_GroupId",
                schema: "customers",
                table: "LockRight",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LockRight_LockId",
                schema: "customers",
                table: "LockRight",
                column: "LockId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_SiteId",
                schema: "customers",
                table: "Member",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_UserId",
                schema: "customers",
                table: "Member",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGroup_SiteId",
                schema: "customers",
                table: "MemberGroup",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_MembersId",
                schema: "customers",
                table: "Membership",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_MemberId",
                schema: "customers",
                table: "Role",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "UIX_Role_SiteId_Name",
                schema: "customers",
                table: "Role",
                columns: new[] { "SiteId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Site_OwnerId",
                schema: "customers",
                table: "Site",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "UIX_Site_Name",
                schema: "customers",
                table: "Site",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UIX_User_Email",
                schema: "customers",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "LockRight",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Membership",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Lock",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "MemberGroup",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Member",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "Site",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "User",
                schema: "customers");

            migrationBuilder.DropSequence(
                name: "Lock_HiLo");

            migrationBuilder.DropSequence(
                name: "LockRight_HiLo");

            migrationBuilder.DropSequence(
                name: "Member_HiLo");

            migrationBuilder.DropSequence(
                name: "MemberGroup_HiLo");

            migrationBuilder.DropSequence(
                name: "Role_HiLo");

            migrationBuilder.DropSequence(
                name: "RolePermission_HiLo");

            migrationBuilder.DropSequence(
                name: "Site_HiLo");

            migrationBuilder.DropSequence(
                name: "User_HiLo");
        }
    }
}

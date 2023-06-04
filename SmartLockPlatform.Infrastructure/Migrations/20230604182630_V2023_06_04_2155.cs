using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLockPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2023_06_04_2155 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Member_MemberId",
                schema: "customers",
                table: "Role");

            migrationBuilder.DropTable(
                name: "Membership",
                schema: "customers");

            migrationBuilder.DropIndex(
                name: "IX_Role_MemberId",
                schema: "customers",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "MemberId",
                schema: "customers",
                table: "Role");

            migrationBuilder.CreateTable(
                name: "GroupMembership",
                schema: "customers",
                columns: table => new
                {
                    MemberGroupId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembership", x => new { x.MemberGroupId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_GroupMembership_MemberGroup_MemberGroupId",
                        column: x => x.MemberGroupId,
                        principalSchema: "customers",
                        principalTable: "MemberGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembership_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "customers",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleMembership",
                schema: "customers",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMembership", x => new { x.MemberId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RoleMembership_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "customers",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMembership_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "customers",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembership_MemberId",
                schema: "customers",
                table: "GroupMembership",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMembership_RoleId",
                schema: "customers",
                table: "RoleMembership",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMembership",
                schema: "customers");

            migrationBuilder.DropTable(
                name: "RoleMembership",
                schema: "customers");

            migrationBuilder.AddColumn<long>(
                name: "MemberId",
                schema: "customers",
                table: "Role",
                type: "bigint",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Role_MemberId",
                schema: "customers",
                table: "Role",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_MembersId",
                schema: "customers",
                table: "Membership",
                column: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Member_MemberId",
                schema: "customers",
                table: "Role",
                column: "MemberId",
                principalSchema: "customers",
                principalTable: "Member",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AuditTrailsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_AspNetUsers_UserId",
                table: "AuditTrails");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_CreatedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_ModifiedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatedById",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_ModifiedById",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_ModifiedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemCodes_AspNetUsers_CreatedById",
                table: "SystemCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemCodes_AspNetUsers_ModifiedById",
                table: "SystemCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_AspNetUsers_CreatedById",
                table: "SystemSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_AspNetUsers_ModifiedById",
                table: "SystemSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemTasks_AspNetUsers_CreatedById",
                table: "SystemTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemTasks_AspNetUsers_ModifiedById",
                table: "SystemTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemTasks_SystemTasks_ParentId",
                table: "SystemTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_CreatedById",
                table: "TicketResolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_ModifiedById",
                table: "TicketResolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_SystemCodeDetails_StatusId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketSubCategories_SubCategoryId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_CreatedById",
                table: "TicketSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_ModifiedById",
                table: "TicketSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSubCategories_TicketCategories_CategoryId",
                table: "TicketSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleProfiles_AspNetRoles_RoleId",
                table: "UserRoleProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_CreatedById",
                table: "UserRoleProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_ModifiedById",
                table: "UserRoleProfiles");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "AuditTrails",
                newName: "PrimaryKey");

            migrationBuilder.AddColumn<string>(
                name: "AffectedColumns",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewValues",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrails_AspNetUsers_UserId",
                table: "AuditTrails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_CreatedById",
                table: "Cities",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_ModifiedById",
                table: "Cities",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatedById",
                table: "Countries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_ModifiedById",
                table: "Countries",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_ModifiedById",
                table: "Departments",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemCodes_AspNetUsers_CreatedById",
                table: "SystemCodes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemCodes_AspNetUsers_ModifiedById",
                table: "SystemCodes",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_AspNetUsers_CreatedById",
                table: "SystemSettings",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_AspNetUsers_ModifiedById",
                table: "SystemSettings",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTasks_AspNetUsers_CreatedById",
                table: "SystemTasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTasks_AspNetUsers_ModifiedById",
                table: "SystemTasks",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTasks_SystemTasks_ParentId",
                table: "SystemTasks",
                column: "ParentId",
                principalTable: "SystemTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_CreatedById",
                table: "TicketResolutions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_ModifiedById",
                table: "TicketResolutions",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_SystemCodeDetails_StatusId",
                table: "Tickets",
                column: "StatusId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketSubCategories_SubCategoryId",
                table: "Tickets",
                column: "SubCategoryId",
                principalTable: "TicketSubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_CreatedById",
                table: "TicketSubCategories",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_ModifiedById",
                table: "TicketSubCategories",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSubCategories_TicketCategories_CategoryId",
                table: "TicketSubCategories",
                column: "CategoryId",
                principalTable: "TicketCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleProfiles_AspNetRoles_RoleId",
                table: "UserRoleProfiles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_CreatedById",
                table: "UserRoleProfiles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_ModifiedById",
                table: "UserRoleProfiles",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_AspNetUsers_UserId",
                table: "AuditTrails");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_CreatedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_ModifiedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatedById",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_ModifiedById",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_ModifiedById",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemCodes_AspNetUsers_CreatedById",
                table: "SystemCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemCodes_AspNetUsers_ModifiedById",
                table: "SystemCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_AspNetUsers_CreatedById",
                table: "SystemSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_AspNetUsers_ModifiedById",
                table: "SystemSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemTasks_AspNetUsers_CreatedById",
                table: "SystemTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemTasks_AspNetUsers_ModifiedById",
                table: "SystemTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemTasks_SystemTasks_ParentId",
                table: "SystemTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_CreatedById",
                table: "TicketResolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_ModifiedById",
                table: "TicketResolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_SystemCodeDetails_StatusId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketSubCategories_SubCategoryId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_CreatedById",
                table: "TicketSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_ModifiedById",
                table: "TicketSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSubCategories_TicketCategories_CategoryId",
                table: "TicketSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleProfiles_AspNetRoles_RoleId",
                table: "UserRoleProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_CreatedById",
                table: "UserRoleProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_ModifiedById",
                table: "UserRoleProfiles");

            migrationBuilder.DropColumn(
                name: "AffectedColumns",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "NewValues",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "PrimaryKey",
                table: "AuditTrails",
                newName: "IpAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrails_AspNetUsers_UserId",
                table: "AuditTrails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_CreatedById",
                table: "Cities",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_ModifiedById",
                table: "Cities",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatedById",
                table: "Countries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_ModifiedById",
                table: "Countries",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_CreatedById",
                table: "Departments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_ModifiedById",
                table: "Departments",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemCodes_AspNetUsers_CreatedById",
                table: "SystemCodes",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemCodes_AspNetUsers_ModifiedById",
                table: "SystemCodes",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_AspNetUsers_CreatedById",
                table: "SystemSettings",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_AspNetUsers_ModifiedById",
                table: "SystemSettings",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTasks_AspNetUsers_CreatedById",
                table: "SystemTasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTasks_AspNetUsers_ModifiedById",
                table: "SystemTasks",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemTasks_SystemTasks_ParentId",
                table: "SystemTasks",
                column: "ParentId",
                principalTable: "SystemTasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_CreatedById",
                table: "TicketResolutions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_ModifiedById",
                table: "TicketResolutions",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_SystemCodeDetails_StatusId",
                table: "Tickets",
                column: "StatusId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketSubCategories_SubCategoryId",
                table: "Tickets",
                column: "SubCategoryId",
                principalTable: "TicketSubCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_CreatedById",
                table: "TicketSubCategories",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSubCategories_AspNetUsers_ModifiedById",
                table: "TicketSubCategories",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSubCategories_TicketCategories_CategoryId",
                table: "TicketSubCategories",
                column: "CategoryId",
                principalTable: "TicketCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleProfiles_AspNetRoles_RoleId",
                table: "UserRoleProfiles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_CreatedById",
                table: "UserRoleProfiles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleProfiles_AspNetUsers_ModifiedById",
                table: "UserRoleProfiles",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

// ReSharper disable InconsistentNaming

namespace SmartLockPlatform.Host.Authorization;

public static class PermissionNames
{
    public static class Sites
    {
        public const string Prefix = "Sites:";
        public const string Register_Members = $"{Prefix}{nameof(Register_Members)}";
        public const string View_Members = $"{Prefix}{nameof(Register_Members)}";
        public const string Register_Lock = $"{Prefix}{nameof(Register_Lock)}";
        public const string Register_Role = $"{Prefix}{nameof(Register_Role)}";
        public const string Register_MemberGroup = $"{Prefix}{nameof(Register_MemberGroup)}";
        public const string Grant_RightToLock = $"{Prefix}{nameof(Grant_RightToLock)}";
        public const string View_Locks = $"{Prefix}{nameof(View_Locks)}";
        public const string View_Roles = $"{Prefix}{nameof(View_Roles)}";
        public const string View_MemberGroups = $"{Prefix}{nameof(View_MemberGroups)}";
        public const string View_MembersOfGroup = $"{Prefix}{nameof(View_MembersOfGroup)}";
        public const string Manipulate_RoleMembers = $"{Prefix}{nameof(Manipulate_RoleMembers)}";
        public const string Manipulate_GroupMembers = $"{Prefix}{nameof(Manipulate_GroupMembers)}";

        public const string View_Entries = $"{Prefix}{nameof(View_Entries)}";
        public const string View_Incidents = $"{Prefix}{nameof(View_Incidents)}";
    }
}
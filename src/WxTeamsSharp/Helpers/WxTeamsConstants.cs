namespace WxTeamsSharp.Helpers
{
    internal static class WxTeamsConstants
    {
#pragma warning disable S1075 // URIs should not be hardcoded (Default URL)

        public const string ApiBaseUrl = "https://api.ciscospark.com/v1/";
        public const string WebhooksUrl = "/webhooks";
        public const string TeamMembershipsUrl = "/team/memberships";
        public const string TeamsUrl = "/teams";
        public const string RoomsUrl = "/rooms";
        public const string RolesUrl = "/roles";
        public const string ResourceGroupsUrl = "/resourceGroups";
        public const string ResourceMembershipsUrl = "/resourceGroup/memberships";
        public const string LicensesUrl = "/licenses";
        public const string MembershipsUrl = "/memberships";
        public const string MessagesUrl = "/messages";
        public const string OrganizationsUrl = "/organizations";
        public const string PeopleUrl = "/people";
        public const string EventsUrl = "/events";

#pragma warning restore S1075 // URIs should not be hardcoded
    }
}

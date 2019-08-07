using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Models.Events;
using WxTeamsSharp.Models.Licenses;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Organizations;
using WxTeamsSharp.Models.People;
using WxTeamsSharp.Models.ResourceGroups;
using WxTeamsSharp.Models.Roles;
using WxTeamsSharp.Models.Rooms;
using WxTeamsSharp.Models.Teams;
using WxTeamsSharp.Models.Webhooks;

namespace WxTeamsSharp.Interfaces.Api
{
    /// <summary>
    /// Webex Teams API
    /// </summary>
    public interface IWxTeamsApi
    {
        /// <inheritdoc/>
        IWxTeamsClient TeamsClient { get; }

        /// <summary>
        /// Sets the access token. 
        /// Required before using the API.
        /// </summary>
        /// <param name="accessToken">The token used to access the API</param>
        /// <param name="url">Base URL for API. Has default. Don't change unless you're sure what you're doing</param>
        void Initialize(string accessToken, string url = WxTeamsConstants.ApiBaseUrl);

        /// <summary>
        /// List all of your webhooks.
        /// </summary>
        /// <param name="max">Limit the maximum number of webhooks in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Webhooks</returns>
        Task<IListResult<Webhook>> GetWebhooksAsync(int max = 100);

        /// <summary>
        /// Shows details for a webhook, by ID.
        /// </summary>
        /// <param name="webhookId">The unique identifier for the webhook.</param>
        /// <returns>This method returns a Webhook</returns>
        Task<Webhook> GetWebhookAsync(string webhookId);

        /// <summary>
        /// Creates a webhook.
        /// For me information: https://developer.webex.com/docs/api/guides/webhooks
        /// </summary>
        /// <param name="name">A user-friendly name for the webhook.</param>
        /// <param name="targetUrl">The URL that receives POST requests for each event.</param>
        /// <param name="resource">The resource type for the webhook. Creating a webhook requires 'read' scope on the resource the webhook is for.</param>
        /// <param name="eventType">The event type for the webhook.</param>
        /// <param name="filter">The filter that defines the webhook scope.</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <returns>This method returns the created Webhook</returns>
        Task<Webhook> CreateWebhookAsync(string name, string targetUrl, WebhookResource resource,
            EventType eventType, string filter = "", string secret = "");

        /// <summary>
        /// Deletes a webhook, by ID.
        /// </summary>
        /// <param name="webhookId">The unique identifier for the webhook.</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteWebhookAsync(string webhookId);

        /// <summary>
        /// Updates a webhook, by ID.
        /// </summary>
        /// <param name="webhookId">The unique identifier for the webhook.</param>
        /// <param name="name">A user-friendly name for the webhook.</param>
        /// <param name="targetUrl">The URL that receives POST requests for each event.</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <param name="status">The status of the webhook. Use active to reactivate a disabled webhook.</param>
        /// <returns>This method returns the updated Webhook</returns>
        Task<Webhook> UpdateWebhookAsync(string webhookId, string name, string targetUrl, string secret = "",
            WebhookStatus? status = null);

        /// <summary>
        /// Deletes a team membership, by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the team membership.</param>
        /// <returns>This method returns a ResponseMessage with should be "OK"</returns>
        Task<IResponseMessage> RemoveUserFromTeamAsync(string membershipId);

        /// <summary>
        /// Add someone to a team by Person ID or email address; optionally making them a moderator.
        /// </summary>
        /// <param name="teamId">The team ID.</param>
        /// <param name="userIdOrEmail">The user ID or email of the person</param>
        /// <param name="isModerator">Whether or not the participant is a team moderator.</param>
        /// <returns>This method returns the added user's Membership object</returns>
        Task<TeamMembership> AddUserToTeamAsync(string teamId, string userIdOrEmail, bool isModerator = false);

        /// <summary>
        /// Shows details for a team membership, by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the team membership.</param>
        /// <returns>This method returns a Membership</returns>
        Task<TeamMembership> GetTeamMembership(string membershipId);

        /// <summary>
        /// Updates a team membership, by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the team membership.</param>
        /// <param name="isModerator">Whether or not the participant is a team moderator.</param>
        /// <returns>This method returns the updated Membership</returns>
        Task<TeamMembership> UpdateTeamMembership(string membershipId, bool isModerator);

        /// <summary>
        /// Lists all team memberships for a given team, specified by the teamId query parameter.
        /// </summary>
        /// <param name="teamId">List memberships for a team, by ID.</param>
        /// <param name="max">Limit the maximum number of team memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Team Memberships</returns>
        Task<IListResult<TeamMembership>> GetTeamMembershipsAsync(string teamId, int max = 100);

        /// <summary>
        /// Lists teams to which the authenticated user belongs.
        /// </summary>
        /// <param name="max">Limit the maximum number of teams in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Teams</returns>
        Task<IListResult<Team>> GetTeamsAsync(int max = 100);

        /// <summary>
        /// Shows details for a team, by ID.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <returns>This method returns a Team</returns>
        Task<Team> GetTeamAsync(string teamId);

        /// <summary>
        /// Creates a team. The authenticated user is automatically added as a member of the team.
        /// </summary>
        /// <param name="name">A user-friendly name for the team.</param>
        /// <returns>This method returns the created Team</returns>
        Task<Team> CreateTeamAsync(string name);

        /// <summary>
        /// Updates details for a team, by ID.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <param name="name">A user-friendly name for the team.</param>
        /// <returns>This method returns an updated Team</returns>
        Task<Team> UpdateTeamAsync(string teamId, string name);

        /// <summary>
        /// Deletes a team, by ID.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <returns>This method returns a ResponseMessage that should be "OK"</returns>
        Task<IResponseMessage> DeleteTeamAsync(string teamId);

        /// <summary>
        /// Shows details for a room, by ID.
        /// The title of the room for 1:1 rooms will be the display name of the other person.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <returns>This method will return a Room</returns>
        Task<Room> GetRoomAsync(string roomId);

        /// <summary>
        /// Creates a room. The authenticated user is automatically added as a member of the room. 
        /// To create a 1:1 room, use MessageBuilder.New().SendToPersonId() or .SendToPersonEmail() to send a 
        /// message directly to another person.
        /// </summary>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <param name="teamId">The ID for the team with which this room is associated.</param>
        /// <returns>This method returns the created Room</returns>
        Task<Room> CreateRoomAsync(string title, string teamId = null);

        /// <summary>
        /// Deletes a room, by ID. Deleted rooms cannot be recovered.
        /// Deleting a room that is part of a team will archive the room instead.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteRoomAsync(string roomId);

        /// <summary>
        /// Updates details for a room, by ID.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <returns>This method returns the updated Room</returns>
        Task<Room> UpdateRoomAsync(string roomId, string title);

        /// <summary>
        /// List rooms.
        /// The title of the room for 1:1 rooms will be the display name of the other person.
        /// By default, lists rooms to which the authenticated user belongs.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="max">Limit the maximum number of rooms in the response.</param>
        /// <param name="teamId">List rooms associated with a team, by ID.</param>
        /// <param name="type">List rooms by type.</param>
        /// <param name="sortBy">Sort results.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Rooms</returns>
        Task<IListResult<Room>> GetRoomsAsync(int max = 100, string teamId = "",
            RoomType? type = null, SortBy? sortBy = null);

        /// <summary>
        /// Shows Webex meeting details for a room such as the SIP address, meeting URL, toll-free and toll dial-in numbers.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <returns>This method returns Meeting Details for a room</returns>
        Task<MeetingDetails> GetMeetingDetailsAsync(string roomId);

        /// <summary>
        /// List all roles.
        /// This roles resource can be accessed only by an admin.
        /// </summary>
        /// <returns>This method returns an IListResult object whose Items property is a list of Roles</returns>
        Task<IListResult<Role>> GetRolesAsync();

        /// <summary>
        /// Shows details for a role, by ID.
        /// This roles resource can be accessed only by an admin.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>This method returns a Role</returns>
        Task<Role> GetRoleAsync(string roleId);

        /// <summary>
        /// List resource groups.
        /// </summary>
        /// <param name="orgId">List resource groups in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Resource Groups</returns>
        Task<IListResult<ResourceGroup>> GetResourceGroupsAsync(string orgId = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceGroupId"></param>
        /// <returns>This method returns a Resource Group</returns>
        Task<ResourceGroup> GetResourceGroupAsync(string resourceGroupId);

        /// <summary>
        /// Lists all resource group memberships for an organization.
        /// </summary>
        /// <param name="max">Limit the maximum number of resource group memberships in the response.</param>
        /// <param name="licenseId">List resource group memberships for a license, by ID.</param>
        /// <param name="personId">List resource group memberships for a person, by ID.</param>
        /// <param name="personOrgId">List resource group memberships for an organization, by ID.</param>
        /// <param name="status">Limit resource group memberships to a specific status.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Resource Group Memberships</returns>
        Task<IListResult<ResourceGroupMembership>> GetResourceGroupMembershipsAsync(int max = 100, string licenseId = "", string personId = "",
            string personOrgId = "", ResourceMembershipStatus? status = null);

        /// <summary>
        /// Shows details for a resource group membership, by ID.
        /// </summary>
        /// <param name="resourceGroupMembershipId">The unique identifier for the resource group membership.</param>
        /// <returns>This method returns a Resource Group Membership</returns>
        Task<ResourceGroupMembership> GetResourceGroupMembershipAsync(string resourceGroupMembershipId);

        /// <summary>
        /// Updates a resource group membership, by ID.
        /// </summary>
        /// <param name="resourceGroupMembershipId">The unique identifier for the resource group membership.</param>
        /// <param name="resourceGroupId">The resource group ID.</param>
        /// <param name="licenseId">The license ID.</param>
        /// <param name="personId">The person ID.</param>
        /// <param name="personOrgId">The organization ID of the person.</param>
        /// <param name="status">The activation status of the resource group membership.</param>
        /// <returns>This method returns the updated Resource Group Membership</returns>
        Task<ResourceGroupMembership> UpdateResourceGroupMembershipAsync(string resourceGroupMembershipId,
            string resourceGroupId, string licenseId, string personId, string personOrgId, ResourceMembershipStatus status);

        /// <summary>
        /// Remove a person from the system. Only an admin can remove a person.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <returns>This method returns a response message which should be "OK"</returns>
        Task<IResponseMessage> DeleteUserAsync(string personId);

        /// <summary>
        /// Update details for a person, by ID
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="personParams">Get from PersonUpdater.Update(IPerson)</param>
        /// <returns>This method returns the updated Person object</returns>
        Task<Person> UpdateUserAsync(string personId, IUpdateablePerson personParams);

        /// <summary>
        /// Create a new user account for a given organization. Only an admin can create a new user account.
        /// Currently, users may have only one email address associated with their account.
        /// </summary>
        /// <param name="personParams">Get from PersonBuilder.New()</param>
        /// <returns>This method returns the created Person object</returns>
        Task<Person> CreateUserAsync(ICreateablePerson personParams);

        /// <summary>
        /// Show the profile for the authenticated user.
        /// </summary>
        /// <returns>This method returns the authenticated user's Person object</returns>
        Task<Person> GetMyUserDetails();

        /// <summary>
        /// Shows details for a person, by ID
        /// </summary>
        /// <param name="userId">A unique identifier for the person.</param>
        /// <returns>This method returns a Person</returns>
        Task<Person> GetUserAsync(string userId);

        /// <summary>
        /// List people in your organization with this email address
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="email">List people with this email address.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        Task<IListResult<Person>> GetUsersByEmailAsync(string email, int max = 100);

        /// <summary>
        /// List people in your organization with this display name
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="displayName">List people whose name starts with this string.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        Task<IListResult<Person>> GetUsersByDisplayNameAsync(string displayName, int max = 100);

        /// <summary>
        /// List people in your organization that match this list of IDs.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="ids">List people by ID. Accepts up to 85 person IDs separated by commas.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        Task<IListResult<Person>> GetUsersByIdListAsync(IEnumerable<string> ids, int max = 100);

        /// <summary>
        /// List people in this organization. Only admin users of another organization (such as partners) may use this.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="orgId">A unique identifier for the organization</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        Task<IListResult<Person>> GetUsersByOrgIdAsync(string orgId, int max = 100);

        /// <summary>
        /// List people in your organization. For most users, either the email or displayName parameter is required. 
        /// Admin users can omit these fields and list all users in their organization.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="email">List people with this email address. For non-admin requests, either this or displayName are required.</param>
        /// <param name="displayName">List people whose name starts with this string. For non-admin requests, either this or email are required.</param>
        /// <param name="id">List people by ID. Accepts up to 85 person IDs separated by commas.</param>
        /// <param name="orgId">List people in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        Task<IListResult<Person>> GetUsersAsync(string email = "", string displayName = "",
            IEnumerable<string> id = default, string orgId = "", int max = 100);

        /// <summary>
        /// List all organizations visible by your account. The results will not be paginated.
        /// </summary>
        /// <returns>This method returns an IListResult object whose Items property is a list of Organizations</returns>
        Task<IListResult<Organization>> GetOrganizationsAsync();

        /// <summary>
        /// Shows details for an organization, by ID.
        /// </summary>
        /// <param name="organizationId">The unique identifier for the organization.</param>
        /// <returns>This method returns an Organization</returns>
        Task<Organization> GetOrganizationAsync(string organizationId);

        /// <summary>
        /// Deletes a message, by message ID.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message.</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteMessageAsync(string messageId);

        /// <summary>
        /// Shows details for a message, by message ID.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message.</param>
        /// <returns>This method returns a Message</returns>
        Task<Message> GetMessageAsync(string messageId);

        /// <summary>
        /// Post a plain text or rich text message, and optionally, a file attachment attachment, to a room.
        /// </summary>
        /// <param name="sendMessage">Use MessageBuilder.New() to start building a message. .Build() will produce an ISendableMessage</param>
        /// <returns>This method returns the Message object that was sent.</returns>
        Task<Message> SendMessageAsync(ISendableMessage sendMessage);

        /// <summary>
        /// Lists all messages in a 1:1 (direct) room.
        /// The list sorts the messages in descending order by creation date.
        /// </summary>
        /// <param name="userIdOrEmail">User ID or Email to retreive messages for</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<Message>> GetDirectMessagesAsync(string userIdOrEmail);

        /// <summary>
        /// Lists all messages in a room before a specific date and time. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="roomId">List messages in a room, by ID.</param>
        /// <param name="before">List messages sent before a date and time.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<Message>> GetRoomMessagesBeforeDateAsync(string roomId, DateTimeOffset before, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = default);

        /// <summary>
        /// Lists all messages in a room before a specific message. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="roomId">List messages in a room, by ID.</param>
        /// <param name="messageId">List messages sent before a message, by ID.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<Message>> GetRoomMessagesBeforeMessageAsync(string roomId, string messageId, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = default);

        /// <summary>
        /// Lists all messages in a room. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="roomId">List messages in a room, by ID.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID.</param>
        /// <param name="before">List messages sent before a date and time.</param>
        /// <param name="beforeMessage">List messages sent before a message, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<Message>> GetRoomMessagesAsync(string roomId, int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default, DateTimeOffset before = default, string beforeMessage = "");

        /// <summary>
        /// Lists all room memberships.
        /// </summary>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of IMembership</returns>
        Task<IListResult<RoomMembership>> GetAllMembershipsAsync(int max = 100);

        /// <summary>
        /// Lists all room memberships associated with a particular user by userId or email
        /// </summary>
        /// <param name="userIdOrEmail">The Id or Email memberships should be associated with</param>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of IMembership</returns>
        Task<IListResult<RoomMembership>> GetMembershipsAssociatedWithAsync(string userIdOrEmail, int max = 100);

        /// <summary>
        /// Lists all room memberships associated with a specific room
        /// </summary>
        /// <param name="roomId">Id of the room to which memberships should be associated</param>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of IMembership</returns>
        Task<IListResult<RoomMembership>> GetRoomMembershipsAsync(string roomId, int max = 100);

        /// <summary>
        /// Add someone to a room by Person ID or email address; optionally making them a moderator.
        /// </summary>
        /// <param name="roomId">The room ID</param>
        /// <param name="userIdOrEmail">The ID or email address of the person</param>
        /// <param name="IsModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method returns the Membership that was created</returns>
        Task<RoomMembership> AddUserToRoomAsync(string roomId, string userIdOrEmail, bool IsModerator = false);

        /// <summary>
        /// Updates properties for a membership by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method returns the Membership that was updated</returns>
        Task<RoomMembership> UpdateMembershipAsync(string membershipId, bool isModerator);

        /// <summary>
        /// Get details for a membership by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <returns>This method returns a Membership</returns>
        Task<RoomMembership> GetMembershipAsync(string membershipId);

        /// <summary>
        /// Deletes a membership by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <returns>This method returns a response message which should be "OK"</returns>
        Task<IResponseMessage> DeleteMembershipAsync(string membershipId);

        /// <summary>
        /// List all licenses for a given organization. 
        /// If no orgId is specified, the default is the organization of the authenticated user.
        /// </summary>
        /// <returns>The method returns an IListResult object whose Items property is a list of Licenses</returns>
        Task<IListResult<License>> GetLicensesAsync();

        /// <summary>
        /// Shows details for a license, by ID.
        /// </summary>
        /// <param name="licenseId">The unique identifier for the license.</param>
        /// <returns>The method returns a License</returns>
        Task<License> GetLicenseAsync(string licenseId);

        /// <summary>
        /// List events in your organization. Several query parameters are available to filter the response.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="max">Limit the maximum number of events in the response.</param>
        /// <param name="resource">List events with a specific resource type.</param>
        /// <param name="type">List events with a specific event type.</param>
        /// <param name="actorId">List events performed by this person, by ID.</param>
        /// <param name="from">List events which occurred after a specific date and time.</param>
        /// <param name="to">List events which occurred before a specific date and time.</param>
        /// <returns>The method returns an IListResult object whose Items property is a list of Events</returns>
        Task<IListResult<Event>> GetEventsAsync(int max = 100, EventResource? resource = null, EventType? type = null,
            string actorId = "", DateTimeOffset from = default, DateTimeOffset to = default);

        /// <summary>
        /// Shows details for an event, by event ID.
        /// </summary>
        /// <param name="eventId">The unique identifier for the event.</param>
        /// <returns>This method returns an Event</returns>
        Task<Event> GetEventAsync(string eventId);
    }
}

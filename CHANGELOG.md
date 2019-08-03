# Changelog

## [1.1.0] - 08-03-2019
### Fixed
- Added Disabled status to WebhookStatus. 

### Changed
- New methods to create webhooks directly from a Room object
    - Room.AddMessageCreatedWebhookAsync()
	- Room.AddMessageDeletedWebhookAsync()
	- Room.AddUserAddedWebhookAsync()
	- Room.AddUserRemovedWebhookAsync()
- New overloads added for Room.AddUserAsync() and .RemoveUserAsync() to take an IPerson object as a parameter
- New WebhookData class for us in a webhook receiving api
- New Example project to showcase WebhookData and receiving webhook callbacks
- Added a method to Message class (not IMessage) to allow easy retreival of full message
since the webhook callback will not include the text for security reasons
    - Message.GetFullMessageAsync()
- Room, Membership, and Message classes are now public for use with WebhookData
- Breaking Change: The "Async" keyword was added to the end of People methods in the WxTeamsApi
    - GetUsersByEmailAsync
	- GetUsersByDisplayNameAsync
	- GetUsersByIdListAsync
	- GetUsersByOrgIdAsync

### Misc
- Documentation has been updated to reflect changes

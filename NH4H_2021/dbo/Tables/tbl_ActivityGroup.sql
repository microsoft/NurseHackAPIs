CREATE TABLE [dbo].[tbl_ActivityGroup]
(
	[ActivityGroupId] INT NOT NULL PRIMARY KEY, 
    [ActivityGroupName] VARCHAR(150) NOT NULL, 
    [ActivityGroupDesc] VARCHAR(250) NULL, 
    [ActivityGroupBadge] VARCHAR(150) NULL, 
    [ActivityCount] INT NOT NULL, 
    [UserRoleId] INT NOT NULL, 
    CONSTRAINT [FK_tbl_ActivityGroup_tbl_UserRole] FOREIGN KEY ([UserRoleId]) REFERENCES [tbl_UserRole]([UserRoleId])
)

CREATE TABLE [dbo].[tbl_Activity]
(
	[ActivityId] INT NOT NULL PRIMARY KEY, 
    [ActivityName] VARCHAR(150) NOT NULL, 
    [ActivityDesc] VARCHAR(250) NULL, 
    [ActivityActionLink] VARCHAR(250) NULL, 
    [ActivityPoints] INT NOT NULL, 
    [ActivityBadge] NVARCHAR(250) NULL, 
    [ActivityGroupId] INT NOT NULL, 
    [Manual]             BIT            NULL,
    CONSTRAINT [FK_tbl_Activity_tbl_ActivityGroup] FOREIGN KEY ([ActivityGroupId]) REFERENCES [tbl_ActivityGroup]([ActivityGroupId])
)

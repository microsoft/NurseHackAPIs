CREATE TABLE [dbo].[tbl_UserActivity]
(
	[UserId] INT NOT NULL , 
    [ActivityId] INT NOT NULL, 
    [ActivityPoints] INT NOT NULL,     
    PRIMARY KEY ([UserId], [ActivityId]), 
    CONSTRAINT [FK_tbl_UserActivity_tbl_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [tbl_Activity]([ActivityId]), 
    CONSTRAINT [FK_tbl_UserActivity_tbl_Users] FOREIGN KEY ([UserId]) REFERENCES [tbl_Users]([UserId])
)

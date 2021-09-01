CREATE TABLE [dbo].[tbl_TeamHackers] (
    [TeamId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [IsLead] INT NOT NULL,
    CONSTRAINT [FK_tbl_TeamHackers_tbl_Teams] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[tbl_Teams] ([TeamId]),
    CONSTRAINT [FK_tbl_TeamHackers_tbl_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tbl_Users] ([UserId])
);


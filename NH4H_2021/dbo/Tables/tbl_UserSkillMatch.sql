CREATE TABLE [dbo].[tbl_UserSkillMatch] (
    [UserId]  INT NULL,
    [SkillId] INT NOT NULL,
    CONSTRAINT [FK_tbl_UserSkillMatch_tbl_Skills] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[tbl_Skills] ([SkillId]),
    CONSTRAINT [FK_tbl_UserSkillMatch_tbl_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tbl_Users] ([UserId])
);


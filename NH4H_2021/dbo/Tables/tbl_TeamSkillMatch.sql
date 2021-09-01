CREATE TABLE [dbo].[tbl_TeamSkillMatch] (
    [TeamId]  INT NULL,
    [SkillId] INT NOT NULL,
    CONSTRAINT [FK_tbl_TeamSkillMatch_tbl_Skills] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[tbl_Skills] ([SkillId]),
    CONSTRAINT [FK_tbl_TeamSkillMatch_tbl_Teams] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[tbl_Teams] ([TeamId])
);


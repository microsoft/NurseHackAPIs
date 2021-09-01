CREATE TABLE [dbo].[tbl_Users] (
    [UserId]             INT             IDENTITY (101, 1) NOT NULL,
    [UserRoleId]           INT   NULL,
    [UserRole]           NVARCHAR (50)   NULL,
    [UserRegEmail]       NVARCHAR (50)   NOT NULL,
    [UserMSTeamsEmail]   NVARCHAR (50)   NOT NULL,
    [UserDisplayName]    NVARCHAR (50)   NULL,
    [UserTimeCommitment] NVARCHAR (50)   NULL,
    [MySkills]           NVARCHAR (1000) NULL,
    [ADUserId]           NVARCHAR (50)   NULL,
    [UserOptOut]         BIT             CONSTRAINT [DF__tbl_Users__UserO__0B91BA14] DEFAULT ((0)) NULL,
    [MSFTOptIn]          BIT             NULL DEFAULT 0,
    [JNJOptIn]           BIT             NULL DEFAULT 0,
    [SONSIELOptIn]       BIT             NULL DEFAULT 0,
    [Active]             BIT             CONSTRAINT [DF_tbl_Users_Active] DEFAULT ((0)) NOT NULL,
    [MailchimpId]        NVARCHAR(100)    NULL,
    [CreatedDate]        DATETIME2 (7)   CONSTRAINT [DF_tbl_Users_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          NVARCHAR (50)   NULL,
    [ModifiedDate]       DATETIME2 (7)   NULL,
    [ModifiedBy]         NVARCHAR (50)   NULL,
    CONSTRAINT [PK_tbl_Users] PRIMARY KEY CLUSTERED ([UserId] ASC), 
    CONSTRAINT [FK_tbl_Users_UserRole] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[tbl_UserRole]([UserRoleId]), 
    CONSTRAINT [CK_tbl_Users_UserRegEmail] UNIQUE(UserRegEmail)
);


GO

--CREATE FULLTEXT INDEX ON [dbo].[tbl_Users] ([UserRegEmail]) KEY INDEX [PK_tbl_Users] ON [NH4H_FULLTEXT] 
--GO


CREATE TABLE [dbo].[tbl_RegLink]
(
	[RegLinkId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UniqueCode] UNIQUEIDENTIFIER NOT NULL,     
    [IntendedEmail] NVARCHAR(150) NULL, 
    [UsedByEmail] NVARCHAR(150) NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0, 
    [UserRole] NVARCHAR(50) NULL
)

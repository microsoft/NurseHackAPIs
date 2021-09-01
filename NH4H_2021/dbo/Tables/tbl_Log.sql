CREATE TABLE [dbo].[tbl_Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](100) NOT NULL,
	[ErrorCode] [nchar](5) NULL,
	[Label] [nchar](30) NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


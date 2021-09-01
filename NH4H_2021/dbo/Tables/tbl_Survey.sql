CREATE TABLE [dbo].[tbl_Survey]
(
	[SurveyId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Pronouns] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[RaceEthnicity] [varchar](50) NULL,
	[Company] [varchar](150) NULL,
	[Expertise] [varchar](100) NULL,
	[Student] [bit] NULL,
	CONSTRAINT [PK_Survey] PRIMARY KEY CLUSTERED (	[SurveyId] ASC )
) 

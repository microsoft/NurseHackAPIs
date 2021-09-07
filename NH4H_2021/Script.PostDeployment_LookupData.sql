/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
-- UserRole Lookup Data
IF (NOT EXISTS(SELECT * FROM [dbo].[tbl_UserRole]))
BEGIN
INSERT INTO [dbo].[tbl_UserRole] ([UserRoleId],[UserRoleName]) VALUES(1, 'Hacker')
INSERT INTO [dbo].[tbl_UserRole] ([UserRoleId],[UserRoleName]) VALUES(2, 'Mentor')
INSERT INTO [dbo].[tbl_UserRole] ([UserRoleId],[UserRoleName]) VALUES(3, 'Panelist')
INSERT INTO [dbo].[tbl_UserRole] ([UserRoleId],[UserRoleName]) VALUES(4, 'Organizer')
END


/* Not required for this interation -- using alternate gamification system
INSERT INTO [dbo].[tbl_ActivityGroup] ([ActivityGroupId],[ActivityGroupName],[ActivityGroupDesc],[ActivityGroupBadge],[ActivityCount],[UserRoleId]) VALUES (1, 'Training', 'Participation in Training activites inlcuding but not limitied to Live Events and Technical Skills Challenge.', NULL, 5, 1)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(1, 'Teams 101', 'Intro to Teams and NurseHack4Health Teams Enviornment', NULL, 1, NULL, 1)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(2, 'Github 101', 'Intro to Github and how it will be used in NH4H.', NULL, 1, NULL, 1)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(3, 'Design Thinking', '', NULL, 1, NULL, 1)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(4, 'PowerApps/Automate', '', NULL, 1, NULL, 1)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(5, 'Power BI', '', NULL, 1, NULL, 1)



INSERT INTO [dbo].[tbl_ActivityGroup] ([ActivityGroupId],[ActivityGroupName],[ActivityGroupDesc],[ActivityGroupBadge],[ActivityCount],[UserRoleId]) VALUES (2, 'Training', 'Participation in Training activites inlcuding but not limitied to Live Events and Technical Skills Challenge.', NULL, 2, 2)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(1, 'Teams 101', 'Intro to Teams and NurseHack4Health Teams Enviornment', NULL, 1, NULL, 2)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(6, 'Mentor Training', '', NULL, 1, NULL, 2)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(7, 'Pitch Coach Training', '', NULL, 1, NULL, 2)

INSERT INTO [dbo].[tbl_ActivityGroup] ([ActivityGroupId],[ActivityGroupName],[ActivityGroupDesc],[ActivityGroupBadge],[ActivityCount],[UserRoleId]) VALUES (3, 'Training', 'Participation in Training activites inlcuding but not limitied to Live Events and Technical Skills Challenge.', NULL, 1, 3)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(8, 'Panelist Training', '', NULL, 1, NULL, 3)


INSERT INTO [dbo].[tbl_ActivityGroup] ([ActivityGroupId],[ActivityGroupName],[ActivityGroupDesc],[ActivityGroupBadge],[ActivityCount],[UserRoleId]) VALUES (4, 'Teams Participation', 'Activities related to MS Teams and Team Builder', NULL, 5, 1)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(9, 'Logged into Teams', '', NULL, 1, NULL, 4)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(10, 'Introduced Yourself', '', NULL, 1, NULL, 4)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(11, 'Completed User Profile', '', NULL, 1, NULL, 4)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(12, 'Created a Hacking Team', '', NULL, 5, NULL, 4)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(13, 'Joined a Hacking Team', '', NULL, 2, NULL, 4)


INSERT INTO [dbo].[tbl_ActivityGroup] ([ActivityGroupId],[ActivityGroupName],[ActivityGroupDesc],[ActivityGroupBadge],[ActivityCount],[UserRoleId]) VALUES (5, 'Teams Participation', 'Activities related to MS Teams and Team Builder', NULL, 2, 2)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(11, 'Completed User Profile', '', NULL, 1, NULL, 5)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(14, 'Logged into Teams', '', NULL, 1, NULL, 5)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(15, 'Introduced Yourself', '', NULL, 1, NULL, 5)

INSERT INTO [dbo].[tbl_ActivityGroup] ([ActivityGroupId],[ActivityGroupName],[ActivityGroupDesc],[ActivityGroupBadge],[ActivityCount],[UserRoleId]) VALUES (6, 'Teams Participation', 'Activities related to MS Teams and Team Builder', NULL, 2, 3)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(16, 'Logged into Teams', '', NULL, 1, NULL, 6)
INSERT INTO [dbo].[tbl_Activity] ([ActivityId],[ActivityName],[ActivityDesc],[ActivityActionLink],[ActivityPoints],[ActivityBadge],[ActivityGroupId]) VALUES(17, 'Introduced Yourself', '', NULL, 1, NULL, 6)
*/
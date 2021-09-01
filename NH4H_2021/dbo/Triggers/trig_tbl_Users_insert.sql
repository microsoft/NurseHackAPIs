CREATE TRIGGER [dbo].[trig_tbl_Users_insert]
ON [dbo].[tbl_Users]
AFTER INSERT
AS 
BEGIN   
    SET NOCOUNT ON;

	DECLARE @UserRoleId AS int

	SELECT @UserRoleId = tbl_UserRole.UserRoleId FROM tbl_UserRole INNER JOIN inserted i on tbl_UserRole.UserRoleName = i.UserRole

    UPDATE [dbo].[tbl_Users] 
    SET  [UserRoleId] = @UserRoleId 
    FROM inserted i
    WHERE [dbo].[tbl_Users].UserId = i.UserId

END

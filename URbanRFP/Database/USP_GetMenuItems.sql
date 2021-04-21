


-- ====================================================================
-- Created date: Jun 23 2019
-- Author: 
-- ====================================================================
Create PROCEDURE USP_GetMenuItems
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	
	Select * from SystemMenu where IsActive=1

    END TRY
    BEGIN CATCH

        SELECT
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END



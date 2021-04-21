
use urbanrfp

-- ====================================================================
-- Created date: Jun 17, 2019
-- Author: 
-- ====================================================================
Create PROCEDURE USP_GetSearchRfpProducts
	@Search nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	
		select p.prd_key,p.prd_name,p.prd_type,p.prd_subtype,p.prd_description,p.prd_benefits, o.org_key, o.org_legal_name, p.prd_benefits from rfp_product p
		left join product_x_organization pxo on p.prd_key = pxo.pxo_prd_key
		left join co_organization o on o.org_key= pxo.pxo_org_key
		where p.prd_name like'%'+ @Search +'%' or p.prd_type like'%'+ @Search +'%' or p.prd_subtype like'%'+ @Search +'%' 
		or o.org_legal_name like'%'+ @Search +'%' or p.prd_key like'%'+ @Search 

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

/*
   **************** Parse By Comma : Retain Space *******************
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ParseByComma_RetainSpace] ( @String VARCHAR(8000) )  
RETURNS @TblSubString TABLE  
(  
varstring VARCHAR(255)  
)  
AS  
BEGIN  
DECLARE @intPos INT,  
@SubStr VARCHAR(100)  
-- Find The First Comma  
SET @IntPos = CHARINDEX(',', @String)  
-- Loop Until There Is Nothing Left Of @String  
WHILE @IntPos > 0  
BEGIN  
-- Extract The String  
SET @SubStr = SUBSTRING(@String, 0, @IntPos)  
-- Insert The String Into The Table  
INSERT INTO @TblSubString (varstring) VALUES (@SubStr)  
-- Remove The String & Comma Separator From The Original  
SET @String = SUBSTRING(@String, LEN(@SubStr) + 2, LEN(@String) - LEN(@SubStr) + 1)   
-- Get The New Index To The String  
SET @IntPos = CHARINDEX(',', @String)  
END  
-- Return The Last One  
INSERT INTO @TblSubString (varstring) VALUES (@String)  
RETURN  
END  
  
GO


/*
   **************** GetPickListIds *******************
*/


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- User Defined Function

CREATE function [dbo].[udf_GetPickListIds](    
 @picktype nvarchar(100) ,    
 @picklisttitle nvarchar(1000),
 @verticalid int
)  
returns @Return TABLE  
(    
  Sno INT IDENTITY (1,1),  
  Picklistid int  ,
  picklisttitle VARCHAR(50),
  picktypetitle VARCHAR(50)
)    
as    
begin    

 if @picklisttitle is not null    
  begin    
INSERT INTO @Return  
select  pl.PicklistId,
	pl.title,
	pt.type     
from PickList pl    
 inner join Picktype pt  on pt.picktypeid = pl.picktypeid   
  and pt.type = @picktype   
  and pl.Title IN (SELECT varstring FROM dbo.ParseByComma_RetainSpace(@picklisttitle))  
where    
 pl.inactive = 0   
 and pt.inactive = 0 
 and pl.verticalid in (@verticalid,-1)
  end    

 return     
end

GO

/*
   **************** GetPickListId *******************
   Note that this is not the same as GetPickListIds as this is a scalar function
*/

CREATE function [dbo].[udf_GetPickListId](  
 @picktype nvarchar(100),  
 @picklisttitle nvarchar(1000),
 @verticalid int  
)  
returns int  
as  
begin  
 declare @Return int  

 if @picklisttitle is not null  
  begin  
select @return = pl.PicklistId
from PickList pl  
	inner join Picktype pt  on pt.picktypeid = pl.picktypeid 
		and pt.type = @picktype 
		and pl.Title = @picklisttitle
where  
	pl.inactive = 0 
	and pt.inactive = 0
	and pl.verticalid in (@verticalid,-1)  
  end  

 return @return  
end

GO

/*
   **************** GetPickListTitle *******************
*/

CREATE  FUNCTION [dbo].[udf_GetPickListTitle](
	@picklistID INT,
	@verticalid int
)
RETURNS Varchar(500)
AS
BEGIN
	DECLARE @Return Varchar(50)

	IF @picklistID IS NOT NULL
		SELECT @return = pl.title
		From PickList pl
		Where pl.PicklistId = @PicklistId and pl.inactive = 0 and pl.verticalid in (@verticalid,-1)
	Else
		SELECT @return = pl.title
		From PickList pl
		Where pl.inactive = 0
		and pl.verticalid in (@verticalid,-1)

	RETURN ltrim(rtrim(@return))
END
GO
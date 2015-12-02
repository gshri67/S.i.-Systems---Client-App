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

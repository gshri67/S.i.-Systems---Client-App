/*
	**************************************Create Manage Direct Reports Update*******************************
*/


CREATE procedure [dbo].[sp_Timesheet_ManageDirectReports_update](  
     @agreementid int,  
     @olddirectreportuserid int,  
     @newdirectreportuserid int,  
     @updateuserid int,  
     @inactive int      
     )  
as  
set nocount on  
begin  
  
update Agreement_ContractAdminContactMatrix  
set DirectReportUserID=@newdirectreportuserid,  
 UpdateDate=getdate(),  
 UpdateUserID=@updateuserid  
where  
 AgreementID=@agreementid  
 and DirectReportUserID=@olddirectreportuserid  
 and inactive=0  
  
  
end  
       
  
GO

/*
	**************************************Create Activity Transaction Generic*******************************
	Note that this is called when a direct report is changed for a contract/timesheeet
*/

CREATE proc [dbo].[sp_SC_ActivityTransaction_Generic]    
 (    
 @internaluserid int,    
 @header varchar(255),    
 @note varchar(8000),    
 @activitytype int,    
 @complete bit,    
 @status int,    
 @AgreementID int,     
 @VerticalID int    
 )    
    
AS    
     
BEGIN    

 SET NOCOUNT ON ;   	
 INSERT INTO activitytransaction      
  (    
  activitytypeid,    
  complete,    
  createuserid,    
  statustype,    
  AgreementID,    
  VerticalID    
  )    
     
 VALUES    
  (    
  @activitytype,    
  @complete,    
  @internaluserid,    
  @status,    
  @AgreementID,    
  @VerticalID    
  )    
    
DECLARE @activitytransactionid int    
SET @activitytransactionid = scope_identity()    
    
 ---    
    
 INSERT INTO activitytransaction_note      
  (    
  activitytransactionid,    
  authoruserid,    
  transactionsubject,    
  note,    
  VerticalID    
  )    
     
 VALUES    
  (    
  @activitytransactionid,    
  @internaluserid,    
  @header,    
  @note,    
  @VerticalID    
  )    
     
    
END    
  
SELECT @activitytransactionid  
     
return  
    

GO



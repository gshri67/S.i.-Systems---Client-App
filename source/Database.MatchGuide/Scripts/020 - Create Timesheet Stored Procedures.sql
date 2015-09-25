

/*
	************************************************************ Timesheet Stored Procedures ******************************************************
*/

CREATE PROC [dbo].[sp_TimesheetTemp_Insert]                                
(                                                    
@aCandidateUserId INT,                                                              
@aContractID INT,                                
@aTSAvailablePeriodID INT,                              
@aQuickPay BIT,                              
@aTSID INT,    
@aTSTempID INT,  
@verticalId int      
)                                                                                   
                  
AS                                                                       
                  
SET NOCOUNT ON                                                                       
                  
BEGIN                                
                  
DECLARE @timesheetavailableperiodid INT                                
DECLARE @newTSTempID INT    
                  
SET @timesheetavailableperiodid = @aTSAvailablePeriodID                              
    
IF @aTSTempID IS NOT NULL    
BEGIN     
 IF EXISTS (     
    SELECT 1     
    FROM TimesheetTemp     
    WHERE TimesheetTemp.TimeSheetTempID = @aTSTempID     
    AND  TimesheetTemp.Inactive = 0    
    )    
 BEGIN     
  UPDATE TimesheetTemp    
  SET TimesheetTemp.inactive = 1    
  WHERE TimesheetTemp.TimesheetTempID = @aTSTempID    
      
  UPDATE TimesheetDetailTemp    
  SET TimesheetDetailTemp.inactive = 1    
  WHERE TimesheetDetailTemp.TimesheetTempID = @aTSTempID    
 END       
END    
    
IF @aTSTempID IS NULL    
    
DECLARE @pTSTempID INT    
    
BEGIN     
 IF EXISTS (     
    SELECT 1     
    FROM TimesheetTemp     
    WHERE TimesheetTemp.AgreementID = @aContractID     
    AND TimesheetTemp.TimeSheetAvailablePeriodID = @aTSAvailablePeriodID    
    AND  TimesheetTemp.Inactive = 0    
    )    
 BEGIN     
  SELECT @pTSTempID = TimesheetTemp.TimesheetTempID     
  FROM TimesheetTemp     
  WHERE TimesheetTemp.AgreementID = @aContractID     
  AND TimesheetTemp.TimeSheetAvailablePeriodID = @aTSAvailablePeriodID    
  AND  TimesheetTemp.Inactive = 0    
    
  UPDATE TimesheetTemp    
  SET inactive = 1    
  WHERE TimesheetTemp.TimesheetTempID = @pTSTempID     
  AND  TimesheetTemp.Inactive = 0    
      
  UPDATE TimesheetDetailTemp    
  SET inactive = 1    
  WHERE TimesheetDetailTemp.TimesheetTempID = @pTSTempID    
 END       
END    
    
    
 INSERT INTO TimeSheetTemp(                                
  createdate,      
  timesheetid,                                
  candidateuserid,        
  agreementid,                                
  quickpay,      
  timesheetavailableperiodid,  
  verticalId   
  )                                
 VALUES(                                
  getdate(),       
  @aTSID,                               
  @aCandidateUserId,                                
  @aContractId,      
  @aQuickPay,                                
  @timesheetavailableperiodid,  
  @verticalId         
 )                                 
      
SET @newTSTempID =  @@identity                  
SELECT @newTSTempID as TimesheetTempId                   
                  
END 
GO


CREATE proc [dbo].[sp_TimesheetTempDetail_Insert]              
(                                  
            
@aContractrateid int,                      
@aPoNumber VARCHAR(250),            
@aProjectId int,                                  
@acontractprojectpoid int,              
@aDay int,              
@aUnitValue  DECIMAL(6,2),              
@aGeneralProjPODesc nvarchar(1000),            
@aTimesheetTempId int            
)                                                                 
                                                                  
as                                                     
                                                  
set nocount on                                                     
              
begin            
            
if @aPoNumber is  null and  @aProjectId is null            
 begin            
            
  insert into TimeSheetDetailTemp             
  (            
   contractrateid,            
   [day],            
   unitvalue,    
   contractprojectpoid,            
   [description],            
   TimesheetTempId            
  )            
  values             
  (            
   @aContractrateid,            
   @aDay,            
   @aUnitValue,      
   0,          
   @aGeneralProjPODesc,            
   @aTimesheetTempId            
  )            
            
 end            
else            
 begin            
            
  insert into TimeSheetDetailTemp
  (            
   contractrateid,            
   ponumber,            
   projectid,            
   contractprojectpoid,            
   [day],            
   unitvalue,            
   TimesheetTempid            
  )            
  select              
   @aContractrateid,            
   @aPoNumber,            
   @aProjectId,            
   @acontractprojectpoid,            
   @aDay,            
   @aUnitValue,            
   @aTimesheetTempId  
 end            
end

GO

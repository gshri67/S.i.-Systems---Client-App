

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

/*
	**************************************Create Timesheet Update*******************************
*/

CREATE proc [dbo].[sp_Timesheet_Update]                  
(                                          
@aTimesheetId int,                  
@aCandidateUserId int,                                                    
@aContractID int,                      
@aTimesheetavailableperiodid  int,          
@aTSSubmittedName varchar(100),    
@verticalid int,  
@aTimesheetType varchar(100),  
@TSstatus varchar(100) 
                  
)                                                                         
                                                                          
as                                                             
                                                          
set nocount on                                                             
                
declare @newtimesheetid int                
  
begin                      
                  
 if @aTimesheetId is null                  
  begin                  
   insert into timesheet(                      
    createdate,                      
    candidateuserid,                      
    agreementid,                      
    timesheetavailableperiodid,                     
    Vacation,                   
    quickpay,                      
    statusid,                      
    resubmittedfromid,                      
    resubmittedtoid,                      
    approverrejectoruserid,                      
    batchid,                      
    submittedpdf,                      
    approvedpdf,                      
    rejectedpdf,    
    verticalid,  
 Timesheettype                 
    )                      
   values(                      
    getdate(),                      
    @aCandidateUserId,                      
    @aContractId,                      
    @aTimesheetavailableperiodid,                      
   1,                  
    0,
	624,--This is the pick list ID for Approved timesheets (Zero time is auto approved)
    null, 
    null, 
    @aCandidateUserId, 
    null,                 
    @aTSSubmittedName, 
    null, 
    null, 
 @verticalid,  
    851 --this is the pick list ID for ETimesheets
   )                       
                     
  end                  
 else                  
  begin                
                
 insert into timesheet                 
 (                
  CreateDate,                
  CandidateUserID,                
  AgreementID,                
  TimeSheetAvailablePeriodID,                
  QuickPay,                
  Vacation,                
  StatusID,                
  ResubmittedFromID,                
  ResubmittedToID,                
  ApproverRejectorUserID,                
  BatchID,                
  submittedpdf,                
  approvedpdf,                
  rejectedpdf,    
  verticalid,  
  Timesheettype                
 )                
 select                 
  CreateDate,                
  CandidateUserID,                
  AgreementID,                
  TimeSheetAvailablePeriodID,                
  QuickPay,                
  Vacation,                
   624,--This is the pick list ID for Approved timesheets (Zero time is auto approved)
  @aTimesheetId,                
  ResubmittedToID,                
  ApproverRejectorUserID,                
  BatchID,                
  submittedpdf,                
  approvedpdf,                
  rejectedpdf,    
  verticalid,  
  Timesheettype               
 from timesheet              
 where timesheetid = @aTimesheetId                
                 
 set @newtimesheetid  =  @@identity                
                
 update  timesheet                  
 set                 
  CreateDate = getdate(),      
  Vacation = 1,                 
  statusid = 624,--This is the pick list ID for Approved timesheets (Zero time is auto approved)
  ResubmittedFromID = @aTimesheetId,          
  submittedpdf = @aTSSubmittedName,      
  ApproverRejectorUserID = @aCandidateUserId,    
  verticalid = @verticalid                
 where timesheetid = @newtimesheetid                  
                 
 update  timesheet                  
 set                 
  ResubmittedToID = @newtimesheetid                
 where timesheetid = @aTimesheetId                  
                
  end                  
                  
end   

GO



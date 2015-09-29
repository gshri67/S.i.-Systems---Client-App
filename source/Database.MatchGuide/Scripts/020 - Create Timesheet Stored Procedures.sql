

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

/*
	**************************************Create Timesheet Insert*******************************
*/


create proc [dbo].[sp_Timesheet_Insert]                                      
(                                                          
@aCandidateUserId int,                                                                    
@aContractID int,                                      
@aTSType varchar(50),                                      
@aTSAvailablePeriodID int,                                    
@aQuickPay bit,                                    
@aSubmittedPdfName Varchar(100),                                  
@aTSID INT,                  
@aIsCPGSubmission BIT = 0,     
@verticalId int,              
@aTimesheetType varchar(100),
@aSubmittedBy INT = NULL,
@isSubmittedEmailSent BIT = 1,
@aDirectReportid INT = NULL
)                                                                                         
                        
as                                                                             
                        
set nocount on                                                                             
                        
begin                                      
                        
declare @timesheetavailableperiodid int                                      
declare @tsSubmittedStatusId int                                  
declare @tsDeclinedStatusId int                               
declare @tsCancelledStatusId int                           
declare @tsapprovedstatusid int     
declare @tsTypeId int
declare @validate_TsId int
             
declare @newTSID int             
DECLARE @TSTempID INT                           
                        
set @tsSubmittedStatusId = 622
set @tsDeclinedStatusId = 623
set @tsCancelledStatusId = 852
set @tsapprovedstatusid = 624
set @tsTypeId = 851
set @validate_TsId=null              
       
select 
	@validate_TsId=timesheetid
from timesheet 
where 
	timesheet.agreementid=@aContractID
	and timesheet.candidateuserid=@aCandidateUserId
	and timesheetavailableperiodid=@aTSAvailablePeriodID
	and timesheet.statusid=@tsSubmittedStatusId



	--Get the direct report from contract
	DECLARE @ContractDirectReportUserID AS  INT
	SELECT @ContractDirectReportUserID=DirectReportUserID FROM Agreement_ContractAdminContactMatrix WHERE agreementid = @aContractID and inactive = 0 


begin tran                                      
set @timesheetavailableperiodid = @aTSAvailablePeriodID                                    
                  
/*** insert into timesheet table this record inserts is through submission ***/                          
if @aTSID is null AND  @aIsCPGSubmission = 1                  
begin                                  
insert into timesheet(                                      
  createdate,                                      
  candidateuserid,                   
  agreementid,                                      
  timesheetavailableperiodid,                                      
  quickpay,                             
  statusid,                  
  approved_date,                  
  IsCPGSubmission,                
  SubmittedUserID,    
  verticalId,
  Timesheettype,
  isSubmittedEmailSent,
  DirectReportUserId              
  )                                      
 values(                                      
  getdate(),                                      
  @aCandidateUserId,                                      
  @aContractId,                                      
  @timesheetavailableperiodid,                                      
  @aQuickPay,                                       
  @tsapprovedstatusid,                  
  getdate(),                  
  1,                
  @aSubmittedBy,    
  @verticalId,
  @tsTypeId,
  @isSubmittedEmailSent,
  isnull(@aDirectReportid,@ContractDirectReportUserID)
 )                                       
set @newTSID =  @@identity                        
select @newTSID as TimesheetId                         
end                                  
/* end cpg submission insertions */                  
                  
ELSE if @aTSID is null 
begin 

	if @validate_TsId is not null
	begin
		select -1 as tsid
		rollback tran
		return
	end
	                                 
 /*** insert into timesheet table ***/                                      
 insert into timesheet(                                      
  createdate,                                      
  candidateuserid,                                      
  agreementid,                                      
  timesheetavailableperiodid,                                      
  quickpay,                                      
  statusid,                                      
  resubmittedfromid,                                      
  resubmittedtoid,                                      
  approverrejectoruserid,                                      
  batchid,                         
  submitted_date,                        
  submittedpdf,                                    
  approvedpdf,                                      
  rejectedpdf,    
  verticalId,
  Timesheettype,
  isSubmittedEmailSent,
  DirectReportUserId                                     
  )                                      
 values(                                      
  getdate(),                                      
  @aCandidateUserId,                                      
  @aContractId,                                      
  @timesheetavailableperiodid,                                      
  @aQuickPay,                                       
  @tsSubmittedStatusId, --statusid                                      
  null, --resubmittedfromid                                      
  null, --resubmittedtoid                                      
  null, --approverrejectoruserid                                      
  null, --batchid                                      
  getdate(),                        
 @aSubmittedPdfName,                                    
  null, --approvedpdf                                      
  null, --rejectedpdf,    
  @verticalId,
  @tsTypeId,
  @isSubmittedEmailSent ,
  @aDirectReportid                                     
 )                                       
                        
set @newTSID =  @@identity                        
select @newTSID as TimesheetId                         
            
  IF EXISTS (            
 SELECT 1             
 FROM TimesheetTemp            
 WHERE TimesheetTemp.AgreementId = @aContractId            
 AND TimesheetTemp.timesheetavailableperiodid = @timesheetavailableperiodid            
 AND TimesheetTemp.Inactive = 0            
  )                      
  BEGIN            
 SELECT @TSTempID = TimesheetTemp.TimesheetTempID             
 FROM TimesheetTemp            
 WHERE TimesheetTemp.AgreementId = @aContractId            
 AND TimesheetTemp.timesheetavailableperiodid = @timesheetavailableperiodid            
 AND TimesheetTemp.Inactive = 0            
               
   UPDATE TimesheetTemp            
   SET TimesheetTemp.TimesheetID = @newTSID,            
   inactive = 1            
   WHERE TimesheetTemp.TimesheetTempId = @TSTempID            
               
   UPDATE TimesheetDetailTemp            
   SET TimesheetDetailTemp.inactive = 1            
   WHERE TimesheetDetailTemp.TimesheetTempId = @TSTempID            
  END            
                      
end                                  
        
else                                  
begin                                  
                       
 --declare @TSStatus varchar(50)                            
 --select @TSStatus = dbo.udf_getpicklisttitle(timesheet.statusid,-1)                          
 --from timesheet                             
 --where timesheet.timesheetid = @aTSID                            
 DECLARE @TSSTatus varchar(50)
 DECLARE @PickListWorkAround INT
 select @TSSTatus = CASE (timesheet.statusid) 
	WHEN 621 THEN 'Open'
	WHEN 622 THEN 'Submitted'
	WHEN 623 THEN 'Rejected'
	ELSE 'Approved'
	END 
 from timesheet
 where timesheet.TimeSheetID = @aTSID
                        
 if ltrim(rtrim(@TSStatus)) = 'Rejected'  
 begin                            

	if @validate_TsId is not null
	begin
		select -1 as tsid
		rollback tran
		return
	end
   /*** insert into timesheet table ***/                                      
   insert into timesheet(                                      
    createdate,                                      
    candidateuserid,                                      
    agreementid,                                      
    timesheetavailableperiodid,                                      
    quickpay,                                      
    statusid,                                      
    resubmittedfromid,                                      
    resubmittedtoid,                                      
    approverrejectoruserid,                                      
    batchid,                                      
    submittedpdf,                                    
    approvedpdf,                                      
    rejectedpdf,                        
  submitted_date,    
  verticalId,
  TimesheetType,
  isSubmittedEmailSent,
  DirectReportUserId                                      
    )                                      
   values(                                      
    getdate(),                                      
    @aCandidateUserId,                                      
    @aContractId,                                      
    @timesheetavailableperiodid,                                      
    @aQuickPay,                                       
    @tsSubmittedStatusId, --statusid                                      
    @aTSID, --resubmittedfromid                                      
    null, --resubmittedtoid                                  
    null, --approverrejectoruserid                                      
    null, --batchid                                      
   @aSubmittedPdfName,                                    
    null, --approvedpdf                                      
    null, --rejectedpdf                                      
    getdate(),    
    @verticalId,
    @tsTypeId,
	@isSubmittedEmailSent                       ,
	@aDirectReportid
   )                                       
                        
set @newTSID =  @@identity                        
select @newTSID as TimesheetId                        
                        
   /*** update the old timesheet record with declined status ***/                                  
   update timesheet                                  
   set statusid = @tsDeclinedStatusId,                        
       resubmittedtoid = @newTSID                                  
   where timesheetid = @aTSID              
               
   /* updates temporary table */            
   IF EXISTS (            
  SELECT 1             
  FROM TimesheetTemp            
  WHERE TimesheetTemp.AgreementId = @aContractId            
  AND TimesheetTemp.timesheetavailableperiodid = @timesheetavailableperiodid            
  AND TimesheetTemp.Inactive = 0            
   )                      
   BEGIN            
  SELECT @TSTempID = TimesheetTemp.TimesheetTempID             
  FROM TimesheetTemp            
  WHERE TimesheetTemp.AgreementId = @aContractId            
  AND TimesheetTemp.timesheetavailableperiodid = @timesheetavailableperiodid            
  AND TimesheetTemp.Inactive = 0            
                
    UPDATE TimesheetTemp            
    SET TimesheetTemp.TimesheetID = @newTSID,            
    inactive = 1            
    WHERE TimesheetTemp.TimesheetTempId = @TSTempID            
                
    UPDATE TimesheetDetailTemp            
    SET TimesheetDetailTemp.inactive = 1            
    WHERE TimesheetDetailTemp.TimesheetTempId = @TSTempID            
   END                        
                    
 end         
 else 
 begin
	if @validate_TsId is not null
	begin
		select -1 as tsid
		rollback tran
		return
	end

                            
   /*** insert into timesheet table ***/                                      
   insert into timesheet(                                      
    createdate,                                      
    candidateuserid,                                      
    agreementid,                                      
    timesheetavailableperiodid,                                      
    quickpay,                                      
    statusid,                                      
    resubmittedfromid,             
    resubmittedtoid,                                      
    approverrejectoruserid,                                      
    batchid,                            
    submittedpdf,                                              
    approvedpdf,                                      
    rejectedpdf,                        
    submitted_date,    
    verticalId,
    Timesheettype,
	isSubmittedEmailSent,
	DirectReportUserId                         
    )                                      
   values(                     
    getdate(),                                      
    @aCandidateUserId,                                      
	@aContractId,                                      
    @timesheetavailableperiodid,                                      
    @aQuickPay,                                       
    @tsSubmittedStatusId, --statusid                                      
    @aTSID, --resubmittedfromid                                      
    null, --resubmittedtoid                                  
    null, --approverrejectoruserid                                      
    null, --batchid                                      
    @aSubmittedPdfName,                                    
    null, --approvedpdf                                      
    null, --rejectedpdf                                      
    getdate(),    
    @verticalId,
	@tsTypeId,
	@isSubmittedEmailSent,                       
	@aDirectReportid
   )                                       
                        
   set @newTSID =  @@identity                        
   select @newTSID as TimesheetId                        
                        
   /*** update the old timesheet record with cancelled status ***/                           
   update timesheet                                  
   set statusid = @tsCancelledStatusId,                        
       resubmittedtoid = @newTSID                      
   where timesheetid = @aTSID              
               
   /* updates temporary table */            
   IF EXISTS (            
  SELECT 1             
  FROM TimesheetTemp            
  WHERE TimesheetTemp.AgreementId = @aContractId            
  AND TimesheetTemp.timesheetavailableperiodid = @timesheetavailableperiodid            
  AND TimesheetTemp.Inactive = 0            
   )                      
   BEGIN            
  SELECT @TSTempID = TimesheetTemp.TimesheetTempID             
  FROM TimesheetTemp            
  WHERE TimesheetTemp.AgreementId = @aContractId            
  AND TimesheetTemp.timesheetavailableperiodid = @timesheetavailableperiodid            
  AND TimesheetTemp.Inactive = 0            
                
    UPDATE TimesheetTemp            
    SET TimesheetTemp.TimesheetID = @newTSID,            
    inactive = 1            
    WHERE TimesheetTemp.TimesheetTempId = @TSTempID            
                
    UPDATE TimesheetDetailTemp            
    SET TimesheetDetailTemp.inactive = 1            
    WHERE TimesheetDetailTemp.TimesheetTempId = @TSTempID            
   END                        
                    
 end                            
                 
end                                  

if @aTimesheetType='ETimesheet'                  
begin
	IF @aIsCPGSubmission = 0                  
		begin                         
		   update timesheet                                  
		   set submittedpdf = @aSubmittedPdfName+'_'+cast(@newTSID as varchar(100))+'_Submitted'                        
		   where timesheetid = @newTSID                        
		end 

	--Insert to New activity table (MG-10363)
	INSERT INTO TimeSheetActivity (TimeSheetID,[Action],CreatedBy,DirectReportID) 
	VALUES (@newTSID,'Submitted',@aCandidateUserId,isnull(@aDirectReportid,@ContractDirectReportUserID))                  --@tsSubmittedStatusId
end
                
   update agreement_contractdetail                      
   set  TimeSheetSubmitted = 1                      
   where  agreement_contractdetail.agreementid = @aContractId                                 
     
   select @aSubmittedPdfName+'_'+cast(@newTSID as varchar(100)) as tsid

commit tran                                      
                        
end 
PRINT 'Created Procedure: sp_Timesheet_Insert'

GO






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



/*
	**************************************Create TimesheetDetail Insert*******************************
*/


CREATE proc [dbo].[sp_TimesheetDetail_Insert]              
(                                  
            
@aContractrateid int,                      
@aPoNumber VARCHAR(250),            
@aProjectId int,                                  
@acontractprojectpoid int,              
@aDay int,              
@aUnitValue  DECIMAL(6,2),              
@aGeneralProjPODesc nvarchar(1000),            
@aTimesheetId int,           
@verticalId int ,
@InvoiceCodeId int
)                                                                 
                                                                  
as                                                     
                                                  
set nocount on                                                     
              
begin            
            
if @aPoNumber is  null and  @aProjectId is null            
 begin            
            
  insert into timesheetdetail             
  (            
   contractrateid,            
   [day],            
   unitvalue,    
   contractprojectpoid,            
   [description],            
   timesheetid,  
   verticalId ,
   InvoiceCodeId           
  )            
  values             
  (            
   @aContractrateid,            
   @aDay,            
   @aUnitValue,      
   0,          
   @aGeneralProjPODesc,            
   @aTimesheetId,  
   @verticalId ,
   @InvoiceCodeId            
  )            
            
 end            
else            
 begin            
            
  insert into timesheetdetail             
  (            
   contractrateid,            
   ponumber,            
   projectid,            
   contractprojectpoid,            
   [day],            
   unitvalue,            
   timesheetid,  
   verticalId ,
   InvoiceCodeId            
  )            
  select              
   @aContractrateid,            
   @aPoNumber,            
   @aProjectId,            
   @acontractprojectpoid,            
   @aDay,            
   @aUnitValue,            
   @aTimesheetId,  
   @verticalId ,
   @InvoiceCodeId                  
 end            
end 
GO

/*
	**************************************Create sp_Timesheet_GetBillingPeriods_ForViewETimesheets SP*******************************
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[sp_Timesheet_GetBillingPeriods_ForViewETimesheets]                
(                                                                            
@contractid int,                                            
@paymentplanid int,                                                                                
@DRUserID INT                                
)                                                                                                           
                                                                                                            
as                                                                                               
                                            
  set nocount on                                            
                                              
  DECLARE @PAMServerName VARCHAR(50), @PaymentplanTitle VARCHAR(50)                 
  DECLARE @timesheetavailableperiodid INT, @mintimesheetid INT, @curTSAPId INT, @agreeementsubid INT                
  DECLARE @CountofDeclined INT, @CountofCancelled INT, @CountofAppSub INT                                            
  DECLARE @contractstartdate DATETIME, @contractenddate DATETIME, @PAMAPDate DATETIME                            
  DECLARE @MonthlyFirstDay DATETIME, @monthlylastday DATETIME, @semimonthlyfirstday DATETIME, @semimonthlylastday DATETIME                              
               
                               
  SET @contractstartdate = ( SELECT a1.startdate FROM [Agreement] a1 WHERE a1.agreementid = @contractid)                                
  SET @contractenddate = ( SELECT a1.enddate FROM [Agreement] a1 WHERE a1.agreementid = @contractid)                            
  SET @agreeementsubid = ( SELECT a1.[AgreementSubID] FROM [Agreement] a1 WHERE a1.agreementid = @contractid)                            
  SET @PaymentplanTitle = (             
     SELECT p1.title                               
     FROM picklist                               
   INNER JOIN picklist p1 ON p1.picklistid = picklist.var6                              
     WHERE picklist.picklistid = @paymentplanid)                           
                               
                              
 SET @MonthlyFirstDay = (SELECT                               
   CONVERT(                         
     DATETIME,                        (                               
      CONVERT(VARCHAR(2),MONTH(a1.startdate))                               
      + '/'                               
   + STR(DAY(dbo.FirstDayOfMonth(a1.startdate)))                              
      + '/'                               
      + CONVERT(VARCHAR(4),YEAR(a1.startdate))                              
    )                              
                                          
     )                               
     FROM agreement a1                               
     WHERE a1.agreementid = @contractid)                              
                               
                               
 SET @monthlylastday = ( SELECT                               
   CONVERT(                              
     DATETIME,                              
     (                               
      CONVERT(VARCHAR(2),MONTH(a1.enddate))                               
      + '/'                               
      + STR(DAY(dbo.LastDayOfMonth(a1.enddate)))                              
      + '/'                               
      + CONVERT(VARCHAR(4),YEAR(a1.enddate))                              
    )                              
                                          
     )                               
     FROM agreement a1                               
     WHERE a1.agreementid = @contractid)                              
                                      
                                  
 SET @semimonthlyfirstday = (SELECT                               
   CONVERT(                              
DATETIME,                              
     (                               
      CONVERT(VARCHAR(2),MONTH(a1.startdate))                               
      + '/'                               
      + STR(16)                              
      + '/'                               
      + CONVERT(VARCHAR(4),YEAR(a1.startdate))                              
    )                              
                                          
     )                               
     FROM agreement a1                               
     WHERE a1.agreementid = @contractid)                              
                               
                               
 SET @semimonthlylastday = @monthlylastday                              
                    
/*** select available timeperiods for contract paymentplan from MG2 ***/                    
if object_id('tempdb..#tmp_availabletimeperiods') is not null                                            
  begin                                                                                                  
     drop table #tmp_availabletimeperiods                    
  end                                                                                                  
  create table #tmp_availabletimeperiods                                    
  (                                             
   [TimeSheetAvailablePeriodID] int default NULL,                    
   TimeSheetAvailablePeriodStartDate DATETIME,                    
   TimeSheetAvailablePeriodEndDate DATETIME                    
  )                                            
  INSERT INTO [#tmp_availabletimeperiods] (                    
  [TimeSheetAvailablePeriodID],                    
  TimeSheetAvailablePeriodStartDate,                    
  TimeSheetAvailablePeriodEndDate                    
  )                     
  SELECT DISTINCT                     
   [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodID],                    
   [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodStartDate],                    
   [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodEndDate]                     
  FROM [TimeSheetAvailablePeriod]                     
  WHERE [TimeSheetPaymentType] = @paymentplanid                    
                  
              
/*** select timesheetids here ***/                     
if object_id('tempdb..#tmp_TimesheetIds') is not null                                            
  begin                                                                                                  
     drop table #tmp_TimesheetIds                                            
  end                                                                                                  
  create table #tmp_TimesheetIds                                             
  (                                             
   timesheetid int default null                                            
  )                                            
                                              
/*** insert timesheetid's of Rejected status into #tmp_TimesheetIds table ***/                                            
  declare tsap_cursor cursor local for                                             
  select timesheet.timesheetavailableperiodid                                            
  from timesheet                                            
  where timesheet.agreementid = @contractid                                            
  and dbo.TimeSheet.inactive = 0                                
                   
  group by timesheet.agreementid, timesheet.timesheetavailableperiodid                    
                                             
  open tsap_cursor                                            
                                              
  fetch next from tsap_cursor                                            
  into @curTSAPId                                            
                                              
  while @@fetch_status = 0                                            
  begin                                            
                                
   select @CountofDeclined = count(timesheetid), @mintimesheetid = max(timesheetid)                                            
   from timesheet                                             
   inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                            
   inner join picklist on picklist.picklistid = timesheet.statusid                                           
   inner join agreement on agreement.agreementid = timesheet.agreementid                                            
   where timesheet.agreementid = @contractid                                 
   AND timesheet.inactive = 0                                           
   and timesheet.statusid = dbo.udf_GetPicklistId( 'Timesheetstatustype', 'Rejected',-1)                                            
   and timesheet.timesheetavailableperiodid = @curTSAPId                                
                                              
if @CountofDeclined > 0                                         
   begin                                            
    select @CountofAppSub = count(timesheet.timesheetid)                                            
    from timesheet                                       
    inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                            
    where                                             
    timesheetid  not in ( @mintimesheetid )                                
    AND dbo.TimeSheet.inactive=0                                             
    and timesheet.agreementid = @contractid                                            
    and timesheet.timesheetavailableperiodid = @curTSAPId                                            
    and timesheet.statusid in                                             
    (                                            
    select picklistid                                             
    from picklist                                            
    where                                             
    picklist.title in ( 'Approved', 'Submitted')                                             
    and picklist.picktypeid =                                             
     (                                             
     select picktypeid                                             
     from picktype                             
     where type = 'Timesheetstatustype'                                            
     )                                            
    )                                            
                                              
    if ( @CountofAppSub = 0 )                                            
    begin                                            
     insert into #tmp_TimesheetIds values ( @mintimesheetid )                                            
    end                                            
                                                
   end                                        
                                              
   fetch next from tsap_cursor                                            
   into @curTSAPId                                            
                                              
  end                                            
   close tsap_cursor                                            
   deallocate tsap_cursor                                            
                                              
/*** insert timesheetid's of cancelled status into #tmp_TimesheetIds table ***/                                    
  declare tsap_cursor cursor local for                                             
  select timesheet.timesheetavailableperiodid                                            
  from timesheet                                            
  where timesheet.agreementid = @contractid        
  AND timesheet.inactive = 0                                
               
  group by timesheet.agreementid, timesheet.timesheetavailableperiodid                                            
                                              
  open tsap_cursor                                            
                                              
  fetch next from tsap_cursor                                            
  into @curTSAPId                                            
                                       
  while @@fetch_status = 0                                       
  begin                                            
                                                 
   select @CountofCancelled = count(timesheetid), @mintimesheetid = max(timesheetid)                                            
 from timesheet                                             
   inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                            
   inner join picklist on picklist.picklistid = timesheet.statusid                                            
   inner join agreement on agreement.agreementid = timesheet.agreementid                                            
   where timesheet.agreementid = @contractid                                 
   AND timesheet.inactive = 0                                           
   and timesheet.statusid = dbo.udf_GetPicklistId( 'Timesheetstatustype', 'Cancelled',-1)                                            
   and timesheet.timesheetavailableperiodid = @curTSAPId                                            
                                              
   if @CountofCancelled > 0                                            
   begin                                            
    select @CountofAppSub = count(timesheet.timesheetid)                                            
    from timesheet                                             
    inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                            
    where                                             
    timesheetid  not in ( @mintimesheetid )                                
    AND timesheet.inactive = 0                                             
    and timesheet.agreementid = @contractid                                            
    and timesheet.timesheetavailableperiodid = @curTSAPId                                            
    and timesheet.statusid in                                             
    (                                            
    select picklistid                                             
    from picklist                                            
    where                                             
    picklist.title in ( 'Approved', 'Submitted','Accepted')                                             
    and picklist.picktypeid =                                             
     (                                             
     select picktypeid                                             
     from picktype                                             
   where type = 'Timesheetstatustype'                                            
     )                                            
    )                                            
                                              
    if ( @CountofAppSub = 0 )                                            
 begin                                           
     insert into #tmp_TimesheetIds values ( @mintimesheetid )                                            
    end                                            
                                                
   end                                            
                                              
   fetch next from tsap_cursor            
   into @curTSAPId                                            
            
  end                                            
   close tsap_cursor                                            
   deallocate tsap_cursor                                            
                         
                         
/*** insert final result into tmp_billingperdiods table ***/                                            
  if object_id('tempdb..#tmp_BillingPeriods') is not null                                            
  begin                                                                                                  
     drop table #tmp_BillingPeriods                                            
  end                                              
             
   create table #tmp_BillingPeriods                                      
  (                                             
   timesheetstatus varchar(50) default null,                                             
   timesheetavailableperiodstartdate datetime null,                                             
   timesheetavailableperiodenddate datetime null,                                            
   timesheetid int,                                           
   directreportname VARCHAR(100) DEFAULT null,                                           
   timesheetavailableperiodid int,                                            
   tsstartdate int,                                            
   tsenddate INT,                                
   inactive BIT                    
   --iscpgsubmission BIT                                
  )                                            
                                
  insert into #tmp_BillingPeriods                                 
  select                                             
   dbo.udf_GetPicklistTitle(timesheet.statusid,-1) timesheetstatus,                                             
   timesheetavailableperiod.timesheetavailableperiodstartdate,                                            
   timesheetavailableperiod.timesheetavailableperiodenddate,                                            
   timesheet.timesheetid,                                          
   u1.firstname +' '+  u1.lastname,                                          
   timesheetavailableperiod.TimeSheetAvailablePeriodID,                                            
   day(timesheetavailableperiod.timesheetavailableperiodstartdate) tsstartdate,                                            
   day(timesheetavailableperiod.timesheetavailableperiodenddate) tsenddate,                                
   timesheet.inactive                                
                                
                                   
  from timesheet                                            
   inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid =  timesheet.timesheetavailableperiodid                                            
   LEFT JOIN agreement ON agreement.agreementid = timesheet.agreementid                                          
   LEFT JOIN [Agreement_ContractAdminContactMatrix] ON [Agreement_ContractAdminContactMatrix].[AgreementID] = [Agreement].[AgreementID]                                          
   LEFT JOIN users u1 ON u1.userid = Agreement_ContractAdminContactMatrix.directreportuserid                                          
  where timesheet.timesheetid in ( select timesheetid from #tmp_TimesheetIds )                                 
  AND timesheet.inactive = 0                                
  and Agreement_ContractAdminContactMatrix.directreportuserid = @DRUserID                                   
                
                       
union                                             
 select                                              
   (                                            
    Case When statusid is null                                            
    then 'Open'                                            
    end                                    
   ) as timesheetstatus,                     
   timesheetavailableperiod.timesheetavailableperiodstartdate,                             
   timesheetavailableperiod.timesheetavailableperiodenddate,                                            
   timesheet.timesheetid,                                            
   u1.firstname +' '+  u1.lastname AS directreportname,                                             
   timesheetavailableperiod.TimeSheetAvailablePeriodID,                            
   day(timesheetavailableperiod.timesheetavailableperiodstartdate) tsstartdate,                                            
   day(timesheetavailableperiod.timesheetavailableperiodenddate) tsenddate,                                
   ISNULL(TimeSheet.inactive,0)                              
                                
from timesheetavailableperiod                                             
  LEFT JOIN timesheet ON timesheetavailableperiod.TimeSheetAvailablePeriodID = timesheet.TimeSheetAvailablePeriodID                                
 AND ((timesheet.agreementid = @contractid AND timesheet.inactive = 0) OR timesheet.agreementid IS NULL)                          
  left join picklist on picklist.picklistid = timesheet.statusid                                            
  LEFT JOIN agreement ON agreement.agreementid = timesheet.agreementid                                        
  LEFT JOIN [Agreement_ContractAdminContactMatrix] ON [Agreement_ContractAdminContactMatrix].[AgreementID] = [Agreement].[AgreementID]                                          
  LEFT JOIN users u1 ON u1.userid = Agreement_ContractAdminContactMatrix.directreportuserid                                          
                                                
  where                            
              
   timesheetavailableperiod.[TimeSheetAvailablePeriodStartDate] >=                             
  (                              
   CASE WHEN @PaymentplanTitle = 'Monthly'                              
   THEN @monthlyfirstday                              
   ELSE (                            
    CASE                             
    WHEN @contractstartdate >= timesheetavailableperiod.[TimeSheetAvailablePeriodStartDate] AND @contractstartdate < @semimonthlyfirstday                        
    THEN @monthlyfirstday                        
    ELSE @semimonthlyfirstday                            
    END                            
   )                            
   END                              
  )                                
 and [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodEndDate] <=                              
  (                              
   CASE WHEN @PaymentplanTitle = 'Monthly'                              
   THEN @monthlylastday                              
   ELSE @semimonthlylastday                            
   END                              
  )                              
  and                           
   (                          
 timesheet.TimeSheetAvailablePeriodID is null                            
 OR                          
 timesheet.[TimeSheetAvailablePeriodID] IS NOT NULL AND timesheet.[AgreementID] = @contractid AND timesheet.iscpgsubmission = 1                          
   )                             
  and timesheetavailableperiod.TimeSheetPaymentType = @paymentplanid                                                
                            
 if object_id('tempdb..#tmp_BillingTSIds') is not null                                            
  begin                                                                                                  
     drop table #tmp_BillingTSIds                                            
  end                                                                                                
                           
  create table #tmp_BillingTSIds ( BillingTSIds int)                                            
                                            
  declare @curTSID int                                            
  declare @curTSStatus varchar(50)                                            
  declare @curTSAvPID int                                            
                                              
  declare billing_cursor cursor for                                             
  select                                            
   timesheetid,                                            
   timesheetavailableperiodid,                                            
   timesheetstatus                                            
                                               
  from #tmp_BillingPeriods       
                                            
  open billing_cursor                                            
                                              
  fetch next from billing_cursor                                            
  into @curTSID, @curTSAvPID, @curTSStatus                                            
                                 
  while @@fetch_status = 0                                            
  begin                                            
   declare @cancount int                                            
   declare @rejcount int                                            
   declare @maxCanTSID int                                            
   declare @maxRejTSID int                                            
                       
   if @curTSStatus = 'Rejected'                                   
   begin                                            
                                                
    select @maxCanTSID = max(tBP.timesheetid), @cancount = count(tBP.timesheetid)                                              
    from #tmp_BillingPeriods tBP                                             
    where tBP.timesheetavailableperiodid = @curTSAvPID                                            
    and tBP.timesheetstatus = 'Cancelled'                                            
    if @cancount <> 0                                            
    begin                                            
     if @curTSID > @maxCanTSID and @maxCanTSID is not null                                            
     begin                                            
      insert into #tmp_BillingTSIds values ( @curTSID)                                              
     end                                            
    end                                            
    else if @cancount = 0                                            
    begin                                            
  insert into #tmp_BillingTSIds values ( @curTSID)                                            
    end                                            
   end                                            
   else if @curTSStatus = 'Cancelled'                                            
   begin                                            
    select @maxRejTSID = max(tBP.timesheetid), @rejcount = count(tBP.timesheetid)                                              
    from #tmp_BillingPeriods tBP                                             
    where tBP.timesheetavailableperiodid = @curTSAvPID                                      
    and tBP.timesheetstatus = 'Rejected'                                            
                                            
    if @rejcount <> 0                                            
    begin                                            
     if @curTSID > @maxRejTSID and @maxRejTSID is not null                                            
     begin                                            
      insert into #tmp_BillingTSIds values ( @curTSID)                                              
     end                                            
    end             
    else if @rejcount = 0                                            
    begin                                            
     insert into #tmp_BillingTSIds values ( @curTSID)                                                
    end                                            
   end                                         
                                            
  fetch next from billing_cursor                                            
  into @curTSID, @curTSAvPID, @curTSStatus                                            
                                            
  end                                             
                                            
  close billing_cursor                                            
  deallocate billing_cursor                                               
      
 if object_id('tempdb..#tmp_BillingPeriodsList') is not null                                            
  begin                                                                                                  
     drop table #tmp_BillingPeriodsList                                            
  end                                                                                      
                                              
  create table #tmp_BillingPeriodsList       
  (       
 timesheetstatus varchar(50),      
 TimesheetTempID int,      
    timesheetavailableperiodstartdate datetime,      
    timesheetavailableperiodenddate datetime,                                            
    timesheetid int,                                 
    directreportname varchar(100),                                          
    timesheetavailableperiodid int,                                            
    tsstartdate datetime,                                            
    tsenddate datetime,                                            
 billperiodsorder int,      
 pdfname varchar(100)      
  )                           
       
  insert into #tmp_BillingPeriodsList      
  select case                                             
    when timesheetstatus = 'Rejected' then 'Rejected/Open'                                             
    when timesheetstatus = 'Cancelled' then 'Cancelled/Open'                                            
    when (timesheetstatus = 'Approved' or timesheetstatus = 'Accepted')--AND iscpgsubmission = 1                                 
    THEN 'Open'                                
    else timesheetstatus                                          
    end                                             
   as timesheetstatus,           
   (          
   CASE          
 WHEN EXISTS           
   (SELECT 1           
    FROM TimesheetTemp           
    WHERE TimesheetTemp.TimesheetID = #tmp_BillingPeriods.TimesheetID           
    AND TimesheetTemp.Inactive = 0)          
   AND (#tmp_BillingPeriods.TimesheetStatus = 'Rejected' OR #tmp_BillingPeriods.TimesheetStatus = 'Cancelled')          
  THEN           
   (          
    SELECT TimesheetTemp.TimeSheetTempID          
    FROM TimesheetTemp           
    WHERE TimesheetTemp.TimesheetID = #tmp_BillingPeriods.TimesheetID           
    AND TimesheetTemp.Inactive = 0              
   ) /* Rejected/Cancelled Period entry in TimesheetTemp */          
 WHEN EXISTS          
   (SELECT 1           
    FROM TimesheetTemp           
    WHERE TimesheetTemp.AgreementID = @contractid           
    AND TimesheetTemp.timesheetavailableperiodid = #tmp_BillingPeriods.timesheetavailableperiodid          
    AND TimesheetTemp.Inactive = 0)          
   AND (#tmp_BillingPeriods.TimesheetStatus <> 'Rejected' AND #tmp_BillingPeriods.TimesheetStatus <> 'Cancelled')            
  THEN           
   (          
    SELECT TimesheetTemp.TimeSheetTempID          
    FROM TimesheetTemp           
    WHERE TimesheetTemp.AgreementID = @contractid           
    AND TimesheetTemp.timesheetavailableperiodid = #tmp_BillingPeriods.timesheetavailableperiodid          
    AND TimesheetTemp.Inactive = 0          
   ) /* Open Period entry in TimesheetTemp */          
 WHEN NOT EXISTS          
   (SELECT 1           
    FROM TimesheetTemp           
    WHERE TimesheetTemp.AgreementID = @contractid           
    AND TimesheetTemp.timesheetavailableperiodid = #tmp_BillingPeriods.timesheetavailableperiodid          
    AND TimesheetTemp.Inactive = 0)          
   AND (#tmp_BillingPeriods.TimesheetStatus <> 'Rejected'         
   AND #tmp_BillingPeriods.TimesheetStatus <> 'Cancelled'         
   AND #tmp_BillingPeriods.TimesheetStatus <> 'Approved'
   AND #tmp_BillingPeriods.TimesheetStatus <> 'Accepted'
	)            
  THEN 0          
 WHEN NOT EXISTS           
   (SELECT 1           
    FROM TimesheetTemp           
    WHERE TimesheetTemp.TimesheetID = #tmp_BillingPeriods.TimesheetID           
    AND TimesheetTemp.Inactive = 0)          
   AND (#tmp_BillingPeriods.TimesheetStatus = 'Rejected' OR #tmp_BillingPeriods.TimesheetStatus = 'Cancelled')          
  THEN 0          
 END           
            
 ) AS TimesheetTempID,           
             
   timesheetavailableperiodstartdate,                                             
   timesheetavailableperiodenddate,                                            
   timesheetid,                                 
   directreportname,                                          
   timesheetavailableperiodid,                                            
   tsstartdate,                                            
   tsenddate,                                            
   case                
    when timesheetstatus = 'Rejected' then 1                                             
    when timesheetstatus = 'Cancelled' then 2                            
    else 3                                            
    end                                             
   as billperiodsorder,                                          
   case                                             
    when timesheetstatus = 'Rejected'                                             
     then (                                             
       select rejectedpdf                                             
       from timesheet                                             
       where timesheet.timesheetid = #tmp_BillingPeriods.timesheetid                                            
      )                                            
    when timesheetstatus = 'Cancelled'                                             
     then (                                             
       select cancelledpdf                                  
       from timesheet                                             
       where timesheet.timesheetid = #tmp_BillingPeriods.timesheetid                                            
      )                                            
    else null                                            
    end                                             
   as pdfname                                            
  from #tmp_BillingPeriods                
  where                                     
    #tmp_BillingPeriods.timesheetid in ( select BillingTSIds from #tmp_BillingTSIds)                         
    OR #tmp_BillingPeriods.timesheetid IS NULL --OR #tmp_BillingPeriods.inactive = 1                                
 and #tmp_BillingPeriods.timesheetstatus not in ('Rejected','Cancelled','Approved','Moved','Batched','Accepted')      
                              

select * from #tmp_BillingPeriodsList      
            
  --order by billperiodsorder 
GO

/*
	**************************************Create GetCustomDate User Defined Function*******************************
*/

CREATE function [dbo].[udf_GetCustomDate](    
 @startdate datetime,    
 @enddate datetime    
)    
returns VARCHAR(30)    
as    
begin    
   
 declare @Return VARCHAR(30)   
   
 SET @Return = ''  
 SET @Return = LEFT(CONVERT( VARCHAR(10),DATENAME(mm,@startdate)), 3) + ' '
 SET @Return = @Return + CONVERT( VARCHAR(10), DATEPART( dd, @startdate)) + '-'  
 SET @Return = @Return + CONVERT( VARCHAR(10), DATEPART( dd, @enddate)) + ' '  
 SET @Return = @Return + CONVERT( VARCHAR(10),DATEPART(yy,@startdate ))  
    
 return @Return    
end    
GO

/*
	**************************************Create UspViewTSForCandidate_TSAPP SP*******************************
*/

CREATE proc [dbo].[UspViewTSForCandidate_TSAPP]                
(                                                                            
	@CandidateID INT      
)                                                                                                           
                                                                                                            
AS                                                                                               
                                            
SET NOCOUNT ON        
set transaction isolation level read uncommitted  
      
BEGIN      
      
 DECLARE @agreementtype INT,@timesheettype int      
 DECLARE @pAgreementID INT, @pDirectReportID INT, @pContractPaymentPlanType INT, @pContractID INT      
 DECLARE @pContractJobTitle VARCHAR(200), @pCustomtimeperiod VARCHAR(100), @pCompanyName Varchar(100)      
 SET @agreementtype = dbo.udf_GetpicklistID('AgreementType','Contract',-1)      
      
 select @timesheettype=picklistid  
 from dbo.udf_getpicklistids('TimesheetType','ETimesheet',-1)  
  
  
 /*** select timesheetids here ***/                     
 IF OBJECT_ID('TEMPDB..#TMP_FinalResultSet') IS NOT NULL                                            
 BEGIN                                                                                                  
  DROP TABLE #TMP_FinalResultSet                                            
 END                                                                                                  
 CREATE TABLE #TMP_FinalResultSet                                  
 (      
 
  TimesheetID INT,      
  timesheetStatus VARCHAR(50),      
  TimesheetTempID INT,      
  contractDesc VARCHAR(200),      
  ContractID INT,      
  payPeriod Varchar(200),      
  CompanyName Varchar(100),      
  Hours Float,      
  OrgHours Float,      
  isoverride BIT,      
  overridevalue INT,      
  iscpgsubmission BIT,      
  vacation BIT,      
  pdfQueryString Varchar(200),      
  submittedpdf Varchar(200),  
  timesheet_note varchar(8000),  
  timesheettype varchar(100),  
  SubmittedDate varchar(30) ,  
  ApprovedDate varchar(30) ,  
  RejectedDate varchar(30) ,  
  CancelledDate varchar(30),
  DisplayOrder bit                                       
 )                                            
        
 DECLARE Contract_Cursor CURSOR LOCAL FOR      
 SELECT  DISTINCT      
   agreement.agreementid,      
   u1.userid as DirectReportID,      
   agreement_contractdetail.ContractPaymentPlanType,      
   agreement_contractdetail.jobtitle,      
   agreement.agreementsubid,      
   company.companyname       
 FROM agreement      
   inner join agreement_contractdetail on agreement_contractdetail.agreementid = agreement.agreementid      
   left join picklist p1 on p1.picklistid = agreement.statustype      
   left join picklist p2 on p2.picklistid = agreement_contractdetail.TimeSheetType      
   left join Agreement_ContractAdminContactMatrix on Agreement_ContractAdminContactMatrix.agreementid = agreement.agreementid      
   and Agreement_ContractAdminContactMatrix.inactive = 0      
   inner join users u1 on u1.userid = Agreement_ContractAdminContactMatrix.DirectReportUserID      
   inner join company ON company.CompanyID = agreement.CompanyID      
 WHERE       
   agreement.agreementtype = @agreementtype      
   and agreement.agreementsubtype in ( select picklistid from dbo.udf_getpicklistids('ContractType','Consultant,Flo Thru,Contract To Hire',-1))      
   and agreement.candidateid = @CandidateID      
      
 OPEN Contract_Cursor       
      
    FETCH NEXT FROM Contract_Cursor                                            
    INTO @pAgreementID, @pDirectReportID, @pContractPaymentPlanType, @pContractJobTitle, @pContractID, @pCompanyName                                            
                                              
    WHILE @@FETCH_STATUS = 0                                            
    BEGIN       
  
  
   IF OBJECT_ID('TEMPDB..#tmp_BillingPeriodsList_nestedlevel1') IS NOT NULL                                            
 BEGIN                                                                    
  DROP TABLE #tmp_BillingPeriodsList_nestedlevel1                                            
 END                                                                                                  
 CREATE TABLE #tmp_BillingPeriodsList_nestedlevel1    
 (        
	timesheetstatus varchar(50),        
	TimesheetTempID int,        
    timesheetavailableperiodstartdate datetime,        
    timesheetavailableperiodenddate datetime,                                              
    timesheetid int,                                   
    directreportname varchar(100),                                            
    timesheetavailableperiodid int,                                              
    tsstartdate datetime,                                              
    tsenddate datetime,                                              
	billperiodsorder int,        
	pdfQueryString varchar(100) 
  )    
  
  insert into #tmp_BillingPeriodsList_nestedlevel1  
  EXEC dbo.sp_Timesheet_GetBillingPeriods_ForViewETimesheets @pAgreementID, @pContractPaymentPlanType, @pDirectReportID        
    
  
  
  INSERT INTO #TMP_FinalResultSet      
  
   SELECT       
    TimesheetID,      
    (CASE       
     WHEN TimesheetTempID = 0 Then 'Open'       
     ELSE 'Saved/Open'      
    END) timesheetStatus,       
    TimesheetTempID,      
    @pContractJobTitle,      
    @pContractID,      
    dbo.udf_GetCustomDate(timesheetavailableperiodstartdate,timesheetavailableperiodenddate),      
    @pCompanyName,      
    (CASE       
     WHEN #tmp_BillingPeriodsList_nestedlevel1.TimesheetTempID > 0 AND #tmp_BillingPeriodsList_nestedlevel1.TimesheetTempID IS NOT NULL      
     THEN(      
      SELECT       
        SUM(ISNULL(cast([TimeSheetDetailTemp].UnitValue as float),'0'))         
       FROM       
        [TimeSheetDetailTemp]        
       WHERE       
        [TimeSheetDetailTemp].[TimeSheetTempID]=#tmp_BillingPeriodsList_nestedlevel1.[TimeSheetTempID])      
     ELSE 0      
    END) Hours,      
    (CASE       
     WHEN TimesheetTempID > 0 AND TimesheetTempID IS NOT NULL      
     THEN(      
      SELECT       
        SUM(ISNULL(cast([TimeSheetDetailTemp].UnitValue as float),'0'))         
       FROM       
        [TimeSheetDetailTemp]        
       WHERE       
        [TimeSheetDetailTemp].[TimeSheetTempID]= #tmp_BillingPeriodsList_nestedlevel1.[TimeSheetTempID])      
     ELSE 0      
    END) OrgHours,      
    0 isoverride,      
    0 overridevalue,      
    0 iscpgsubmission,      
    0 vacation,      
    NULL pdfQueryString,      
    NULL submittedpdf,  
	null  as note,  
	null  as timesheettype,  
	null as SubmittedDate,  
    null as ApprovedDate,  
    null as RejectedDate,  
    null as CancelledDate,
    1 as DisplayOrder   
   FROM #tmp_BillingPeriodsList_nestedlevel1
         
      
  FETCH NEXT FROM Contract_Cursor            
  INTO @pAgreementID, @pDirectReportID, @pContractPaymentPlanType, @pContractJobTitle, @pContractID, @pCompanyName                                    
 END      
       
 CLOSE Contract_Cursor                                            
 DEALLOCATE Contract_Cursor       
  
  
--Resend Optimization
DECLARE @manualtimeid INT, @timesheet INT
SELECT @manualtimeid = picklistid FROM dbo.[udf_GetPickListIds]('TimeSheetType','Manual',-1)
SELECT @timesheet = picklistid FROM dbo.[udf_GetPickListIds]('TimeSheetType','ETimesheet',-1)

SELECT  
	DISTINCT       
	timesheet.timesheetid,      
	p1.title AS p1title,      
	0 AS TimesheetTempID,      
	Agreement_ContractDetail.jobtitle,      
	Agreement.Agreementsubid,      
	dbo.udf_getcustomdate(TimeSheetAvailablePeriod.TimeSheetAvailablePeriodStartDate,TimeSheetAvailablePeriod.TimeSheetAvailablePeriodEndDate) customtimeperiod,      
	company.companyname,      
	0 AS Hours  ,
	0 AS OrgHours, 
	timesheet.isoverride,      
	timesheet.overridevalue,      
	timesheet.iscpgsubmission,      
	timesheet.vacation,      
	CASE       
		WHEN (p1.title = 'Approved' or p1.title='Accepted') THEN timesheet.approvedpdf       
		WHEN p1.title = 'cancelled' THEN timesheet.cancelledpdf       
		WHEN p1.title = 'rejected' THEN timesheet.rejectedpdf       
		WHEN p1.title = 'submitted' THEN timesheet.submittedpdf      
		WHEN p1.title = 'moved' THEN timesheet.approvedpdf      
	END      
	AS pdfQueryString,      
	timesheet.submittedpdf,  
	timesheetnote.Comment,  
	p2.title ,  
	timesheet.submitted_date ,  
	timesheet.approved_date,  
	timesheet.rejected_date,  
	timesheet.cancelled_date,
	1 AS DisplayOrder  
	INTO #t
 FROM      
   timesheet      
   INNER JOIN Agreement_ContractDetail ON Agreement_ContractDetail.AgreementID = timesheet.AgreementID      
   INNER JOIN Agreement ON Agreement.agreementid = timesheet.AgreementID      
   INNER JOIN TimeSheetAvailablePeriod  ON TimeSheetAvailablePeriod.TimeSheetAvailablePeriodID = timesheet.TimeSheetAvailablePeriodID      
   INNER JOIN picklist p1 ON p1.picklistid = timesheet.StatusID         
   INNER JOIN users u1 ON u1.userid = timesheet.CandidateUserID      
   INNER JOIN company ON company.CompanyID = agreement.CompanyID      
   LEFT JOIN timesheetnote ON timesheetnote.timesheetid=timesheet.timesheetid  
   LEFT JOIN picklist p2 ON p2.picklistid=isnull(timesheet.Timesheettype,@timesheettype)  
   
  WHERE      
	  timesheet.candidateuserid = @CandidateID   
	  AND timesheet.timesheettype in (@manualtimeid , @timesheet)	--(850,851)	-- != (SELECT picklistid FROM dbo.[udf_GetPickListIds]('TimeSheetType','Client',-1))   
	  AND timesheet.inactive=0      
   

SELECT 
	t.timesheetid,t.iscpgsubmission,t.isoverride,
	CASE WHEN t.iscpgsubmission = 1 
	THEN SUM(ISNULL(CAST(tadmin.BulkHours AS FLOAT),'0')) 
	ELSE 
	(
		CASE 
			WHEN t.isoverride = 0  
			THEN  SUM(ISNULL(CAST(tdetail.UnitValue AS FLOAT),'0')) 
			ELSE t.overridevalue	
		END
	)
	END
	AS cpgHours,
	SUM(ISNULL(CAST(tdetail.[UnitValue]AS FLOAT),'0'))  AS orgHours
	INTO #t1
FROM 
	#t t
	left join TimeSheetAdminDetail tadmin ON tadmin.TimesheetID =  t.timesheetid	 
	left join TimeSheetDetail tdetail ON tdetail.timesheetid =  t.timesheetid
GROUP BY t.timesheetid, t.iscpgsubmission,t.isoverride,t.overridevalue


SELECT 
	DISTINCT 
	t.timesheetid,      
	t.p1title,      
	t.TimesheetTempID   ,
	t.jobtitle,      
	t.Agreementsubid,      
	t.customtimeperiod,      
	t.companyname,      
	t1.cpgHours,
	t1.orgHours, 
 	t.isoverride,      
	t.overridevalue,      
	t.iscpgsubmission,      
	t.vacation,      
	t.pdfQueryString,      
	t.submittedpdf,  
	t.Comment,  
	t.title ,  
	t.submitted_date ,  
	t.approved_date,  
	t.rejected_date,  
	t.cancelled_date,
	t.DisplayOrder
	INTO #t2 
FROM
	#t t
	inner join #t1 t1 ON t1.timesheetid = t.timesheetid
  --Resend Optimization Ends
  
       
  INSERT INTO #TMP_FinalResultSet    
    
    SELECT       
   timesheettemp.timesheettempid,        
   (Case when picklist.title='Cancelled' then 'Save/Cancel' else 'Saved' end ) as Title,    
   0,        
   Agreement_ContractDetail.jobtitle,        
   Agreement.Agreementsubid,        
   dbo.udf_getcustomdate(TimeSheetAvailablePeriod.TimeSheetAvailablePeriodStartDate,TimeSheetAvailablePeriod.TimeSheetAvailablePeriodEndDate) customtimeperiod,        
   company.companyname,        
   (        
   case         
    when timesheettemp.iscpgsubmission = 1        
    then (SELECT         
       SUM(ISNULL(cast([TimeSheetAdminDetail].BulkHours as float),'0'))           
      FROM         
       [TimeSheetAdminDetail]          
    WHERE         
       [TimeSheetAdminDetail].[TimesheetID]=[timesheettemp].[timesheettempid])        
    else        
    (        
     (        
     Case        
     When timesheettemp.isoverride = 0        
     then          
      (SELECT         
       SUM(ISNULL(cast([TimeSheetDetailtemp].[UnitValue]as float),'0'))        
      FROM         
       [TimeSheetDetailtemp]          
      WHERE         
       [TimeSheetDetailtemp].[TimesheettempID]=[timesheettemp].[timesheettempid])         
     else        
      timesheettemp.overridevalue        
     end           
     )         
    )         
   end         
   ) as Hours,        
   (SELECT         
    SUM(ISNULL(cast([TimeSheetDetailtemp].[UnitValue]as float),'0'))        
   FROM         
    [TimeSheetDetailtemp]          
   WHERE         
    [TimeSheetDetailtemp].[TimesheettempID]=[timesheettemp].[timesheettempid]) as orgHours,        
   timesheettemp.isoverride,        
   timesheettemp.overridevalue,        
   timesheettemp.iscpgsubmission,        
   timesheettemp.vacation,   
   Null as pdfQueryString,      
   timesheettemp.submittedpdf,   
   timesheetnote.Comment as Comment,    
   'ETimeSheet'as title ,    
   timesheettemp.createDate  ,    
   timesheettemp.approved_date,    
   timesheettemp.rejected_date,    
    timesheettemp.cancelled_date,
     0 as DisplayOrder
  from        
   timesheettemp        
   inner join Agreement_ContractDetail on Agreement_ContractDetail.AgreementID = timesheettemp.AgreementID        
   inner join Agreement on Agreement.agreementid = timesheettemp.AgreementID        
   inner join TimeSheetAvailablePeriod  on TimeSheetAvailablePeriod.TimeSheetAvailablePeriodID = timesheettemp.TimeSheetAvailablePeriodID   
   inner join users u1 on u1.userid = timesheettemp.CandidateUserID        
   inner join company ON company.CompanyID = agreement.CompanyID   
   left join picklist on picklist.picklistid=timesheettemp.statusid
   left join timesheetnote on timesheetnote.timesheetid=timesheettemp.timesheettempid  
  where        
   timesheettemp.candidateuserid = @CandidateID    
   and timesheettemp.inactive=0  
      
  union  
    
	SELECT * FROM #t2
       
 
SELECT 
	
	TimesheetID,
	timesheettype,      
	timesheetStatus,
	contractDesc,
	ContractID,
	payPeriod,      
	CompanyName,
	iscpgsubmission ,
	vacation,
	pdfQueryString,
	submittedpdf,
	timesheet_note,
	isoverride,
	(CASE 
		WHEN timesheetStatus = 'Submitted' and timesheettype = 'Etimesheet' 
		THEN 1
		ELSE 0
	END) AS isResendEnabled,
	(CASE WHEN iscpgsubmission =1 THEN Hours
     ELSE
		  CASE WHEN isoverride =1 THEN
			   CASE WHEN Hours > OrgHours THEN hours - OrgHours 
			   ELSE OrgHours - hours
			   END
		  ELSE
			OrgHours
		  END

	END) as tsHours,
(
CASE WHEN iscpgsubmission =1 THEN Hours
	 WHEN isoverride =1  THEN
		CASE WHEN Hours > OrgHours THEN
			 OrgHours + (Hours-OrgHours)
		ELSE
			 OrgHours - (OrgHours-Hours)
		END
	ELSE OrgHours
END) as varHrs,
(CASE WHEN isoverride =1 and iscpgsubmission =0 THEN
	CASE WHEN Hours > OrgHours THEN
		'<span style=color:green>' + cast(OrgHours as varchar)+ ' + ' + cast((Hours-OrgHours) as varchar)+ ' </span>
				<span style=font-size:10px;>
				<br>
				<a href= '+'javascript:showMe('+'''OverrideDetails'',''CII.PIP_ViewOverrideNote'',''UV='+cast(timesheetid as varchar)+''',3)>'
					+' [View Override Note]
				</a>
		</span>'
	ELSE
		'<span style=color:red>'+ cast(OrgHours as varchar)+ ' - ' + cast((OrgHours-Hours) as varchar)+' </span>
		<span style=font-size:10px;>
		<br>
		<a href= '+'javascript:showMe('+'''OverrideDetails'',''CII.PIP_ViewOverrideNote'',''UV='+cast(timesheetid as varchar)+''',3)>'
			+' [View Override Note]
		</a>
		</span>'

	END
ELSE
''
END) as Overridecolor,


(CASE 
		WHEN timesheetStatus ='submitted' or timesheetStatus = 'Saved'
			THEN FORMAT(CAST(Submitteddate as datetime),'dd/MM/yyyy hh:mm tt')
		WHEN timesheetStatus ='Accepted' or timesheetStatus = 'Approved' or timesheetStatus = 'Moved'
			THEN FORMAT(CAST(ApprovedDate as datetime),'dd/MM/yyyy hh:mm tt')
		WHEN timesheetStatus = 'cancelled' or timesheetStatus = 'Save/Cancel'
			THEN FORMAT(CAST(CancelledDate as datetime),'dd/MM/yyyy hh:mm tt') 
		WHEN timesheetStatus = 'Rejected'
			THEN FORMAT(CAST(RejectedDate as datetime),'dd/MM/yyyy hh:mm tt') 
	END)  as statusTitleString

FROM #TMP_FinalResultSet WHERE [#TMP_FinalResultSet].timesheetStatus NOT LIKE '%open%'
ORDER BY DisplayOrder,TimesheetID DESC 


END 

GO




/*
	**************************************Create UspTimesheetGetBillingPeriods_TSAPP SP*******************************
*/

CREATE proc [dbo].[UspTimesheetGetBillingPeriods_TSAPP]                                
(                                                                                            
@contractid int,                                                            
  @paymentplanid int,                                                                                                
  @DRUserID INT                                                
)                                                                                                                           
                                                                                                                            
as                                                                                                               
                      
  DECLARE @PaymentplanTitle VARCHAR(50)                           
  DECLARE @timesheetavailableperiodid INT, @mintimesheetid INT, @curTSAPId INT, @agreeementsubid INT                          
  DECLARE @CountofDeclined INT, @CountofCancelled INT, @CountofAppSub INT                                                      
  DECLARE @contractstartdate DATETIME, @contractenddate DATETIME                  
  DECLARE @MonthlyFirstDay DATETIME, @monthlylastday DATETIME, @semimonthlyfirstday DATETIME, @semimonthlylastday DATETIME                                        
  DECLARE @TSPickTypeId INT
        
 SET NOCOUNT ON      
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED      
                            
  SET @contractstartdate = ( SELECT a1.startdate FROM [Agreement] a1 WHERE a1.agreementid = @contractid)                                          
  SET @contractenddate = ( SELECT a1.enddate FROM [Agreement] a1 WHERE a1.agreementid = @contractid)                                      
  SET @agreeementsubid = ( SELECT a1.[AgreementSubID] FROM [Agreement] a1 WHERE a1.agreementid = @contractid)                                      
  SET @PaymentplanTitle = (                                         
							 SELECT p1.title                                         
							 FROM picklist                                         
								INNER JOIN picklist p1 ON p1.picklistid = picklist.var6                                        
							 WHERE picklist.picklistid = @paymentplanid)                                     
                                         
                                        
 SET @MonthlyFirstDay = (SELECT                                         
   CONVERT(                                   
     DATETIME,                        (                                         
      CONVERT(VARCHAR(2),MONTH(a1.startdate))                                         
      + '/'                                         
   + STR(DAY(dbo.FirstDayOfMonth(a1.startdate)))                                        
      + '/'                                         
      + CONVERT(VARCHAR(4),YEAR(a1.startdate))                                        
    )                                        
                                                    
     )                                         
     FROM agreement a1                                         
     WHERE a1.agreementid = @contractid)                                        
                                         
                                         
 SET @monthlylastday = ( SELECT                                         
   CONVERT(                                        
     DATETIME,                                        
     (                                         
      CONVERT(VARCHAR(2),MONTH(a1.enddate))                                         
      + '/'                                         
      + STR(DAY(dbo.LastDayOfMonth(a1.enddate)))                                        
      + '/'                                         
      + CONVERT(VARCHAR(4),YEAR(a1.enddate))                                        
    )                     
  
     )                                         
     FROM agreement a1                                         
     WHERE a1.agreementid = @contractid)                                        
                                                
                                        
 SET @semimonthlyfirstday = (SELECT                                         
   CONVERT(                                        
     DATETIME,                                        
     (                                         
      CONVERT(VARCHAR(2),MONTH(a1.startdate))                                
      + '/'                                         
      + STR(16)                                        
      + '/'                                     
      + CONVERT(VARCHAR(4),YEAR(a1.startdate))                                        
    )                                        
                                              
     )                                         
     FROM agreement a1                                         
     WHERE a1.agreementid = @contractid)                                        
                                         
                                         
 SET @semimonthlylastday = @monthlylastday        
 
 SELECT @TSPickTypeId=picktypeid                                                       
					 FROM picktype                                       
					 WHERE TYPE = 'Timesheetstatustype'                                                                         
     
if object_id('tempdb..#RecordsAlreadyInPAM') is not null                  
  drop table #RecordsAlreadyInPAM                  
                    
  select          
		 contractid,      
         agreementid,               
         pam.DATE1    
  into [#RecordsAlreadyInPAM]     
  from  VWHistoricalReplicatedPam10501 pam     
  where pam.agreementid=@contractid  and PAYTYPID<>'INSURANCE'   --To exclude the insurance fee(month end -31st date) entry from pam table to avail the 16-31 timeperiod for candidate
   
      
  CREATE NONCLUSTERED INDEX IDX_Date1 ON [#RecordsAlreadyInPAM](date1)      
                              
/*** select available timeperiods for contract paymentplan from MG2 ***/                              
if object_id('tempdb..#tmp_availabletimeperiods') is not null                                                      
  begin                                                                                                          
     drop table #tmp_availabletimeperiods                              
  end                                                              
  create table #tmp_availabletimeperiods                                              
  (                                                       
   [TimeSheetAvailablePeriodID] int default NULL,                              
   TimeSheetAvailablePeriodStartDate DATETIME,                              
   TimeSheetAvailablePeriodEndDate DATETIME                              
  )                                                      
  INSERT INTO [#tmp_availabletimeperiods] (                              
  [TimeSheetAvailablePeriodID],                              
  TimeSheetAvailablePeriodStartDate,                              
  TimeSheetAvailablePeriodEndDate                              
  )                               
  SELECT DISTINCT                               
   [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodID],                              
   [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodStartDate],                              
   [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodEndDate]                               
  FROM [TimeSheetAvailablePeriod]                               
  WHERE [TimeSheetPaymentType] = @paymentplanid                              
                            
                       
/*** select timesheetids here ***/                               
if object_id('tempdb..#tmp_TimesheetIds') is not null                                                      
  begin                                                                                                            
     drop table #tmp_TimesheetIds                                                      
  end                                                                                                            
  create table #tmp_TimesheetIds                                                       
  (                                                       
   timesheetid int default null                                                      
  )                                                      
                                                        
/*** insert timesheetid's of Rejected status into #tmp_TimesheetIds table ***/                                                      
  declare tsap_cursor cursor for                                                       
  select timesheet.timesheetavailableperiodid                                                      
  from timesheet                                                      
  where timesheet.agreementid = @contractid                                                      
  and dbo.TimeSheet.inactive = 0                                          
  group by timesheet.agreementid, timesheet.timesheetavailableperiodid                              
                                                       
  open tsap_cursor                                      
                                                        
  fetch next from tsap_cursor                                                      
  into @curTSAPId                                                      
                                                        
  while @@fetch_status = 0                                                      
  begin                   
                                                           
   select @CountofDeclined = count(timesheetid), @mintimesheetid = max(timesheetid)                                                      
   from timesheet             
   inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                                      
   inner join picklist on picklist.picklistid = timesheet.statusid                             
   inner join agreement on agreement.agreementid = timesheet.agreementid                                                      
   where timesheet.agreementid = @contractid                                           
   AND timesheet.inactive = 0                                          
   and timesheet.statusid = dbo.udf_GetPicklistId( 'Timesheetstatustype', 'Rejected',-1)                                                      
   and timesheet.timesheetavailableperiodid = @curTSAPId                                          
                          
if @CountofDeclined > 0                                                   
   begin                                                      
    select @CountofAppSub = count(timesheet.timesheetid)                                                      
    from timesheet                                                 
    inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                                      
    where                                                       
    timesheetid  not in ( @mintimesheetid )                                          
    AND dbo.TimeSheet.inactive=0                                                       
    and timesheet.agreementid = @contractid                                                      
    and timesheet.timesheetavailableperiodid = @curTSAPId                                                      
    and timesheet.statusid in                                                       
    (                                                      
    select picklistid                                                       
    from picklist                                                      
    where                                                       
    picklist.title in ( 'Approved', 'Submitted','Accepted','Batched','Moved')                                                       
    and picklist.picktypeid =                                                       
     (                                                       
     select picktypeid                                                       
     from picktype                                       
     where type = 'Timesheetstatustype'                                                      
     )                                                      
    )                                                      
                                                        
    if ( @CountofAppSub = 0 )                                                      
    begin                                                      
     insert into #tmp_TimesheetIds values ( @mintimesheetid )                                                      
    end                                                      
                                                          
   end                                                      
                                                        
   fetch next from tsap_cursor                                                      
   into @curTSAPId                                    
                                                        
  end                                                      
   close tsap_cursor                                                      
   deallocate tsap_cursor                                                      
                                                        
/*** insert timesheetid's of cancelled status into #tmp_TimesheetIds table ***/                                                        
  declare tsap_cursor cursor for                                                       
  select timesheet.timesheetavailableperiodid                                                      
  from timesheet                                                      
  where timesheet.agreementid = @contractid                                                      
  AND timesheet.inactive = 0           
  group by timesheet.agreementid, timesheet.timesheetavailableperiodid                                                      
                                                        
  open tsap_cursor                                                      
                                                     
  fetch next from tsap_cursor         
  into @curTSAPId                                                      
                                                        
  while @@fetch_status = 0                                                 
  begin                  
                         
   select @CountofCancelled = count(timesheetid), @mintimesheetid = max(timesheetid)                                                      
 from timesheet                                                       
   inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                                      
   inner join picklist on picklist.picklistid = timesheet.statusid                                                      
   inner join agreement on agreement.agreementid = timesheet.agreementid                                                      
   where timesheet.agreementid = @contractid                                           
   AND timesheet.inactive = 0                                                    
   and timesheet.statusid = dbo.udf_GetPicklistId( 'Timesheetstatustype', 'Cancelled',-1)                                                      
   and timesheet.timesheetavailableperiodid = @curTSAPId                                                      
                                                        
   if @CountofCancelled > 0                                                      
   begin                                                      
    select @CountofAppSub = count(timesheet.timesheetid)                                                      
    from timesheet                                                       
    inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid = timesheet.timesheetavailableperiodid                                                      
    where                                                       
    timesheetid  not in ( @mintimesheetid )                                          
    AND timesheet.inactive = 0                                                       
    and timesheet.agreementid = @contractid                                                      
    and timesheet.timesheetavailableperiodid = @curTSAPId                                                      
    and timesheet.statusid in                                                       
    (                                                      
    select picklistid                                                       
    from picklist                                                      
    where                                                       
    picklist.title in ( 'Approved', 'Submitted','Accepted','Batched','Moved')                                                       
    and picklist.picktypeid =        
     (                                                       
     select picktypeid                                                       
     from picktype                                                       
     where type = 'Timesheetstatustype'                                                      
     )                                                      
    )                                                      
          
    if ( @CountofAppSub = 0 )                                                      
 begin                                                     
     insert into #tmp_TimesheetIds values ( @mintimesheetid )                                                      
    end                                                      
         
   end                                                      
                                                        
   fetch next from tsap_cursor                                                      
   into @curTSAPId                                                      
                                                        
  end                                                      
   close tsap_cursor                        
   deallocate tsap_cursor                                                      
                                   
                                   
/*** insert final result into tmp_billingperdiods table ***/                                                      
  if object_id('tempdb..#tmp_BillingPeriods') is not null                                                      
  begin                                                                                                            
     drop table #tmp_BillingPeriods                                                      
  end                                     
                                                        
   create table #tmp_BillingPeriods                                                
  (
   timesheetstatus varchar(50) default null,                                                       
   timesheetavailableperiodstartdate datetime null,                                                       
   timesheetavailableperiodenddate datetime null,                                                      
   timesheetid int,                                          
   directreportname VARCHAR(100) DEFAULT null,                                                     
   timesheetavailableperiodid int,                                                      
   tsstartdate int,                                                      
   tsenddate INT,                                          
   inactive BIT  ,                  
   agreementid int,              
   timesheet_note varchar(8000),            
   timesheettype varchar(200),
   AgreementsubId int
   
   --iscpgsubmission BIT                                          
  )                                                      
                                   
  insert into #tmp_BillingPeriods                                           
  select                                                       
   dbo.udf_GetPicklistTitle(timesheet.statusid,-1) timesheetstatus,                                                       
   timesheetavailableperiod.timesheetavailableperiodstartdate,                                                      
   timesheetavailableperiod.timesheetavailableperiodenddate,                                                      
   timesheet.timesheetid,                                                    
   u1.firstname +' '+  u1.lastname,                                                    
   timesheetavailableperiod.TimeSheetAvailablePeriodID,                                                      
   day(timesheetavailableperiod.timesheetavailableperiodstartdate) tsstartdate,                                                      
   day(timesheetavailableperiod.timesheetavailableperiodenddate) tsenddate,                                          
   timesheet.inactive  ,                  
   @contractid,              
   timesheetnote.Comment,            
   p1.title as timesheettype,
   agreement.AgreementSubId
   --timesheet.iscpgsubmission                                          
                                             
  from timesheet                                                      
   inner join timesheetavailableperiod on timesheetavailableperiod.timesheetavailableperiodid =  timesheet.timesheetavailableperiodid                                                      
   LEFT JOIN agreement ON agreement.agreementid = timesheet.agreementid                     
   LEFT JOIN [Agreement_ContractAdminContactMatrix] ON [Agreement_ContractAdminContactMatrix].[AgreementID] = [Agreement].[AgreementID]                                                    
   LEFT JOIN users u1 ON u1.userid = Agreement_ContractAdminContactMatrix.directreportuserid                                                    
   left join timesheetnote on timesheetnote.timesheetid=timesheet.timesheetid              
 left join picklist p1 on p1.picklistid = timesheet.timesheettype            
  where timesheet.timesheetid in ( select timesheetid from #tmp_TimesheetIds )                 
  AND timesheet.inactive = 0                                          
  and Agreement_ContractAdminContactMatrix.directreportuserid = @DRUserID                                             
                                 
union                                                       
 select                                                        
   (                                                      
    Case When statusid is null                                                      
    then 'Open'                                                      
    end                                  
   ) as timesheetstatus,                                                      
   timesheetavailableperiod.timesheetavailableperiodstartdate,                                                      
   timesheetavailableperiod.timesheetavailableperiodenddate,                                                      
   timesheet.timesheetid,                                                      
   u1.firstname +' '+  u1.lastname AS directreportname,                                                       
   timesheetavailableperiod.TimeSheetAvailablePeriodID,                       
   day(timesheetavailableperiod.timesheetavailableperiodstartdate) tsstartdate,                                                      
   day(timesheetavailableperiod.timesheetavailableperiodenddate) tsenddate,                              
   ISNULL(TimeSheet.inactive,0)  , @contractid ,              
   timesheetnote.Comment,            
 p1.title as timesheettype,
 agreement.AgreementSubId
                                          
from timesheetavailableperiod                                                       
  LEFT JOIN timesheet ON timesheetavailableperiod.TimeSheetAvailablePeriodID = timesheet.TimeSheetAvailablePeriodID                                          
 AND ((timesheet.agreementid = @contractid AND timesheet.inactive = 0) OR timesheet.agreementid IS NULL)                           
  left join picklist on picklist.picklistid = timesheet.statusid                                                      
  LEFT JOIN agreement ON agreement.agreementid = timesheet.agreementid                                                  
  LEFT JOIN [Agreement_ContractAdminContactMatrix] ON [Agreement_ContractAdminContactMatrix].[AgreementID] = [Agreement].[AgreementID]                                                    
  LEFT JOIN users u1 ON u1.userid = Agreement_ContractAdminContactMatrix.directreportuserid                                                    
  left join picklist p1 on p1.picklistid = timesheet.timesheettype            
   left join timesheetnote on timesheetnote.timesheetid=timesheet.timesheetid                                                        
  where                                      
   timesheetavailableperiod.[TimeSheetAvailablePeriodStartDate] >=                                       
  (                                        
   CASE WHEN @PaymentplanTitle = 'Monthly'                                        
   THEN @monthlyfirstday                                        
   ELSE (                                      
    CASE                                       
    WHEN @contractstartdate >= timesheetavailableperiod.[TimeSheetAvailablePeriodStartDate] AND @contractstartdate < @semimonthlyfirstday                                  
    THEN @monthlyfirstday                                  
    ELSE @semimonthlyfirstday             
    END                                      
   )                                      
   END                                        
  )                                     and [TimeSheetAvailablePeriod].[TimeSheetAvailablePeriodEndDate] <=                                        
  (                                        
   CASE WHEN @PaymentplanTitle = 'Monthly'                                        
   THEN @monthlylastday                                        
   ELSE                   
  (                  
   CASE                                         
   WHEN @contractenddate >= timesheetavailableperiod.[TimeSheetAvailablePeriodStartDate]                   
   THEN @semimonthlylastday                                    
   ELSE dateadd(d,-1,@contractstartdate)                  
   END                                        
  )                     
   END                                        
  )                  
  and                                     
   (                                    
 timesheet.TimeSheetAvailablePeriodID is null                                      
 OR                                    
 timesheet.[TimeSheetAvailablePeriodID] IS NOT NULL AND timesheet.[AgreementID] = @contractid AND timesheet.iscpgsubmission = 1                                    
   )                                       
  and timesheetavailableperiod.TimeSheetPaymentType = @paymentplanid                                                          
                                      
 if object_id('tempdb..#tmp_BillingTSIds') is not null                                                      
  begin                                                                                               
     drop table #tmp_BillingTSIds                                                      
  end                                                                                                          
                                                        
  create table #tmp_BillingTSIds ( BillingTSIds int)                                                      
                                                      
  declare @curTSID int                                                      
  declare @curTSStatus varchar(50)                                                      
  declare @curTSAvPID int                                                      
                                                        
  declare billing_cursor cursor for                                                       
  select                                                      
   timesheetid,                                                      
   timesheetavailableperiodid,                                              
   timesheetstatus                                                      
                          
  from #tmp_BillingPeriods                                                       
                                                      
  open billing_cursor                          
                                                        
  fetch next from billing_cursor                                                      
  into @curTSID, @curTSAvPID, @curTSStatus                                                      
                                           
  while @@fetch_status = 0                                                      
  begin                                                      
   declare @cancount int                                                      
   declare @rejcount int                                                      
   declare @maxCanTSID int                                                      
   declare @maxRejTSID int                                                      
                                   
   if @curTSStatus = 'Rejected'                                             
   begin                                                      
                                                          
    select @maxCanTSID = max(tBP.timesheetid), @cancount = count(tBP.timesheetid)                                                        
    from #tmp_BillingPeriods tBP                                                       
    where tBP.timesheetavailableperiodid = @curTSAvPID                                                      
    and tBP.timesheetstatus = 'Cancelled'                                                      
    if @cancount <> 0                                                      
    begin                                                      
     if @curTSID > @maxCanTSID and @maxCanTSID is not null                                                      
     begin         
      insert into #tmp_BillingTSIds values ( @curTSID)                                                        
     end                                                      
    end                                                      
    else if @cancount = 0                                                      
    begin                                                      
  insert into #tmp_BillingTSIds values ( @curTSID)                                                      
    end                                                      
   end                                                      
   else if @curTSStatus = 'Cancelled'                     
   begin                                                      
    select @maxRejTSID = max(tBP.timesheetid), @rejcount = count(tBP.timesheetid)                                                        
    from #tmp_BillingPeriods tBP                                                       
    where tBP.timesheetavailableperiodid = @curTSAvPID                                                
    and tBP.timesheetstatus = 'Rejected'                                                      
                                                      
    if @rejcount <> 0                                                      
    begin                                                      
     if @curTSID > @maxRejTSID and @maxRejTSID is not null                                                      
     begin                                              
      insert into #tmp_BillingTSIds values ( @curTSID)                                                        
     end                                                      
    end                                                      
    else if @rejcount = 0                                                      
    begin                                                      
     insert into #tmp_BillingTSIds values ( @curTSID)                                                          
    end                                                      
   end                                                      
                                                      
  fetch next from billing_cursor                                                      
  into @curTSID, @curTSAvPID, @curTSStatus                                                      
                                                      
  end                                                       
                                                      
  close billing_cursor                                                      
  deallocate billing_cursor                                                         
                             
update [#tmp_BillingPeriods]                  
   set [#tmp_BillingPeriods].[timesheetavailableperiodstartdate] = null                  
  ,[#tmp_BillingPeriods].[timesheetavailableperiodenddate] = null                  
  ,[#tmp_BillingPeriods].[timesheetavailableperiodid] = null                   
  from #tmp_billingPeriods                  
 inner join [#RecordsAlreadyInPAM]                  
    on #tmp_billingPeriods.[agreementid] = [#RecordsAlreadyInPAM].[agreementid]                  
 where #RecordsAlreadyInPAM.[date1]                   
  between [#tmp_BillingPeriods].[timesheetavailableperiodstartdate]                  
            and [#tmp_BillingPeriods].[timesheetavailableperiodenddate]                  
                              
                               
                  
delete                   
  from [#tmp_BillingPeriods]                   
 where [#tmp_BillingPeriods].[timesheetavailableperiodstartdate] is null                  
   and [#tmp_BillingPeriods].[timesheetavailableperiodenddate] is null                  
   and [#tmp_BillingPeriods].[timesheetavailableperiodid] is null                  
                     
                     
       
  select case                                                             
    when timesheetstatus = 'Rejected' then 'Rejected/Open'                                                             
    when timesheetstatus = 'Cancelled' then 'Cancelled/Open'                                                            
    when (timesheetstatus = 'Approved' or  timesheetstatus ='Accepted')--AND iscpgsubmission = 1                                                 
    THEN 'Open'                                                
 else timesheetstatus                                                          
    end                                                             
   as timesheetstatus,                           
   (                          
   CASE                          
 WHEN EXISTS                           
   (SELECT 1                           
    FROM TimesheetTemp                           
    WHERE TimesheetTemp.TimesheetID = #tmp_BillingPeriods.TimesheetID                           
    AND TimesheetTemp.Inactive = 0)                          
   AND (#tmp_BillingPeriods.TimesheetStatus = 'Rejected' OR #tmp_BillingPeriods.TimesheetStatus = 'Cancelled')                          
  THEN                           
   (                          
    SELECT TimesheetTemp.TimeSheetTempID                          
    FROM TimesheetTemp                           
    WHERE TimesheetTemp.TimesheetID = #tmp_BillingPeriods.TimesheetID                           
    AND TimesheetTemp.Inactive = 0                              
   ) /* Rejected/Cancelled Period entry in TimesheetTemp */                          
 WHEN EXISTS                          
   (SELECT 1                           
    FROM TimesheetTemp                           
    WHERE TimesheetTemp.AgreementID = @contractid                           
    AND TimesheetTemp.timesheetavailableperiodid = #tmp_BillingPeriods.timesheetavailableperiodid                          
    AND TimesheetTemp.Inactive = 0)                          
   AND (#tmp_BillingPeriods.TimesheetStatus <> 'Rejected' AND #tmp_BillingPeriods.TimesheetStatus <> 'Cancelled')                            
  THEN                           
   (                          
    SELECT TimesheetTemp.TimeSheetTempID                          
    FROM TimesheetTemp                           
    WHERE TimesheetTemp.AgreementID = @contractid                           
    AND TimesheetTemp.timesheetavailableperiodid = #tmp_BillingPeriods.timesheetavailableperiodid                          
    AND TimesheetTemp.Inactive = 0                          
   ) /* Open Period entry in TimesheetTemp */                          
 WHEN NOT EXISTS                          
   (SELECT 1                           
    FROM TimesheetTemp                           
    WHERE TimesheetTemp.AgreementID = @contractid                           
    AND TimesheetTemp.timesheetavailableperiodid = #tmp_BillingPeriods.timesheetavailableperiodid                          
    AND TimesheetTemp.Inactive = 0)                          
   AND (#tmp_BillingPeriods.TimesheetStatus <> 'Rejected'                    
   AND #tmp_BillingPeriods.TimesheetStatus <> 'Cancelled'                         
   AND #tmp_BillingPeriods.TimesheetStatus <> 'Approved'              
   AND #tmp_BillingPeriods.TimesheetStatus <> 'Accepted'              
   )                            
  THEN 0                          
 WHEN NOT EXISTS                           
   (SELECT 1                           
    FROM TimesheetTemp                           
    WHERE TimesheetTemp.TimesheetID = #tmp_BillingPeriods.TimesheetID                           
    AND TimesheetTemp.Inactive = 0)              
   AND (#tmp_BillingPeriods.TimesheetStatus = 'Rejected' OR #tmp_BillingPeriods.TimesheetStatus = 'Cancelled')                          
  THEN 0                          
 END                           
                            
 ) AS TimesheetTempID,                           
                             
   timesheetavailableperiodstartdate,                                                             
   timesheetavailableperiodenddate,                                                            
   dbo.udf_getcustomdate(timesheetavailableperiodstartdate,timesheetavailableperiodenddate) TSCustomDate,                      
   timesheetid,                                                 
   directreportname,                                                          
   timesheetavailableperiodid,                                                            
   tsstartdate,                                                            
   tsenddate,            
   case                                                             
    when timesheetstatus = 'Rejected' then 1                                                             
    when timesheetstatus = 'Cancelled' then 2                                            
    else 3                                                   
    end                                                             
   as billperiodsorder,                                                          
 case                                                             
    when timesheetstatus = 'Rejected'                                                             
     then (                                                             
       select rejectedpdf                                                      
       from timesheet                                                             
       where timesheet.timesheetid = #tmp_BillingPeriods.timesheetid                                                            
      )                                                            
    when timesheetstatus = 'Cancelled'                                                             
     then (                                                             
       select cancelledpdf                                                  
       from timesheet                                                             
       where timesheet.timesheetid = #tmp_BillingPeriods.timesheetid                                                            
      )                                                            
    else null end                                                      
   as pdfname,              
  timesheet_note,            
  timesheettype,
  AgreementId,
  AgreementSubId
  from #tmp_BillingPeriods                                
  where                                                     
    #tmp_BillingPeriods.timesheetid in ( select BillingTSIds from #tmp_BillingTSIds)                                         
    OR #tmp_BillingPeriods.timesheetid IS NULL --OR #tmp_BillingPeriods.inactive = 1                                                
                                                          
  order by billperiodsorder 


GO

/*
	**************************************Create GetOpenTimeSheetForCandidate SP*******************************
*/

CREATE PROC [dbo].[UspGetOpenTSForCandidate_TSAPP]
(
	@candidateID INT,
	@TimesheetType varchar(5)
)
AS

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


	if object_id('tempdb..#ALLCONTRACTS') is not null                                              
	begin                                              
		DROP TABLE #ALLCONTRACTS                                     
	end        

	if object_id('tempdb..#PENDINGCONTRACTS') is not null                                              
	begin                                              
		DROP TABLE #PENDINGCONTRACTS                                     
	end        

	if object_id('tempdb..#CandidateBillingPeriods') is not null                                              
	begin                                              
		DROP TABLE #CandidateBillingPeriods                                     
	end        

	if object_id('tempdb..#FinalCandidateBillingPeriods') is not null                                              
	begin                                              
		DROP TABLE #FinalCandidateBillingPeriods                                     
	end      
	
	  Create table #CandidateBillingPeriods                                                

	  (                                                       

	   timesheetstatus varchar(50) default null,      
	   
	   TimeSheetTempID int,                                                 

	   timesheetavailableperiodstartdate datetime null,                                                       

	   timesheetavailableperiodenddate datetime null,                                                      

	   TSCustomDate varchar(15),

	   timesheetid int,                                          

	   directreportname VARCHAR(100) DEFAULT null,                                                     

	   timesheetavailableperiodid int,                                                      

	   tsstartdate int,                                                      

	   tsenddate INT,    
	   
	   billperiodsorder INT,                                      

	   pdfname varchar(200),          

	   timesheet_note varchar(8000),            

	   timesheettype varchar(200),
	   
	   agreementid int,         

	   agreementsubid int                       

	  )                                                      



	
	DECLARE @AgreementType int
	DECLARE @AgreementSubPicktype int
	DECLARE @ETimesheetId int
	DECLARE @ClientTimesheetId int
	
	select 	@AgreementType = picklistid
				from	picklist
				where	picktypeid =
					(
					select 	picktypeid
					from	picktype
					where	type = 'AgreementType'
					)
				and	title = 'Contract'
				
	select 	@AgreementSubPicktype = picktypeid
			from	picktype
			where	type = 'ContractType'
			
	select @ETimesheetId=picklistid from dbo.udf_getpicklistids('TimesheetType','ETimesheet',-1)
	
	select @ClientTimesheetId=picklistid from dbo.udf_getpicklistids('TimesheetType','Client',-1)
	
	select 	distinct
	
			agreement.agreementsubid as contractid,
			agreement.agreementid,
			agreement.startdate,
			agreement.enddate,
			p1.title as contractstatus,
			agreement_contractdetail.jobtitle,
			p2.title as timesheettype,
			u1.firstname + ' ' + u1.lastname as DirectReportName,
			agreement.companyid,
			company.CompanyName,
			u1.userid as DirectReportID,
			agreement_contractdetail.ContractPaymentPlanType,
			dbo.udf_getpicklisttitle(agreement_contractdetail.ContractPaymentPlanType,-1) PaymentPlanTypeTitle,
			1 as Active,
			newId() AS SNO
	INTO #ALLCONTRACTS

	from	agreement
			inner join agreement_contractdetail on agreement_contractdetail.agreementid = agreement.agreementid
			inner join company on company.companyid=agreement.companyid
			left join picklist p1 on p1.picklistid = agreement.statustype
			left join picklist p2 on p2.picklistid = agreement_contractdetail.TimeSheetType
			left join Agreement_ContractAdminContactMatrix on Agreement_ContractAdminContactMatrix.agreementid = agreement.agreementid
			and Agreement_ContractAdminContactMatrix.inactive = 0
			inner join users u1 on u1.userid = Agreement_ContractAdminContactMatrix.DirectReportUserID
	
	where	
			agreement.agreementtype = @AgreementType
				
			and agreement.agreementsubtype in
				(
				select 	picklistid
				from	picklist
				where	picktypeid =@AgreementSubPicktype
				and	title in ('Consultant','Flo Thru','Contract To Hire')
				)	
			and agreement.candidateid = @candidateID
			and (
					 (rtrim(ltrim(@TimesheetType)) ='ETS' and  agreement_contractdetail.timesheettype = @ETimesheetId)
					 OR
					 (agreement_contractdetail.timesheettype <> @ClientTimesheetId )
			    )
			and agreement.enddate  > '2008-05-31'
	order by 
	contractid desc

	DECLARE @CNT int
	DECLARE @PendingCNT int

	SELECT @CNT=count(*) FROM #ALLCONTRACTS
	SELECT @PendingCNT=count(*) FROM #ALLCONTRACTS WHERE contractstatus='PENDING'

	DECLARE @RecordCount INT
	DECLARE @contractid int
	DECLARE @paymentplanid int
	DECLARE @DirectReportID int
	DECLARE @SNO uniqueidentifier

	SET @RecordCount=0
	print @CNT
	WHILE @RecordCount<@CNT
	BEGIN
		SELECT TOP 1 @contractid=agreementid,@paymentplanid=ContractPaymentplantype, @DirectReportID=DirectReportID,@SNO=SNO FROM #ALLCONTRACTS WHERE Active=1 order by 1 desc

		INSERT INTO  #CandidateBillingPeriods
		
		EXEC dbo.UspTimesheetGetBillingPeriods_TSAPP @contractid, @paymentplanid, @DirectReportID
		
		SET @RecordCount=@RecordCount+1

		UPDATE #ALLCONTRACTS SET Active=0  WHERE SNO=@SNO and  Active=1
	END


	SELECT	c.*,
			a.enddate,a.contractid,a.DirectReportName as ContractDirectReportName,a.CompanyName as ClientName,
			cast((ROW_NUMBER() OVER (PARTITION BY c.agreementid ORDER BY c.agreementid DESC )) as varchar) AS chk_count,
			(timesheetstatus+'|'+cast((CASE WHEN timesheetid > 0  then timesheetid  ELSE 0 END) as varchar)+'|'+
			cast(TimesheetAvailablePeriodStartdate as varchar(11)) +'|'+ cast(TimesheetAvailablePeriodEnddate as varchar(11))
			+'|'+ cast(timesheetavailableperiodid as varchar)+ '|' + cast((ROW_NUMBER()  OVER (PARTITION BY c.agreementid  ORDER BY c.agreementid desc)) as varchar)+'|'+
			cast(TimesheetTempID as varchar)+'|'+ cast(c.agreementid as varchar)+'|'+contractstatus+'|'+a.TimesheetType+'|'+(CASE WHEN tsstartdate =1 and tsenddate >15 then 'Monthly' ELSE  'SemiMonthly' END)+'|'+cast(directreportid as varchar)) as  valueString,
			(timesheetstatus+'|'+cast((CASE WHEN timesheetid > 0  then timesheetid  ELSE 0 END) as varchar)+'|'+
			cast(TimesheetAvailablePeriodStartdate as varchar(11)) +'|'+ cast(TimesheetAvailablePeriodEnddate as varchar(11))
			+'|'+ cast(timesheetavailableperiodid as varchar)+ '|' + cast((ROW_NUMBER()  OVER (PARTITION BY c.agreementid  ORDER BY c.agreementid desc)) as varchar)+'|'+
			cast(TimesheetTempID as varchar)+'|'+ '1' +'|'+ cast(c.agreementid as varchar)+'|'+contractstatus+'|'+a.TimesheetType+'|'+(CASE WHEN tsstartdate =1 and tsenddate >15 then 'Monthly' ELSE  'SemiMonthly' END)+'|'+cast(directreportid as varchar)) as  valueStringZero,
			c.pdfname as pdfQueryString
	 INTO #FinalCandidateBillingPeriods
	 FROM #CandidateBillingPeriods c
	 INNER JOIN #ALLCONTRACTS a on a.agreementid=c.agreementid
	 order by c.agreementid desc

    SELECT 
	 	  
	  timesheetstatus ,      
	   
	  F.TimesheetTempID,

	  convert(varchar(12),timesheetavailableperiodstartdate,107)  as tsStartDate,                                                       

	  convert(varchar(12),timesheetavailableperiodenddate,107)  as tsEndDate,                                                      

	  TSCustomDate as timesheetPeriod,

	  F.timesheetid ,                                          

	  directreportname ,       
	  
	  ContractDirectReportName,                                              
	  	   
	  billperiodsorder,                                      

	  timesheet_note ,            

	  timesheettype ,
	   
	  F.agreementid ,         

	  contractid,

	  ClientName,

	  0 as TSSaveHours,

	 (CASE WHEN (timesheetavailableperiodstartdate > enddate and timesheetstatus <> 'Rejected/Open' and Chk_Count > 1) THEN 0
		WHEN (timesheetstatus <> 'Rejected/Open' and Chk_Count > 1) THEN 0
		WHEN (timesheetstatus = 'Cancelled/Open' or timesheetstatus = 'open') THEN 1
	 END) as IsEnabled,

	 valueString,

	 valueStringZero,

	 pdfQueryString,
	 
	 chk_count,

	 1 as TSSubmitOrSavedFlag,

	 @CNT as GetContracts,

	 @PendingCNT as GetContracts_Pending

	 FROM #FinalCandidateBillingPeriods F
	  LEFT JOIN TimesheetTemp  on TimesheetTemp.TimesheetTempID =F.TimesheetTempID
	  LEFT JOIN Picklist on picklist.picklistid=TimesheetTemp.statusid and Picklist.title='Cancelled'--	  =854

	 WHERE
		  (isnull(F.TimesheetTempID,0) = 0 or  (isnull(F.TimesheetTempID,0) > 0  and   Picklist.title='Cancelled' ))

	 UNION ALL

	 SELECT


		(CASE WHEN timesheetstatus = 'Open' THEN 'Saved' else timesheetstatus END) as timesheetstatus ,      
	 	   
	   TimesheetDetailTemp.TimesheetTempID TimeSheetTempID,                                              

	   convert(varchar(12),timesheetavailableperiodstartdate,107)  as tsStartDate,                                                       

	   convert(varchar(12),timesheetavailableperiodenddate,107)  as tsEndDate,                                                      

	   TSCustomDate as timesheetPeriod,


	   timesheetid ,                                          

	   directreportname ,  
	   
	   ContractDirectReportName,                                                   
	   	   
	   0 as billperiodsorder,                                      

	   timesheet_note ,            

	   timesheettype ,
	   
	   agreementid ,         

	   contractid,

	   ClientName,

	   SUM(ISNULL(cast([TimesheetDetailTemp].UnitValue as float),0)) TSSaveHours,
	   
	  (CASE WHEN (c.timesheetavailableperiodstartdate > c.enddate and timesheetstatus <> 'Rejected/Open' and Chk_Count > 1) THEN 0
		   WHEN (timesheetstatus <> 'Rejected/Open' and Chk_Count > 1) THEN 0
		   WHEN (timesheetstatus = 'Cancelled/Open' or timesheetstatus = 'open') THEN 1
	   END) as IsEnabled,

	   valueString,
	   
	   valueStringZero,

	   pdfQueryString,

	   chk_count,

	   0 as TSSubmitOrSavedFlag,

	   @CNT as GetContracts,

	   @PendingCNT as GetContracts_Pending

	 FROM TimesheetDetailTemp
	 LEFT JOIN #FinalCandidateBillingPeriods C on TimesheetDetailTemp.TimesheetTempID =C.TimesheetTempID
	 WHERE
		TimesheetDetailTemp.Inactive = 0 AND C.TimesheetTempID > 0
	 GROUP BY TimesheetDetailTemp.TimesheetTempID,c.timesheetavailableperiodstartdate,c.timesheetavailableperiodenddate,c.EndDate,TSCustomDate,
	 timesheetstatus,Chk_Count,valueStringZero,pdfQueryString,ContractDirectReportName,directreportname,agreementid,contractid,ClientName,valueString,
	 timesheetid, timesheet_note,timesheettype

	 ORDER BY agreementid DESC,chk_count      

END		

GO



/*
	**************************************Create Get Project PO Details*******************************
*/


USE [MatchGuideDev]
GO

/****** Object:  StoredProcedure [dbo].[UspGetProjectPODetails]    Script Date: 2/10/2016 1:25:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*---------------------------------------------------------------------------------------------------------------------------------------
Name:			[UspGetProjectPODetails]
Description:	What the stored procedure will do?
Purpose:		In what context this stored procedure will be used?
-----------------------------------------------------------------------------------------------------------------------------------------
Version		Date		Author			BugID		Notes
-----------------------------------------------------------------------------------------------------------------------------------------
1.0			27.09.2015  Karhikeyan.M	MG-11262	
-----------------------------------------------------------------------------------------------------------------------------------------*/
CREATE PROC [dbo].[UspGetProjectPODetails]
( 
	@agreementid int
)

AS 
SET NOCOUNT ON                                                     
              
BEGIN    
--select * from (		
		select 
			contractprojectpo.IsGeneralProjectPO,
			contractprojectpo.agreementid [AgreementId],contractprojectpo.InactiveForUser,
			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cpm.companyprojectid 
					from companypoprojectmatrix cpm 
						inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid)
				else companyproject.companyprojectid 
				end
			) [ProjectId],
			Null as EinvoiceId,
			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cp.description 
					from companypoprojectmatrix cpm 
						inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companyproject.[Description]
				end
			) [ProjectDescription],

			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cp.projectid 
					from companypoprojectmatrix cpm 
						inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companyproject.[projectid]
				end
			) [DisplayProjectID],
			
			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cp.CompanyProjectID 
					from companypoprojectmatrix cpm 
						inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companyproject.[CompanyProjectID]
				end
			) [CompanyProjectID],			

			contractprojectpo.contractprojectpoid [contractprojectpoid],

			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cpm.companypoid 
					from companypoprojectmatrix cpm 
						inner join companypo cpo on cpo.companypoid = cpm.companypoid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companypo.companypoid 
				end
			) [POId],

			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cpo.description 
					from companypoprojectmatrix cpm 
						inner join companypo cpo on cpo.companypoid = cpm.companypoid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companypo.[Description] 
				end
			) [PODescription],

			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cpo.PONumber 
					from companypoprojectmatrix cpm 
						inner join companypo cpo on cpo.companypoid = cpm.companypoid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companypo.PONumber
				end
			) DisplayPONumber,

			( case when contractprojectpo.companypoprojectmatrixid is not null 
				then ( select cpo.ponumber
					from companypoprojectmatrix cpm 
						inner join companypo cpo on cpo.companypoid = cpm.companypoid
					where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
					)
				else companypo.PONumber 
				end
			) [PONumber],
			companypoprojectmatrix.companypoprojectmatrixid,
			0 as sortval,
			contractprojectpo.verticalid,
			0 as EInvoiceType	
		from contractprojectpo
			left join companypoprojectmatrix on companypoprojectmatrix.companypoprojectmatrixid = contractprojectpo.companypoprojectmatrixid
			left join companyproject on contractprojectpo.companyprojectid = companyproject.companyprojectid
			left join companypo on companypo.companypoid =contractprojectpo.companypoid
		where contractprojectpo.agreementid = @agreementid
		and contractprojectpo.inactive = 0
		and contractprojectpo.InactiveForUser=0 

		union
		
		select 
		-1 as IsGeneralProjectPO,
		Con.contractId as AgreementId,
		Con.InactiveForUser,
		Null as ProjectId,
		con.InvoiceCodeId as EinvoiceId,
		Con.InvoiceCodeText as ProjectDescription,
		Con.InvoiceCodeText as DisplayProjectId,
		Null as CompanyProjectId,
		Null as ContractProjectPoid,
		Null as POID,
		Null as PODescription,
		Null as DisplayPOnumber,
		Null as POnumber,
		Null as CompanyPoProjectMatrixId,
		0 as Sortval,
		Agreement.verticalId,
		1 as EinvoiceType
		from ContractInvoiceCode Con
		inner join Agreement on Agreement.AgreementId=Con.ContractId
		where agreement.agreementId=@agreementid
		and Con.isActive=1
		and 	Con.InactiveForUser=0 
		--) as a 
		--where isnull([ProjectId],EinvoiceId) = @projectPOCode or POId = @projectPOCode or PONumber = @projectPOCode
END 


GO


/*
	**************************************Create Get Rate Term Details*******************************
*/

USE [MatchGuideDev]
GO

/****** Object:  StoredProcedure [dbo].[UspGetRateTermDetails_TSAPP]    Script Date: 2/10/2016 11:34:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*---------------------------------------------------------------------------------------------------------------------------------------
Name:			[UspGetRateTermDetails_TSAPP]
Description:	Get rate term details of the contract of the candidate while submit a timesheet
Purpose:		Get rate term details of the contract of the candidate while submit a timesheet
-----------------------------------------------------------------------------------------------------------------------------------------
Version		Date		Author			BugID		Notes
-----------------------------------------------------------------------------------------------------------------------------------------
1.0			07.01.2016  Kavitha.S	    MG-12114 Timesheet App - MG AP

-----------------------------------------------------------------------------------------------------------------------------------------*/
CREATE PROC [dbo].[UspGetRateTermDetails_TSAPP]
( 
	@AgreementId int,
	@TimesheetID int =0,
	@StartDate datetime=null,
	@Enddate datetime=null,
	@TimesheetStatus varchar(10)='None'
)

AS 
SET NOCOUNT ON                                                     
              
BEGIN   

IF @TimesheetStatus ='Submitted'
BEGIN

			SELECT

			CASE
				WHEN [TimeSheetDetail].InvoiceCodeid IS NULL
				THEN [TimeSheetDetail].[ContractProjectPoID]
			ELSE [TimeSheetDetail].InvoiceCodeid
			END AS ContractProjectPOId,
				[TimeSheetDetail].[ContractRateID],
			CASE
				WHEN [TimeSheetDetail].InvoiceCodeid IS NULL
				THEN CONVERT(VARCHAR(10),[TimeSheetDetail].[ContractProjectPoID]) +  '_' + CONVERT(VARCHAR(10),[TimeSheetDetail].[ContractRateID])
			ELSE CONVERT(VARCHAR(10),[TimeSheetDetail].[InvoiceCodeid]) +  '_' + CONVERT(VARCHAR(10),[TimeSheetDetail].[ContractRateID])
			END AS ProjectPORateList,
			CRD.PrimaryRateTerm,CRD.RateDescription,CRD.HoursPerDay,CRD.BillRate,CRD.PayRate
			FROM [TimeSheetDetail]
				INNER JOIN [TimeSheet] ON [TimeSheet].[TimeSheetID] = [TimeSheetDetail].[TimesheetID]
				LEFT JOIN [agreement_contractratedetail] CRD ON (CRD.agreementid = [TimeSheet].AgreementID AND CRD.agreementid = @AgreementID and CRD.inactive = 0
																and CRD.startdate <= @StartDate
																and CRD.enddate >= @EndDate)
			WHERE [TimeSheet].[Inactive] = 0
				AND TimesheetDetail.TimesheetID =  @timesheetid
			GROUP BY [TimeSheetDetail].[ContractProjectPoID], [TimeSheetDetail].[ContractRateID],[TimeSheetDetail].InvoiceCodeid,
						CRD.PrimaryRateTerm,CRD.RateDescription,CRD.HoursPerDay,CRD.BillRate,CRD.PayRate

END

ELSE If @TimesheetStatus='Saved'
BEGIN
		SELECT

			CASE
				WHEN [TimeSheetDetailTemp].InvoiceCodeid IS NULL
				THEN [TimeSheetDetailTemp].[ContractProjectPoID]
			ELSE [TimeSheetDetailTemp].InvoiceCodeid
			END AS ContractProjectPOId,
				[TimeSheetDetailTemp].[ContractRateID],
			CASE
				WHEN [TimeSheetDetailTemp].InvoiceCodeid IS NULL
				THEN CONVERT(VARCHAR(10),[TimeSheetDetailTemp].[ContractProjectPoID]) +  '_' + CONVERT(VARCHAR(10),[TimeSheetDetailTemp].[ContractRateID])
			ELSE CONVERT(VARCHAR(10),[TimeSheetDetailTemp].[InvoiceCodeid]) +  '_' + CONVERT(VARCHAR(10),[TimeSheetDetailTemp].[ContractRateID])
			END AS ProjectPORateList,
			CRD.PrimaryRateTerm,CRD.RateDescription,CRD.HoursPerDay,CRD.BillRate,CRD.PayRate
			FROM [TimeSheetDetailTemp]
				INNER JOIN [TimeSheetTemp] ON [TimeSheetTemp].[TimeSheetTempID] = [TimeSheetDetailTemp].[TimesheetTempID]
				LEFT JOIN [agreement_contractratedetail] CRD ON (CRD.agreementid = [TimeSheetTemp].AgreementID AND CRD.agreementid = @AgreementID and CRD.inactive = 0
																and CRD.startdate <= @StartDate
																and CRD.enddate >= @EndDate)
		WHERE [TimeSheetTemp].[Inactive] = 0
				AND [TimeSheetDetailTemp].[Inactive] = 0
				AND TimesheetDetailTemp.TimesheetTempID = @timesheetid
				
		GROUP BY [TimeSheetDetailTemp].[ContractProjectPoID], [TimeSheetDetailTemp].[ContractRateID],[TimeSheetDetailTemp].InvoiceCodeid,
					CRD.PrimaryRateTerm,CRD.RateDescription,CRD.HoursPerDay,CRD.BillRate,CRD.PayRate
END
ELSE
BEGIN
	SELECT
	
	CRD.ContractRateID,CRD.PrimaryRateTerm,CRD.RateDescription,CRD.HoursPerDay,CRD.BillRate,CRD.PayRate
	from agreement_contractratedetail CRD
	where CRD.agreementid = @AgreementID
	and CRD.inactive = 0
	and CRD.startdate <= @StartDate
	and CRD.enddate >= @EndDate
END



END 


GO





/*
	**************************************Create Get Project PO Rate Details*******************************
*/

USE [MatchGuideDev]
GO

/****** Object:  StoredProcedure [dbo].[UspGetProjectPORateDetails_TSAPP]    Script Date: 2/10/2016 2:21:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*---------------------------------------------------------------------------------------------------------------------------------------
Name:			[UspGetProjectPORateDetails_TSAPP]
Description:	To get Project/Po details with RateTerm
Purpose:		In what context this stored procedure will be used?
-----------------------------------------------------------------------------------------------------------------------------------------
Version		Date		Author			BugID		Notes
-----------------------------------------------------------------------------------------------------------------------------------------
1.0			11.01.2016  Kavitha.S	    MG-12114 Timesheet App - MG AP	
-----------------------------------------------------------------------------------------------------------------------------------------*/
CREATE PROC [dbo].[UspGetProjectPORateDetails_TSAPP]
( 
	@contractProjectPOIDList VARCHAR(1000),
	@contractRateTermIDList  VARCHAR(1000),
	@verticalid int
)

AS 
SET NOCOUNT ON                                                     
              
BEGIN    

 select   
  contractprojectpo.agreementid [AgreementId],  
  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cpm.companyprojectid   
    from companypoprojectmatrix cpm   
     inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
	and cpm.verticalid = @verticalid
	)  
   else companyproject.companyprojectid   
   end  
  ) [ProjectId], 
  
  Null as EinvoiceId, 

  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cp.description   
    from companypoprojectmatrix cpm   
     inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid  
	and cpm.verticalid = @verticalid
    )  
   else companyproject.[Description]  
   end  
  ) [ProjectDescription],  

  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cp.projectid   
    from companypoprojectmatrix cpm   
     inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid  
	and cpm.verticalid = @verticalid
    )  
   else companyproject.[projectid]  
   end  
  ) [DisplayProjectID], 

	( case when contractprojectpo.companypoprojectmatrixid is not null 
		then ( select cp.CompanyProjectID 
			from companypoprojectmatrix cpm 
				inner join companyproject cp on cp.companyprojectid = cpm.companyprojectid
			where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid
			and cpm.verticalid = @verticalid
			)
		else companyproject.[CompanyProjectID]
		end
	) [CompanyProjectID],			

  [ContractProjectPO].[ContractProjectPOID],  

  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cpm.companypoid   
    from companypoprojectmatrix cpm   
     inner join companypo cpo on cpo.companypoid = cpm.companypoid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid  
	and cpm.verticalid = @verticalid
    )  
   else companypo.companypoid   
   end  
  ) [POId],  

  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cpo.description   
    from companypoprojectmatrix cpm   
     inner join companypo cpo on cpo.companypoid = cpm.companypoid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid  
	and cpm.verticalid = @verticalid
    )  
   else companypo.[Description]   
   end  
  ) [PODescription],  

  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cpo.PONumber   
    from companypoprojectmatrix cpm   
     inner join companypo cpo on cpo.companypoid = cpm.companypoid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid  
	and cpm.verticalid = @verticalid
    )  
   else companypo.PONumber  
   end  
  ) DisplayPONumber,  

  ( case when contractprojectpo.companypoprojectmatrixid is not null   
   then ( select cpo.ponumber  
    from companypoprojectmatrix cpm   
     inner join companypo cpo on cpo.companypoid = cpm.companypoid  
    where cpm.companypoprojectmatrixid = companypoprojectmatrix.companypoprojectmatrixid  
	and cpm.verticalid = @verticalid
    )  
   else companypo.PONumber   
   end  
  ) [PONumber],  
  companypoprojectmatrix.companypoprojectmatrixid ,
  contractrateid,
  primaryrateterm,
  ratedescription,
  '$ ' + CONVERT(Varchar(10),PayRate) AS rateAmount,
  p.Title AS ratetype,
  0 as EinvoiceType  
  

 from contractprojectpo  
  LEFT JOIN companypoprojectmatrix on companypoprojectmatrix.companypoprojectmatrixid = contractprojectpo.companypoprojectmatrixid  
  LEFT JOIN companyproject on contractprojectpo.companyprojectid = companyproject.companyprojectid  
  LEFT JOIN companypo on companypo.companypoid =contractprojectpo.companypoid
  LEFT JOIN agreement_contractratedetail ac on ac.agreementid=contractprojectpo.agreementid
									and contractrateid in(select val from dbo.getsplit(@contractRateTermIDList,','))
  LEFT JOIN picklist p ON p.picklistid = ac.RateTermType
 where   
 contractprojectpo.inactive = 0  
 and [ContractProjectPO].[ContractProjectPOID] in(select val from dbo.getsplit(@contractProjectPOIDList,','))
									
UNION ALL

Select 
	Con.contractId as AgreementId,
	Null as ProjectId,
	Con.InvoiceCodeId as EinvoiceId,
	Con.InvoiceCodeText as ProjectDescription,
	Con.InvoiceCodeText as DisplayProjectId,
	Null as CompanyProjectId,
	Null as ContractProjectPoid,
	Null as POID,
	Null as PODescription,
	Null as DisplayPOnumber,
	Null as POnumber,
	Null as CompanyPoProjectMatrixId,
	contractrateid,
	primaryrateterm,
	ratedescription,
	'$ ' + CONVERT(Varchar(10),PayRate) AS rateAmount,
	p.Title AS ratetype,
	1 as EinvoiceType

	from ContractInvoiceCode Con
	inner join Agreement on Agreement.AgreementId=Con.ContractId
	LEFT JOIN agreement_contractratedetail ac on ac.agreementid=con.ContractId and 
									 contractrateid in(select val from dbo.getsplit(@contractRateTermIDList,','))
     LEFT JOIN picklist p ON p.picklistid = ac.RateTermType
	where Con.isactive=1
	and Con.InvoiceCodeId  in(select val from dbo.getsplit(@contractProjectPOIDList,','))
		
	
	
END 


GO




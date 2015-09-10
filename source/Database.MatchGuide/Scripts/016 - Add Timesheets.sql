SET IDENTITY_INSERT [dbo].[TimeSheetAvailablePeriod] ON;
INSERT INTO [dbo].[TimeSheetAvailablePeriod]
           ([TimeSheetAvailablePeriodID]
		   ,[TimeSheetAvailablePeriodStartDate]
           ,[TimeSheetAvailablePeriodEndDate])
     VALUES
           (1, '2015-09-01 00:00:00.000', '2015-09-30 00:00:00.000')
		   ,(2, '2015-08-01 00:00:00.000', '2015-08-15 00:00:00.000')
		   ,(3, '2015-07-01 00:00:00.000', '2015-07-31 00:00:00.000')
		   ,(4, '2015-07-01 00:00:00.000', '2015-07-15 00:00:00.000')
		   ,(5, '2015-07-16 00:00:00.000', '2015-07-31 00:00:00.000')
		   ,(6, '2015-06-01 00:00:00.000', '2015-06-30 00:00:00.000')
		   ,(7, '2015-06-01 00:00:00.000', '2015-06-15 00:00:00.000')
		   ,(8, '2015-06-16 00:00:00.000', '2015-06-30 00:00:00.000');

SET IDENTITY_INSERT [dbo].[TimeSheetAvailablePeriod] OFF;

INSERT INTO [dbo].[TimeSheet]
           ([CandidateUserID]
           ,[AgreementID]
           ,[TimeSheetAvailablePeriodID]
           ,[StatusID]
		   ,[DirectReportUserId])
     VALUES
			--Candice, Jul 1 - 31, Submitted, Bob Smith (Direct Report)
            (12, 4, 3, 622, 1)
		    --Candice, Aug 1 - 31, Open, Bob Smith (Direct Report)
		   ,(12, 4, 1, 621, 1)
		   --Tommy, Jul 1 - 31, Approved, Bob Smith (Direct Report)
		   ,(10, 1, 3, 624, 1)
		   --Candice, June 1 - 15, Approved, Bob Smith (Direct Report)
		   ,(12, 1, 7, 624, 1)
		   --Candice, Jul 16 - 30, Approved, Bob Smith (Direct Report)
		   ,(12, 1, 8, 624, 1);

INSERT INTO [dbo].[TimeSheetDetail]
           ([ContractRateID]
           ,[PONumber]
           ,[ProjectID]
           ,[Day]
           ,[UnitValue]
           ,[Description]
           ,[TimesheetID])
     VALUES
			--Candice's July Entries
           (4, null, null, 1, 8, null, 1)
		   ,(4, null, null, 2, 8, null, 1)
		   ,(4, null, null, 3, 8, null, 1)
		   ,(4, null, null, 6, 8, null, 1)
		   ,(4, null, null, 7, 8, null, 1)
		   ,(4, null, null, 8, 8, null, 1)
		   ,(4, null, null, 9, 8, null, 1)
		   ,(4, null, null, 10, 8, null, 1)
		   ,(4, null, null, 13, 8, null, 1)
		   ,(4, null, null, 14, 8, null, 1)
		   ,(4, null, null, 15, 8, null, 1)
		   ,(4, null, null, 16, 8, null, 1)
		   ,(4, null, null, 17, 8, null, 1)
		   ,(4, null, null, 20, 8, null, 1)
		   ,(4, null, null, 21, 8, null, 1)
		   ,(4, null, null, 22, 8, null, 1)
		   ,(4, null, null, 23, 8, null, 1)
		   ,(4, null, null, 24, 8, null, 1)
		   ,(4, null, null, 27, 8, null, 1)
		   ,(4, null, null, 28, 8, null, 1)
		   ,(4, null, null, 29, 8, null, 1)
		   ,(4, null, null, 30, 8, null, 1)
		   ,(4, null, null, 31, 8, null, 1)
		   --Tommy's July Entries
		   ,(5, null, null, 1, 8, null, 3)
		   ,(5, null, null, 2, 8, null, 3)
		   ,(5, null, null, 3, 8, null, 3)
		   ,(5, null, null, 6, 8, null, 3)
		   ,(5, null, null, 7, 8, null, 3)
		   ,(5, null, null, 8, 8, null, 3)
		   ,(5, null, null, 9, 8, null, 3)
		   ,(5, null, null, 10, 8, null, 3)
		   ,(5, null, null, 13, 8, null, 3)
		   ,(5, null, null, 14, 8, null, 3)
		   ,(5, null, null, 15, 8, null, 3)
		   ,(5, null, null, 16, 8, null, 3)
		   ,(5, null, null, 17, 8, null, 3)
		   ,(5, null, null, 20, 8, null, 3)
		   ,(5, null, null, 21, 8, null, 3)
		   ,(5, null, null, 22, 8, null, 3)
		   ,(5, null, null, 23, 8, null, 3)
		   ,(5, null, null, 24, 8, null, 3)
		   ,(5, null, null, 27, 8, null, 3)
		   ,(5, null, null, 28, 8, null, 3)
		   ,(5, null, null, 29, 8, null, 3)
		   ,(5, null, null, 30, 8, null, 3)
		   ,(5, null, null, 31, 8, null, 3)
		   -- First half of June's TimeEntries
		   ,(5, null, null, 1, 8, null, 4)
		   ,(5, null, null, 2, 8, null, 4)
		   ,(5, null, null, 3, 8, null, 4)
		   ,(5, null, null, 4, 8, null, 4)
		   ,(5, null, null, 5, 8, null, 4)
		   ,(5, null, null, 8, 8, null, 4)
		   ,(5, null, null, 9, 8, null, 4)
		   ,(5, null, null, 10, 8, null, 4)
		   ,(5, null, null, 11, 8, null, 4)
		   ,(5, null, null, 12, 8, null, 4)
		   ,(5, null, null, 15, 8, null, 4)
		   -- Second half of June's TimeEntries
		   ,(5, null, null, 16, 8, null, 5)
		   ,(5, null, null, 17, 8, null, 5)
		   ,(5, null, null, 18, 8, null, 5)
		   ,(5, null, null, 19, 8, null, 5)
		   ,(5, null, null, 21, 8, null, 5)
		   ,(5, null, null, 22, 8, null, 5)
		   ,(5, null, null, 23, 8, null, 5)
		   ,(5, null, null, 24, 8, null, 5)
		   ,(5, null, null, 25, 8, null, 5)
		   ,(5, null, null, 28, 8, null, 5)
		   ,(5, null, null, 29, 8, null, 5)
		   ,(5, null, null, 30, 8, null, 5);
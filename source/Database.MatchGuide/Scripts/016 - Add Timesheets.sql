SET IDENTITY_INSERT [dbo].[TimeSheetAvailablePeriod] ON;
INSERT INTO [dbo].[TimeSheetAvailablePeriod]
           ([TimeSheetAvailablePeriodID]
		   ,[TimeSheetAvailablePeriodStartDate]
           ,[TimeSheetAvailablePeriodEndDate])
     VALUES
           (1, '2015-09-01 00:00:00.000', '2015-09-30 00:00:00.000')
		   ,(2, '2015-09-01 00:00:00.000', '2015-09-15 00:00:00.000')
		   ,(3, '2015-08-01 00:00:00.000', '2015-08-31 00:00:00.000')
		   ,(4, '2015-08-01 00:00:00.000', '2015-08-15 00:00:00.000')
		   ,(5, '2015-08-16 00:00:00.000', '2015-08-31 00:00:00.000')
		   ,(6, '2015-07-01 00:00:00.000', '2015-07-30 00:00:00.000')
		   ,(7, '2015-07-01 00:00:00.000', '2015-07-15 00:00:00.000')
		   ,(8, '2015-07-16 00:00:00.000', '2015-07-31 00:00:00.000');

SET IDENTITY_INSERT [dbo].[TimeSheetAvailablePeriod] OFF;

INSERT INTO [dbo].[TimeSheet]
           ([CandidateUserID]
           ,[AgreementID]
           ,[TimeSheetAvailablePeriodID]
           ,[StatusID]
		   ,[DirectReportUserId])
     VALUES
			--Candice, Aug 1 - 31, Submitted, Bob Smith (Direct Report)
            (12, 1, 3, 622, 1)
		    --Candice, Sep 1 - 31, Open, Bob Smith (Direct Report)
		   ,(12, 4, 1, 621, 1)
		   --Tommy, Aug 1 - 31, Approved, Bob Smith (Direct Report)
		   ,(10, 1, 3, 624, 1)
		   --Candice, Jul 1 - 15, Approved, Bob Smith (Direct Report)
		   ,(12, 1, 7, 624, 1)
		   --Candice, Jul 16 - 31, Approved, Bob Smith (Direct Report)
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
			--Candice's Aug Entries
           (1, 'PC126', null, 3, 8, null, 1)
		   ,(1, 'PC126', null, 4, 8, null, 1)
		   ,(1, 'PC126', null, 5, 8, null, 1)
		   ,(1, 'PC126', null, 6, 8, null, 1)
		   ,(1, 'PC126', null, 7, 8, null, 1)
		   ,(1, 'PC126', null, 10, 8, null, 1)
		   ,(1, 'PC126', null, 11, 8, null, 1)
		   ,(1, 'PC126', null, 12, 8, null, 1)
		   ,(1, 'PC126', null, 13, 8, null, 1)
		   ,(1, 'PC126', null, 14, 8, null, 1)
		   ,(1, 'PC126', null, 17, 8, null, 1)
		   ,(1, 'PC126', null, 18, 8, null, 1)
		   ,(1, 'PC126', null, 19, 8, null, 1)
		   ,(1, 'PC126', null, 20, 8, null, 1)
		   ,(1, 'PC126', null, 21, 8, null, 1)
		   ,(1, 'PC126', null, 24, 8, null, 1)
		   ,(1, 'PC126', null, 25, 8, null, 1)
		   ,(1, 'PC126', null, 26, 8, null, 1)
		   ,(1, 'PC126', null, 27, 8, null, 1)
		   ,(1, 'PC126', null, 28, 8, null, 1)
		   ,(1, 'PC126', null, 31, 8, null, 1)
		   --Tommy's Aug Entries
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
		   -- First half of Jul's TimeEntries
		   ,(1, 'PC123', null, 1, 8, null, 4)
		   ,(1, 'PC123', null, 2, 8, null, 4)
		   ,(1, 'PC123', null, 3, 8, null, 4)
		   ,(1, 'PC123', null, 6, 8, null, 4)
		   ,(1, 'PC123', null, 7, 8, null, 4)
		   ,(1, 'PC123', null, 8, 8, null, 4)
		   ,(1, 'PC123', null, 9, 8, null, 4)
		   ,(1, 'PC123', null, 10, 8, null, 4)
		   ,(1, 'PC123', null, 13, 8, null, 4)
		   ,(1, 'PC123', null, 14, 8, null, 4)
		   ,(1, 'PC123', null, 15, 8, null, 4)
		   -- Second half of Jul's TimeEntries
		   ,(8, 'PC123', null, 16, 4, null, 5)
		   ,(8, 'PC123', null, 17, 4, null, 5)
		   ,(8, 'PC123', null, 20, 4, null, 5)
		   ,(8, 'PC123', null, 21, 4, null, 5)
		   ,(8, 'PC123', null, 22, 4, null, 5)
		   ,(8, 'PC123', null, 23, 4, null, 5)
		   ,(8, 'PC123', null, 24, 4, null, 5)
		   ,(8, 'PC123', null, 27, 4, null, 5)
		   ,(8, 'PC123', null, 28, 4, null, 5)
		   ,(8, 'PC123', null, 29, 4, null, 5)
		   ,(8, 'PC123', null, 30, 4, null, 5)
		   ,(8, 'PC123', null, 31, 4, null, 5)
		   ,(1, 'PC126', null, 16, 4, null, 5)
		   ,(1, 'PC126', null, 17, 4, null, 5)
		   ,(1, 'PC126', null, 20, 4, null, 5)
		   ,(1, 'PC126', null, 21, 4, null, 5)
		   ,(1, 'PC126', null, 22, 4, null, 5)
		   ,(1, 'PC126', null, 23, 4, null, 5)
		   ,(1, 'PC126', null, 24, 4, null, 5)
		   ,(1, 'PC126', null, 27, 4, null, 5)
		   ,(1, 'PC126', null, 28, 4, null, 5)
		   ,(1, 'PC126', null, 29, 4, null, 5)
		   ,(1, 'PC126', null, 30, 4, null, 5)
		   ,(1, 'PC126', null, 31, 4, null, 5);
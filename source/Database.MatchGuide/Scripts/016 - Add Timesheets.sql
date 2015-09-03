INSERT INTO [dbo].[TimeSheetAvailablePeriod]
           ([TimeSheetAvailablePeriodStartDate]
           ,[TimeSheetAvailablePeriodEndDate])
     VALUES
           ('2015-08-01 00:00:00.000', '2015-08-31 00:00:00.000')
		   ,('2015-08-01 00:00:00.000', '2015-08-15 00:00:00.000')
		   ,('2015-07-01 00:00:00.000', '2015-07-31 00:00:00.000')
		   ,('2015-07-01 00:00:00.000', '2015-07-15 00:00:00.000')
		   ,('2015-07-16 00:00:00.000', '2015-07-31 00:00:00.000');



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
		   ,(10, 1, 3, 624, 1);

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
		   ,(5, null, null, 31, 8, null, 3);
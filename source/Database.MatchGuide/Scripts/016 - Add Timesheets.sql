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
           ,[StatusID])
     VALUES
			--Candice, Jul 1 - 31, Submitted
            (12, 4, 3, 622)
		    --Candice, Aug 1 - 31, Open
		   ,(12, 4, 1, 621);

INSERT INTO [dbo].[TimeSheetDetail]
           ([ContractRateID]
           ,[PONumber]
           ,[ProjectID]
           ,[Day]
           ,[UnitValue]
           ,[Description]
           ,[TimesheetID])
     VALUES
           (4, null, null, 3, 8, null, 2)
		   ,(4, null, null, 4, 8, null, 2)
		   ,(4, null, null, 5, 8, null, 2)
		   ,(4, null, null, 6, 8, null, 2)
		   ,(4, null, null, 7, 8, null, 2)
		   ,(4, null, null, 10, 8, null, 2)
		   ,(4, null, null, 11, 8, null, 2)
		   ,(4, null, null, 12, 8, null, 2)
		   ,(4, null, null, 13, 8, null, 2)
		   ,(4, null, null, 14, 8, null, 2)
		   ,(4, null, null, 17, 8, null, 2)
		   ,(4, null, null, 18, 8, null, 2)
		   ,(4, null, null, 19, 8, null, 2)
		   ,(4, null, null, 20, 8, null, 2)
		   ,(4, null, null, 21, 8, null, 2)
		   ,(4, null, null, 24, 8, null, 2)
		   ,(4, null, null, 25, 8, null, 2)
		   ,(4, null, null, 26, 8, null, 2)
		   ,(4, null, null, 27, 8, null, 2)
		   ,(4, null, null, 28, 8, null, 2)
		   ,(4, null, null, 31, 8, null, 2);
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
			--Candice, Jul 1 - 31, Open
            (12, 4, 3, 622)
		    --Candice, Jul 1 - 31, Open
		   ,(12, 4, 1, 621);
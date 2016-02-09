/*
   **************** FirstDayOfMonth *******************
*/

CREATE FUNCTION [dbo].[FirstDayOfMonth]
  ( @Date DATETIME)
RETURNS datetime
AS
BEGIN

  RETURN (CAST(STR(MONTH(@Date))+'/'+STR(01)+'/'+STR(YEAR(@Date)) AS DateTime))

END

GO

/*
   **************** LastDayOfMonth *******************
*/


CREATE FUNCTION [dbo].[LastDayOfMonth]
  ( @Date DATETIME)
RETURNS datetime
AS
BEGIN


	RETURN (CASE WHEN MONTH(@Date)= 12
	THEN DATEADD(day, -1, CAST('01/01/' + STR(YEAR(@Date)+1) AS DateTime))
	ELSE DATEADD(day, -1, CAST(STR(MONTH(@Date)+1) + '/01/' + STR(YEAR(@Date)) AS DateTime))
	END)

END

GO
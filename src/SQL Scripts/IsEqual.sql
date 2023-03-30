CREATE FUNCTION [dbo].[IsEqual]
(
    @value1 SQL_VARIANT,
    @value2 SQL_VARIANT
)
RETURNS BIT
AS
BEGIN

    RETURN (CASE
                WHEN (ISNULL(NULLIF(@value1, @value2), NULLIF(@value2, @value1)) IS NULL) THEN
                    1
                ELSE
                    0
            END
           );

END;


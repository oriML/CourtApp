CREATE PROCEDURE GetMonthlyContactsReport
    @Year INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH CurrentMonth AS (
        SELECT 
            DepartmentId,
            COUNT(*) AS TotalContacts
        FROM Contact
        WHERE YEAR(CreatedAt) = @Year 
          AND MONTH(CreatedAt) = @Month
        GROUP BY DepartmentId
    ),
    PreviousMonth AS (
        SELECT 
            DepartmentId,
            COUNT(*) AS TotalContacts
        FROM Contact
        WHERE YEAR(CreatedAt) = CASE WHEN @Month = 1 THEN @Year - 1 ELSE @Year END
          AND MONTH(CreatedAt) = CASE WHEN @Month = 1 THEN 12 ELSE @Month - 1 END
        GROUP BY DepartmentId
    ),
    LastYearSameMonth AS (
        SELECT 
            DepartmentId,
            COUNT(*) AS TotalContacts
        FROM Contact
        WHERE YEAR(CreatedAt) = @Year - 1 
          AND MONTH(CreatedAt) = @Month
        GROUP BY DepartmentId
    )
    SELECT 
        d.DepartmentId,
        d.DepartmentName,
        ISNULL(c.TotalContacts, 0) AS CurrentMonthContacts,
        ISNULL(p.TotalContacts, 0) AS PreviousMonthContacts,
        ISNULL(l.TotalContacts, 0) AS LastYearContacts,
        (ISNULL(c.TotalContacts, 0) - ISNULL(p.TotalContacts, 0)) AS DiffFromPreviousMonth,
        (ISNULL(c.TotalContacts, 0) - ISNULL(l.TotalContacts, 0)) AS DiffFromLastYear
    FROM Departments d
    LEFT JOIN CurrentMonth c ON d.DepartmentId = c.DepartmentId
    LEFT JOIN PreviousMonth p ON d.DepartmentId = p.DepartmentId
    LEFT JOIN LastYearSameMonth l ON d.DepartmentId = l.DepartmentId
    ORDER BY d.DepartmentName;
END

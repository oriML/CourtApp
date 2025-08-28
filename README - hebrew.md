# פרויקט ניהול פניות

## תיאור כללי
הפרויקט מציע מערכת מלאה לניהול פניות הכוללת **API ב-ASP.NET Core (.NET C#)** עם חיבור ל-PostgreSQL, וכן ממשק **Angular 19** מותאם לנייד עם Material Design.  
המערכת מאפשרת: ניהול פניות (CRUD), לוגין עם JWT, הצגת דוחות חודשיים וארכיטקטורה נקייה לפי עקרון SOC.

---

## רכיבי הפרויקט
### Backend (.NET 8 Web API)
- **ASP.NET Core Web API** – פיתוח API מודולרי ומופרד לפי שכבות (Controllers, Services, Repositories).
- **Entity Framework Core** – גישה ל-DB.
- **PostgreSQL** – בסיס נתונים בענן (Render).
- **FluentValidation** – ולידציות בצד השרת.
- **JWT Authentication** – אבטחת גישה ל-API.
- **Concurrency Handling** – שימוש ב-RowVersion / Timestamp.
- **Unit Tests** – בדיקות בסיסיות עם xUnit.

### Frontend (Angular 19)
- **Angular** – ארכיטקטורה לפי חלוקה ל-core, shared, features.
- **Angular Material** – UI מודרני, מותאם לנייד, צבעי לבן ותכלת רגועים.
- **JWT Local Storage** – שמירת טוקן וניהול session.
- **מודולים וקומפוננטות**:
  - דף לוגין.
  - רשימת פניות (כולל הוספה, עריכה, מחיקה).
  - מודאל להצגת פרטי פנייה ועריכת סטטוס.
  - דוח חודשי עם גרף/טבלה.
- **Snackbar Service** – הודעות עם אייקונים מותאמים (הצלחה, שגיאה, מחיקה וכו’).

---

## יתרונות / חסרונות של הטכנולוגיות
- **.NET Core + Angular** – שילוב נפוץ עם תמיכה רחבה בקהילה וסקיילביליות גבוהה.
- **PostgreSQL** – אמין, תומך ב-JSON, גמיש ונפוץ בענן.  
  חסרון: פחות אינטגרציה טבעית עם כלי Microsoft לעומת SQL Server.
- **FluentValidation** – מפריד לוגיקה של ולידציה בצורה נקייה.  
- **JWT** – מאפשר אבטחה מבוזרת אך דורש ניהול נכון של תוקף הטוקן ורענון.

---

## טיפול באבטחה
- אימות משתמשים עם JWT (כולל Claims).
- שימוש ב-HTTPS בלבד.
- Cookie HttpOnly + SameSite במידת הצורך.
- בדיקות ולידציה קפדניות עם FluentValidation.

---

## טיפול בשגיאות
- שימוש ב-Middleware גלובלי ל-Exception Handling.
- החזרת תשובות בפורמט עקבי (ErrorResponse).
- לוגים ב-Serilog.

---

## מנגנוני קישור
- **Backend → DB**: חיבור EF Core ל-PostgreSQL עם Connection String.
- **Frontend → Backend**: שימוש ב-HttpClient עם Interceptor להוספת JWT.
- **CORS**: מוגדר ב-API לאפשר גישה מכתובות ספציפיות בלבד (למשל `http://localhost:4200`).

---

## הוראות התקנה והרצה
### דרישות מוקדמות
- Docker / Docker Compose מותקן.
- Node.js 20+ מותקן.
- .NET 8 SDK מותקן.

### התקנה והרצה
1. שכפל את הריפוזיטורי:
   ```bash
   git clone <repo-url>
   ```

2. הקם את ה-Backend:
   ```bash
   cd backend
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

3. הקם את ה-Frontend:
   ```bash
   cd frontend
   npm install
   ng serve -o
   ```

4. שימוש ב-Docker (אופציונלי):
   ```bash
   docker-compose up --build
   ```

---

## נקודות לשיפור עתידי
- הוספת הרשמה (Register) עם אימות מייל.
- תמיכה ברענון טוקנים (Refresh Tokens).
- הרחבת הדוחות (פילוחים לפי מחלקה, SLA).

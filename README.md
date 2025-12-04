Birahe – Online Riddle Contest Platform

Birahe is an online riddle contest platform that allows users to participate in timed contests, solve riddles, earn rewards, and track their progress on a dynamic leaderboard. The platform includes both user-facing features and an admin panel for managing contests and users.

Features

User Features
- User Registration & Login with JWT-based authentication.
- Contest Participation:
  - Open riddles and attempt answers.
  - View hints after opening them.
  - Submit answers with rate-limiting to prevent abuse.
  - Earn rewards for correct answers.
- Dynamic Leaderboard showing top users.
- User Balance Tracking to monitor earned points/rewards.
- Secure File Handling for reward and hint images.

Admin Features
- Riddle Management:
  - Add, edit, delete riddles.
  - Upload hint and reward images.
- User Management:
  - View all users.
  - Ban/unban users.
  - Edit usernames and passwords.
- Contest Configuration:
  - Set contest start time and manage contest rules.

Technologies

- Backend: C#, ASP.NET Web Api, Entity Framework Core
- Authentication & Security: JWT, Role-Based Access Control
- Database: Entity Framework Core with a relational database
- API Design: RESTful API endpoints returning JSON and files
- Caching: Leaderboard caching for performance

Architecture & Design

- Service Result Pattern for standardized API responses (MapServiceResult and MapImageServiceResult).
- Dependency Injection for maintainable and testable backend services.
- Rate Limiting implemented per user per riddle to ensure fair play.
- File Upload & Download for riddle images securely.

Getting Started

Prerequisites:
- .NET 8 SDK
- SQL Server / PostgreSQL / or any EF Core-supported database
- Optional: Postman or Swagger for API testing

Setup:
1. Clone the repository:
   git clone https://github.com/hosseinbelbasi/Birahe.git
   cd Birahe
2. Configure database connection in appsettings.json
3. Apply migrations:
   dotnet ef database update
4. Run the project:
   dotnet run
5. Open Swagger at https://localhost:{port}/swagger to explore APIs

Contributing

Contributions are welcome! Please fork the repository and submit a pull request with a clear description of your changes.


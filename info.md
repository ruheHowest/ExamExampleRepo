# Db-first scaffolding with EF Core 10.0
Use this command to generate student and course entities
```bash
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=school;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -p ExamExamplesRepo.Infrastruture/ExamExamplesRepo.Infrastruture.csproj -s ExamExamplesRepo.Infrastruture/ExamExamplesRepo.Infrastruture.csproj -o Models -c SchoolDbContext
```
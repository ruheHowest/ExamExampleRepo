var builder = DistributedApplication.CreateBuilder(args);


var sqlServer = builder
    .AddSqlServer("sqlserver")
    .AddDatabase("SchoolDb", "School");

var api = builder.AddProject<Projects.ExamExamplesRepo>("school-api")
    .WithReference(sqlServer);

builder.Build().Run();

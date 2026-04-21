var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.ExamExamplesRepo>("school-api");

builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ExamExamplesRepo>("examexamplesrepo");

builder.Build().Run();

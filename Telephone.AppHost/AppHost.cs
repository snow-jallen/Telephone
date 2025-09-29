var builder = DistributedApplication.CreateBuilder(args);

var aidan = builder.AddProject<Projects.AidanApi>("aidanapi");

var omner = builder.AddProject<Projects.OmnerApi>("omnerapi")
    .WithReference(aidan);

var kaden = builder.AddProject<Projects.KadenApi>("kadenapi")
    .WithReference(omner);

builder.Build().Run();

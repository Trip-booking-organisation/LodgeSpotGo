using JetSetGo.AccommodationManagement.Api;
using JetSetGo.AccommodationManagement.Application;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpContextAccessor();
    builder.Services
        .AddPresentation(builder.Configuration)
        .AddApplication();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors(app.Configuration
        .GetSection("Cors")
        .GetSection("PolicyName").Value!);
    app.UseHttpsRedirection();
    app.MapEndpoints();
    app.Run(); 
}
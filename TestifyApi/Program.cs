using Microsoft.EntityFrameworkCore;
using Testify.API.Extensions;
using Testify.Application.Extensions;
using Testify.Domain.Entities;
using Testify.Infrastructure.Extensions;
using Testify.Infrastructure.Persistance;
using Testify.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<UserSeeder>();
builder.Services.AddScoped<QuizSeeder>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TestifyDbContext>();
    db.Database.Migrate();
    var seeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
    var quizSeeder = scope.ServiceProvider.GetRequiredService<QuizSeeder>();
    await seeder.SeedUsersAsync();
    await quizSeeder.SeedQuizzesAsync();
}

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseStaticFiles();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity API");
        c.SwaggerEndpoint("/swagger/Quizzes/swagger.json", "Quiz API");
        c.SwaggerEndpoint("/swagger/Questions/swagger.json", "Question API");
        c.SwaggerEndpoint("/swagger/QuizAttempts/swagger.json", "QuizAttempt API");
        c.SwaggerEndpoint("/swagger/UserProfile/swagger.json", "UserProfile API");
        c.SwaggerEndpoint("/swagger/Enums/swagger.json", "Enum API");
        c.InjectStylesheet("/custom.css");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();

public partial class Program { }
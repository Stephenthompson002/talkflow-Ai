using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TalkFlow.Api.Data;
using TalkFlow.Api.Hubs;
using TalkFlow.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// NOTE: The line 'public partial class Program { }' has been removed from here 
// to resolve the compilation error on 'var builder = WebApplication.CreateBuilder(args);'.
// You must now ensure your test project can access the application's internals 
// by adding an InternalsVisibleTo attribute to your main project's .csproj file.

var builder = WebApplication.CreateBuilder(args);

// Database Context with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddSingleton<IAudioService, WhisperService>();
builder.Services.AddScoped<IIntentService, GptIntentService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=postgres;Database=talkflow;Username=talkflow_user;Password=password",
        name: "postgres",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "database", "ready" }
    );

// SignalR, Controllers, Swagger
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
var jwtKey = builder.Configuration["JWT_SECRET_KEY"] 
    ?? "fallback-secret-key-minimum-32-characters-long";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT_ISSUER"] ?? "talkflow-api",
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT_AUDIENCE"] ?? "talkflow-app",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/chat"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure database tables are created with correct casing
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database creation warning: {ex.Message}");
        // Continue - tables might already exist
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

// Health check endpoints
app.MapHealthChecks("/health");

// Test endpoint to verify database works
app.MapGet("/api/test-db", async (AppDbContext dbContext) =>
{
    try
    {
        // Test database connection and create a sample conversation
        var conversation = new TalkFlow.Api.Models.Conversation 
        { 
            Title = "Test Conversation", 
            CustomerId = "test-customer-1" 
        };
        
        dbContext.Conversations.Add(conversation);
        await dbContext.SaveChangesAsync();
        
        return Results.Ok(new { 
            message = "Database test successful!", 
            conversationId = conversation.Id 
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database test failed: {ex.Message}");
    }
});

app.MapGet("/", () => "TalkFlow API is running!");

app.Run();
using Microsoft.EntityFrameworkCore;
using Part3_UserApi.Data;
using Part3_UserApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "server=localhost;port=3306;database=delivery_db;user=root;password=1234;";

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    try
    {
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning("База данных недоступна: {Message}", ex.Message);
    }
}

app.MapGet("/", () => Results.Ok(new
{
    service = "Part3_UserApi",
    message = "REST API пользователей работает.",
    endpoints = new[]
    {
        "POST /user",
        "GET /user/{id}",
        "GET /user",
        "PUT /user/{id}",
        "DELETE /user/{id}"
    }
}));

// POST /user
app.MapPost("/user", async (UserCreateRequest request, UserDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Login) ||
        string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new { error = "Login и Password обязательны." });
    }

    if (request.Password.Length < 6)
    {
        return Results.BadRequest(new { error = "Пароль должен содержать минимум 6 символов." });
    }

    bool exists = await db.Users.AnyAsync(x => x.Login == request.Login);
    if (exists)
    {
        return Results.Conflict(new { error = "Пользователь с таким login уже существует." });
    }

    var user = new User
    {
        Login = request.Login.Trim(),
        PassHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/user/{user.Id}", new UserResponse(user.Id, user.Login));
});

// GET /user/{id}
app.MapGet("/user/{id:int}", async (int id, UserDbContext db) =>
{
    var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    return user is null
        ? Results.NotFound(new { error = "Пользователь не найден." })
        : Results.Ok(new UserResponse(user.Id, user.Login));
});

// GET /user
app.MapGet("/user", async (UserDbContext db) =>
{
    var users = await db.Users
        .AsNoTracking()
        .OrderBy(x => x.Id)
        .Select(x => new UserResponse(x.Id, x.Login))
        .ToListAsync();

    return Results.Ok(users);
});

// PUT /user/{id}
app.MapPut("/user/{id:int}", async (int id, UserUpdateRequest request, UserDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
    if (user is null)
        return Results.NotFound(new { error = "Пользователь не найден." });

    if (!string.IsNullOrWhiteSpace(request.Login) &&
        request.Login != user.Login)
    {
        bool loginExists = await db.Users.AnyAsync(x => x.Login == request.Login);
        if (loginExists)
            return Results.Conflict(new { error = "Такой login уже занят." });

        user.Login = request.Login.Trim();
    }

    if (!string.IsNullOrWhiteSpace(request.Password))
    {
        if (request.Password.Length < 6)
            return Results.BadRequest(new { error = "Пароль должен содержать минимум 6 символов." });

        user.PassHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
    }

    await db.SaveChangesAsync();
    return Results.Ok(new UserResponse(user.Id, user.Login));
});

// DELETE /user/{id}
app.MapDelete("/user/{id:int}", async (int id, UserDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
    if (user is null)
        return Results.NotFound(new { error = "Пользователь не найден." });

    db.Users.Remove(user);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

public record UserCreateRequest(string Login, string Password);
public record UserUpdateRequest(string? Login, string? Password);
public record UserResponse(int Id, string Login);

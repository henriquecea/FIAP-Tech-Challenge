using FCG.Application.Service;
using FCG.Domain.Interface.Repository;
using FCG.Domain.Interface.Service;
using FCG.Infrastructure.Repository;
using FCG.WebAPI.Extension;

var builder = WebApplication.CreateBuilder(args);

// Serviços customizados
builder.AddSwagger();
builder.AddJwtAuthentication();
builder.AddDbContext();

// Serviços padrão ASP.NET
builder.Services.AddAuthorization();
builder.Services.AddControllers()
       .AddNewtonsoftJson();

// Injeção de dependência
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Redirecionamento da raiz para /swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
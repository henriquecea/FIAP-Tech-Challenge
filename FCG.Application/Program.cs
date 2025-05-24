using FCG.Application.Service;
using FCG.Domain.Interface;
using FCG.Infrastructure.Repository;
using FCG.WebAPI.Extension;

var builder = WebApplication.CreateBuilder(args);

// Servi�os customizados
builder.AddSwagger();
builder.AddJwtAuthentication();
builder.AddDbContext();

// Servi�os padr�o ASP.NET
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Inje��o de depend�ncia (DI)
builder.Services.AddScoped<IUserService, UserService>();
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

// Autentica��o e Autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

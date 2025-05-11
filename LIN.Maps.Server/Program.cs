global using LIN.Maps.Server;
global using LIN.Types.Responses;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
using Http.Extensions;
using LIN.Access.Auth;
using LIN.Maps.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthenticationService();
builder.Services.AddLINHttp();
builder.Services.AddMapBox(builder.Configuration["mapbox:token"] ?? string.Empty);


var sqlConnection = builder.Configuration["ConnectionStrings:cloud"] ?? string.Empty;

// Servicio de BD
builder.Services.AddDbContext<LIN.Maps.Server.Data.Context>(options =>
{
    options.UseSqlServer(sqlConnection);
});






var app = builder.Build();


try
{
    // Si la base de datos no existe
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<LIN.Maps.Server.Data.Context>();
    var res = dataContext.Database.EnsureCreated();
}
catch
{
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();
app.UseLINHttp();
Conexión.SetStringConnection(sqlConnection);

app.MapControllers();

app.Run();
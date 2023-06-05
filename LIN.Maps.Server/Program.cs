global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using LIN.Modules;
global using LIN.Types.Responses;
global using LIN.Maps.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


string sqlConnection = builder.Configuration["ConnectionStrings:somee"] ?? string.Empty;

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
{ }


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

LIN.Maps.Server.Mapbox.Token = builder.Configuration["mapbox:token"] ?? string.Empty;

app.UseCors("AllowAnyOrigin");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

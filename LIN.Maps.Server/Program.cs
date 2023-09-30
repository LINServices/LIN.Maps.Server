global using LIN.Maps.Server;
global using LIN.Modules;
global using LIN.Types.Responses;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;

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


var sqlConnection = builder.Configuration["ConnectionStrings:con"] ?? string.Empty;

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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();
Mapbox.Token = builder.Configuration["mapbox:token"] ?? string.Empty;

app.UseCors("AllowAnyOrigin");
app.UseHttpsRedirection();

app.UseAuthorization();
Conexión.SetStringConnection(sqlConnection);

app.MapControllers();

app.Run();
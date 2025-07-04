using Microsoft.Extensions.Options;
using SistemaVentas.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InjectDependencies(builder.Configuration);
builder.Services.AddCors(options=>
    options.AddPolicy("React", app => {
        app.AllowAnyHeader()
           .AllowAnyMethod()
           .WithOrigins("http://localhost:3000"); 
    })    
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("React");
app.UseAuthorization();

app.MapControllers();

app.Run();

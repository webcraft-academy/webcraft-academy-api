using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using webcnAPI.Service;
using webcnAPI.Domain;
using webcnAPI.Repository;
using webcnAPI.Configurations;
using System.Text;
using Microsoft.OpenApi.Models;
using webcnAPI.V1.Installers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<WebcraftDBSettings>(
builder.Configuration.GetSection("WebCraftAcademyDatabase"));
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();
builder.Services.RegisterSwagger();
builder.Services.RegisterAuthentication();
builder.Services.RegisterMvcAndCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseHttpsRedirection();


app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      .WithOrigins("*"));

app.UseAuthorization();

app.MapControllers();

app.Run();
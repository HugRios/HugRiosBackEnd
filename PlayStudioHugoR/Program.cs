using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlayStudioHugoR.Models.DbPlayContext;
using PlayStudioHugoR.Repository;
using PlayStudioHugoR.Repository.Interfaces;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;
using SendGrid.Helpers.Mail;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string ConnectionString = string.Format(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
builder.Services.AddDbContext<PlayStudioDbContext>(options => options.UseSqlServer(ConnectionString));

string sendGridApiKey = builder.Configuration["SendGrid:ApiKey"]; // Replace with your configuration key
builder.Services.AddSingleton<ISendGridClient>(provider =>
{
    return new SendGridClient(sendGridApiKey);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

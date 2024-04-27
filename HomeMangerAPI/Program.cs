using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Add support to logging with SERILOG


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
     .WriteTo.File("C:\\API-CORE-LOGSd\\log.txt",
     rollingInterval: RollingInterval.Day)
    .CreateLogger();
    
/*
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
*/

/**/

builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddSerilog()
);

Log.Error("fffff");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSerilogRequestLogging();
app.UseAuthorization();

app.MapControllers();

app.Run();

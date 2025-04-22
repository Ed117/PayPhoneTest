using System.Text.Json.Serialization;
using WalletTransfer.Application;
using WalletTransfer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1) Register Infrastructure and Application layers
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// 2) Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 3) Configure MVC, JSON and CORS (if needed)
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Example CORS setup:
// builder.Services.AddCors(opts =>
// {
//     opts.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4) API Versioning, Logging, FluentValidation, etc.
// builder.Services.AddApiVersioning();
// builder.Logging.ClearProviders().AddConsole();

var app = builder.Build();

// 5) Enable Swagger in development only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6) Middlewares
// app.UseCors("AllowAll"); // uncomment if using CORS
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
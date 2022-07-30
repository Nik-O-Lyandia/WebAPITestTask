using WebApp.Interfaces;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();   // Makes easy to manage Http client over all controllers

// Adding dependences
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IBitcoinService, BitcoinService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddControllers();
builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

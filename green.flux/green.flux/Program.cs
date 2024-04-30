using FluentValidation;
using FluentValidation.AspNetCore;
using green.flux.Application;
using green.flux.Domain;
using green.flux.Infrastructure;
using green.flux.Validation;
//using green.flux.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// IConfiguration instance'ýný alýn
var configuration = builder.Configuration;



// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers();
builder.Services
	.AddFluentValidationAutoValidation()
	.AddValidatorsFromAssemblyContaining<GroupValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IValidator<Group>, GroupValidator>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();

builder.Services.AddTransient<IValidator<ChargeStation>, ChargeStationValidator>();
builder.Services.AddScoped<IChargeStationService, ChargeStationService>();
builder.Services.AddScoped<IChargeStationRepository, ChargeStationRepository>();

builder.Services.AddTransient<IValidator<Connector>, ConnectorValidator>();
builder.Services.AddScoped<IConnectorService, ConnectorService>();
builder.Services.AddScoped<IConnectorRepository, ConnectorRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

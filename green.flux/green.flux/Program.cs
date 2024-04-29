using FluentValidation;
using FluentValidation.AspNetCore;
using green.flux.Application;
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
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IChargeStationService, ChargeStationService>();
builder.Services.AddTransient<IConnectorService, ConnectorService>();

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

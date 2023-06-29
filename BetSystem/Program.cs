using BetSystem.BetSystemDbContext;
using BetSystem.BusinnessLogic;
using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using BetSystem.Endpoint;
using BetSystem.Middlewares;
using BetSystem.Model;
using BetSystem.Validator;
using BetSystem.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<BetDbContext>(options =>
            options.UseNpgsql(connectionString)); builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddScoped<IBetOnEventService, BetOnEventService>();
        builder.Services.AddScoped<ISportEventService, SportEventService>();
        builder.Services.AddScoped<ITeamService, TeamService>();

        builder.Services.AddScoped<IValidator<BetOnEventDto>, BetOnEventValidator>();
        builder.Services.AddScoped<IValidator<TeamDto>, TeamValidator>();
        builder.Services.AddScoped<IValidator<EventResultDto>, EventResultValidator>();
        builder.Services.AddScoped<IValidator<SportEventDto>, SportEventValidator>();

        builder.Services.AddSingleton<IMongoDatabase>(options => {
            var settings = builder.Configuration.GetSection("MongoDb").Get<MongoDBSettings>();
            var client = new MongoClient(settings.ConnectionString);
            return client.GetDatabase(settings.DatabaseName);
        });

        builder.Services.AddSingleton<IMongoDBService, MongoDBService>();



        builder.Services.AddScoped<IValidator<IdRequestDto>, IdRequestValidator>();

        builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));






        var app = builder.Build();

        

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        

        app.AddBetOnEventMapping();
        app.AddSportEventMapping();
        app.AddTeamMapping();


        app.Run();


    }
}
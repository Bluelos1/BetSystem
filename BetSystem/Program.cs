using BetSystem.BetSystemDbContext;
using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using BetSystem.Endpoint;
using BetSystem.Middlewares;
using BetSystem.Validator;
using BetSystem.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<BetDbContext>(o => o.UseInMemoryDatabase("Db"));
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddScoped<IBetOnEventService, BetOnEventService>();
        builder.Services.AddScoped<ISportEventService, SportEventService>();
        builder.Services.AddScoped<ITeamService, TeamService>();

        builder.Services.AddScoped<IValidator<BetOnEventDto>, BetOnEventValidator>();
        builder.Services.AddScoped<IValidator<TeamDto>, TeamValidator>();
        builder.Services.AddScoped<IValidator<EventResultDto>, EventResultValidator>();
        builder.Services.AddScoped<IValidator<SportEventDto>, SportEventValidator>();
        builder.Services.AddScoped<IValidator<IdRequestDto>, IdRequestValidator>();

        builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();





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
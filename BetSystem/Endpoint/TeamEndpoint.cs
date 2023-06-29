using BetSystem.BusinnessLogic;
using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using BetSystem.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BetSystem.Endpoint
{
    public static class TeamEndpoint
    {
        public static void AddTeamMapping(this WebApplication app)
        {
            app.MapPost("/Team", PostTeam) ;
            app.MapGet("/Team", GetAll) ;
            app.MapGet("/Team/{id}", GetById);
            app.MapDelete("/Team/{id}", DeleteById);
            app.MapPut("/Team/{id}", PutTeam);
        }
        private static IResult PostTeam(IValidator<TeamDto> validator, TeamDto teamDto, [FromServices] ITeamService service,IMongoDBService mongoDBService)
        {
            var result = validator.Validate(teamDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }
            mongoDBService.CreateTeam(teamDto);
            service.PostTeam(teamDto);
            return Results.Created($"/Team/{teamDto.Id}", teamDto);
        }

        private static IResult GetAll([FromServices] ITeamService service, IMongoDBService mongoDBService)
        {
            var allTeams = mongoDBService.GetTeams();
            return Results.Ok(service.GetAllTeams());
        }

        private static IResult GetById(int id, [FromServices] ITeamService service)
        {
            var result = service.GetTeamById(id);
            if (result == null) return Results.NotFound(id);
            return Results.Ok(result);
        }

        private static IResult DeleteById(int id, [FromServices] ITeamService service)
        {
            if (service.GetTeamById(id) == null) return Results.NotFound(id);
            service.DeleteTeamById(id);
            return Results.Ok(id);

        }

        private static IResult PutTeam(int id, IValidator<TeamDto> validator, TeamDto teamDto, [FromServices] ITeamService service)
        {
            var result = validator.Validate(teamDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }
            if (service.GetTeamById(id) == null) return Results.NotFound(id);


            service.PutTeam(id, teamDto);
            return Results.NoContent();
        }
    }
}

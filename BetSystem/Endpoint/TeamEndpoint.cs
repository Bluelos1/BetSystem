using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BetSystem.Endpoint
{
    public static class TeamEndpoint
    {
        public static void AddTeamMapping(this WebApplication app)
        {
            app.MapPost("/Team", PostTeam);
            app.MapGet("/Team", GetAll);
            app.MapGet("/Team/{id}", GetById);
            app.MapDelete("/Team/{id}", DeleteById);
            app.MapPut("/Team/{id}", PutTeam);
        }

        private static IResult PostTeam(IValidator<TeamDto> validator, TeamDto teamDto, [FromServices] ITeamService teamService)
        {
            var result = validator.Validate(teamDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }
            teamService.PostTeam(teamDto);
            return Results.Created($"/Team/{teamDto.Id}", teamDto);
        }

        private static IResult GetAll([FromServices] ITeamService teamService)
        {
            return Results.Ok(teamService.GetAllTeams());
        }

        private static IResult GetById(int id, [FromServices] ITeamService teamService)
        {
            var result = teamService.GetTeamById(id);
            if (result == null) return Results.NotFound(id);
            return Results.Ok(result);
        }

        private static IResult DeleteById(int id, [FromServices] ITeamService teamService)
        {
            if (teamService.GetTeamById(id) == null) return Results.NotFound(id);
            teamService.DeleteTeamById(id);
            return Results.Ok(id);

        }

        private static IResult PutTeam(int id, IValidator<TeamDto> validator, TeamDto teamDto, [FromServices] ITeamService teamService)
        {
            var result = validator.Validate(teamDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }
            if (teamService.GetTeamById(id) == null) return Results.NotFound(id);


            teamService.PutTeam(id, teamDto);
            return Results.NoContent();
        }
    }
}

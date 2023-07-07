using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BetSystem.Endpoint
{
    public static class SportEventEndpoint
    {
        public static void AddSportEventMapping(this WebApplication app)
        {
            app.MapPost("/SportEvent", PostSportEvent);
            app.MapGet("/SportEvent", GetAllEvents);
            app.MapGet("/SportEvent/{id}", GetById);
            app.MapPut("/SportEvent/{id}", PutSportEvent);
            app.MapDelete("/SportEvent/{id}", DeleteById);

            app.MapPost("/SportEvent/{id}/teams", PostTeamToSportEvent);
            app.MapGet("/SportEvent/{id}/teams", GetTeamsFromSportEvent);

            app.MapPost("/SportEvent/{id}/bets", PlaceBetOnEvent);
            app.MapGet("/SportEvent/{id}/bets", GetBetToSportEvent);

            app.MapPost("/SportEvent/{id}/results", PostResultToSportEvent);
            app.MapGet("/SportEvent/{id}/results", GetResultFromSportEvent);

        }

        

        private static IResult GetResultFromSportEvent(int id, [FromServices] ISportEventService sportEventService)
        {
            var sportEvent = sportEventService.GetById(id);
            if (sportEvent == null) return Results.NotFound();
            return Results.Ok(sportEventService.GetResultsFromSportEvent(id));
        }

        private static IResult PostResultToSportEvent(int id, EventResultDto eventResultDto, IValidator<EventResultDto> validator, [FromServices] ISportEventService sportEventService)
        {
            var sportEvent = sportEventService.GetById(id);

            if (sportEvent == null) return Results.NotFound();
            var result = validator.Validate(eventResultDto);

            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            if (sportEventService.GetById(id) == null) Results.NotFound(id);
            if (sportEventService.GetById(eventResultDto.TeamId) == null) return Results.BadRequest("Team Not Found");

            sportEventService.AddResultToSportEvent(id, eventResultDto);

            return Results.Ok();

        }

        private static IResult GetBetToSportEvent(int id, [FromServices] ISportEventService sportEventService)
        {
            var sportEvent = sportEventService.GetById(id);
            if (sportEvent == null) return Results.NotFound();
            return Results.Ok(sportEventService.GetBetsFromSportEvent(id));
        }

        private static IResult PlaceBetOnEvent(int id,IValidator<BetOnEventDto> validator, BetOnEventDto betOnEventDto, [FromServices] ISportEventService sportEventService)
        {
            var sportEvent = sportEventService.GetById(id);
            if (sportEvent == null) return Results.NotFound();
            var result = validator.Validate(betOnEventDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }
            if (sportEventService.GetById(betOnEventDto.TeamId) == null) return Results.BadRequest("Team Not Found");
            if (sportEventService.GetById(betOnEventDto.EventId) == null) return Results.BadRequest("Event Not Found");

            sportEventService.AddBetToSportEvent( betOnEventDto);
            return Results.Created($"/BetOnEvent/{betOnEventDto.Id}", betOnEventDto);
        }

        private static IResult GetTeamsFromSportEvent(int id, [FromServices] ISportEventService sportEventService)
        {
            var sportEvent = sportEventService.GetById(id);

            if (sportEvent == null) return Results.NotFound();
            return Results.Ok(sportEventService.GetTeamsFromSportEvent(id));


        }

        public static IResult PostTeamToSportEvent(int id, IdRequestDto idRequestDto, IValidator<IdRequestDto> validator, [FromServices] ISportEventService sportEventService, [FromServices] ITeamService teamService)
        {
            if (sportEventService.GetById(id) == null) return Results.NotFound();
            var result = validator.Validate(idRequestDto);

            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            var team = teamService.GetTeamById(idRequestDto.Id);
            if (team == null) return Results.BadRequest("Team Not Found");
            sportEventService.AddTeamToSportEvent(id, idRequestDto);
            return Results.Created($"/SportEvent/{idRequestDto.Id}/teams", idRequestDto);
        }

        public static IResult PostSportEvent(IValidator<SportEventDto> validator, SportEventDto sportEventDto, [FromServices] ISportEventService sportEventService)
        {
            var result = validator.Validate(sportEventDto);

            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            sportEventService.PostSportEvent(sportEventDto);
            return Results.Created($"/SportEvent/{sportEventDto.Id}", sportEventDto);
        }

        public static IResult GetAllEvents([FromServices] ISportEventService sportEventService)
        {
            return Results.Ok(sportEventService.GetAllEvents());
        }

        public static IResult GetById(int id, [FromServices] ISportEventService sportEventService)
        {
            var result = sportEventService.GetById(id);
            if (result == null) return Results.NotFound(id);
            return Results.Ok(result);
        }

        private static IResult DeleteById(int id, [FromServices] ISportEventService sportEventService)
        {
            if (sportEventService.GetById(id) == null) return Results.NotFound();

            sportEventService.DeleteById(id);
            return Results.Ok(id);
        }

        private static IResult PutSportEvent(int id, SportEventDto sportEventDto, IValidator<SportEventDto> validator, [FromServices] ISportEventService sportEventService)
        {
            if (sportEventService.GetById(id) == null) return Results.NotFound();


            var result = validator.Validate(sportEventDto);
            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());

            sportEventService.PutSportEvent(id, sportEventDto);
            return Results.NoContent();
        }
    }
}

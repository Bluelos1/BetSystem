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

        

        private static IResult GetResultFromSportEvent(int id, [FromServices] ISportEventService service)
        {
            var sportEvent = service.GetById(id);
            if (sportEvent == null) return Results.NotFound();
            return Results.Ok(service.GetResultsFromSportEvent(id));
        }

        private static IResult PostResultToSportEvent(int id, EventResultDto eventResultDto, IValidator<EventResultDto> validator, [FromServices] ISportEventService service)
        {
            var sportEvent = service.GetById(id);

            if (sportEvent == null) return Results.NotFound();
            var result = validator.Validate(eventResultDto);

            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            if (service.GetById(id) == null) Results.NotFound(id);
            service.AddResultToSportEvent(id, eventResultDto);

            return Results.Ok();

        }

        private static IResult GetBetToSportEvent(int id, [FromServices] ISportEventService service)
        {
            var sportEvent = service.GetById(id);
            if (sportEvent == null) return Results.NotFound();
            return Results.Ok(service.GetBetsFromSportEvent(id));
        }

        private static IResult PlaceBetOnEvent(int id,IValidator<BetOnEventDto> validator, BetOnEventDto betOnEventDto, [FromServices] ISportEventService service)
        {
            var sportEvent = service.GetById(id);
            if (sportEvent == null) return Results.NotFound();
            var result = validator.Validate(betOnEventDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            service.AddBetToSportEvent( betOnEventDto);
            return Results.Created($"/BetOnEvent/{betOnEventDto.Id}", betOnEventDto);
        }

        private static IResult GetTeamsFromSportEvent(int id, [FromServices] ISportEventService service)
        {
            var sportEvent = service.GetById(id);

            if (sportEvent == null) return Results.NotFound();
            return Results.Ok(service.GetTeamsFromSportEvent(id));


        }

        public static IResult PostTeamToSportEvent(int id, IdRequestDto idRequestDto, IValidator<IdRequestDto> validator, [FromServices] ISportEventService service, [FromServices] ITeamService teamService)
        {
            var sportEvent = service.GetById(id);

            if (sportEvent == null) return Results.NotFound();
            var result = validator.Validate(idRequestDto);

            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            if (teamService.GetTeamById(idRequestDto.Id) == null) return Results.BadRequest("Team Not Found");
            service.AddTeamToSportEvent(id, idRequestDto);
            return Results.Created($"/SportEvent/{idRequestDto.Id}/teams", idRequestDto);
        }

        public static IResult PostSportEvent(IValidator<SportEventDto> validator, SportEventDto sportEventDto, [FromServices] ISportEventService service)
        {
            var result = validator.Validate(sportEventDto);

            if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            service.PostSportEvent(sportEventDto);
            return Results.Created($"/SportEvent/{sportEventDto.Id}", sportEventDto);
        }

        public static IResult GetAllEvents([FromServices] ISportEventService service)
        {
            return Results.Ok(service.GetAllEvents());
        }

        public static IResult GetById(int id, [FromServices] ISportEventService service)
        {
            var result = service.GetById(id);
            if (result == null) return Results.NotFound(id);
            return Results.Ok(result);
        }

        private static IResult DeleteById(int id, [FromServices] ISportEventService service)
        {
            service.DeleteById(id);
            return Results.Ok(id);
        }

        private static IResult PutSportEvent(int id, SportEventDto sportEventDto, [FromServices] ISportEventService service)
        {
            service.PutSportEvent(id, sportEventDto);
            return Results.NoContent();
        }
    }
}

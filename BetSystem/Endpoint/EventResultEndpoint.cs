using BetSystem.BusinnessLogic;
using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BetSystem.Endpoint
{
    public static class EventResultEndpoint
    {
        public static void AddEventResultMapping(this WebApplication app)
        {
            app.MapPost("/EventResult", PostResult);
            app.MapGet("/EventResult", GetAllResult);
            app.MapGet("/EventResult/{id}", GetEventResultById);
            app.MapDelete("/EventResult/{id}", DeleteEventResultById);
        }
        private static IResult PostResult(IValidator<EventResultDto> validator, EventResultDto eventResultDto, [FromServices] IEventResultService service)
        {
            var result = validator.Validate(eventResultDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            service.PostEventResult(eventResultDto);
            return Results.Created($"/EventResult/{eventResultDto.Id}", eventResultDto);
        }

        private static IResult GetAllResult([FromServices] IEventResultService service)
        {
            return Results.Ok(service.GetAllEvents());
        }

        private static IResult GetEventResultById(int id, [FromServices] IEventResultService service)
        {
            var result = service.GetEventResultById(id);
            if (result == null) { return Results.NotFound(id); }
            return Results.Ok(result);
        }

        private static IResult DeleteEventResultById(int id, [FromServices] IEventResultService service)
        {
            service.DeleteEventResultById(id);
            return Results.Ok(id);
        }
    }
}

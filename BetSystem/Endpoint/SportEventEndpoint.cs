using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BetSystem.Endpoint
{
    public static class SportEventEndpoint
    {
        public static void AddSportEventMapping(this WebApplication app) 
        {
            app.MapPost("/SportEvent",PostSportEvent);
            app.MapGet("/SportEvent",GetAllEvents);
            app.MapGet("/SportEvent/{id}",GetById);
            app.MapDelete("/SportEvent/{id}", DeleteById);
        }

        public static IResult PostSportEvent(IValidator<SportEventDto> validator,SportEventDto sportEventDto, [FromServices] ISportEventService service)
        {
            var result = validator.Validate(sportEventDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            service.PostSportEvent(sportEventDto);
            return Results.Created($"/SportEvent/{sportEventDto.Id}", sportEventDto);
        }

        public static IResult GetAllEvents([FromServices] ISportEventService service)
        {
            return Results.Ok(service.GetAllEvents());
        }

        private static IResult GetById(int id, [FromServices] ISportEventService service)
        {
            var result = service.GetById(id);
            if (result == null)  return Results.NotFound(id);
            return Results.Ok(result);
        }

        private static IResult DeleteById(int id, [FromServices] ISportEventService service)
        {
            service.DeleteById(id);
            return Results.Ok(id);
        }
    }
}

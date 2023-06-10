using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BetSystem.Endpoint
{
    public static class BetOnEventEndpoint
    {
        public static void AddBetOnEventMapping (this WebApplication app)
        {
            app.MapPost("/BetOnEvent", PlaceBetOnEvent);  
            app.MapGet("/BetOnEvent", GetAllBets);
            app.MapGet("/BetOnEvent/{id}", GetById);
            app.MapDelete("/BetOnEvent/{id}", DeleteById);
        }

        private static IResult PlaceBetOnEvent(IValidator<BetOnEventDto> validator,BetOnEventDto betOnEventDto, [FromServices] IBetOnEventService service)
        {
            var result = validator.Validate(betOnEventDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            service.PlaceBetOnEvent(betOnEventDto);
            return Results.Created($"/BetOnEvent/{betOnEventDto.Id}", betOnEventDto);
        }

        private static IResult GetAllBets ([FromServices]IBetOnEventService service)
        {
           return Results.Ok(service.GetAllBets());
        }

        private static IResult GetById(int id, [FromServices] IBetOnEventService service)
        {
            var result = service.GetBetOnEventById(id);
            if (result == null) return Results.NotFound(id);
            return Results.Ok(result);
        }

        private static IResult DeleteById(int id, [FromServices] IBetOnEventService service)
        {
            service.DeleteBetOnEvent(id);
            return Results.Ok(id);
        }

    }
}

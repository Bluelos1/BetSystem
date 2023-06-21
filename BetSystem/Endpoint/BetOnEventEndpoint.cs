using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
using BetSystem.Model;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BetSystem.Endpoint
{
    public static class BetOnEventEndpoint
    {
        public static void AddBetOnEventMapping(this WebApplication app)
        {
            app.MapGet("/BetOnEvent", GetAllBets);
            app.MapGet("/BetOnEvent/{id}", GetById);
            app.MapPut("/BetOnEvent/{id}", PutBet);
            app.MapDelete("/BetOnEvent/{id}", DeleteById);
            app.MapGet("/BetOnEvent/stats", GetStatistics);

        }

        private static IResult GetStatistics([FromServices] IBetOnEventService service)
        {
            return Results.Ok(service.GetStatistics());
        }

       

        private static IResult GetAllBets([FromServices] IBetOnEventService service)
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
            if (service.GetBetOnEventById(id) == null) return Results.NotFound();

            service.DeleteBetOnEvent(id);
            return Results.Ok(id);
        }

        private static IResult PutBet(int id, BetOnEventDto betOnEventDto, IValidator<BetOnEventDto> validator, [FromServices] IBetOnEventService service)
        {
            if (service.GetBetOnEventById(id) == null) return Results.NotFound();
            var result = validator.Validate(betOnEventDto);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            service.PutBetOnEvent(id, betOnEventDto);
            return Results.NoContent();
        }

    }
}

﻿using BetSystem.Contract;
using BetSystem.Contract.BusinnessLogic;
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
            service.DeleteBetOnEvent(id);
            return Results.Ok(id);
        }

        private static IResult PutBet(int id, BetOnEventDto betOnEventDto, [FromServices] IBetOnEventService service)
        {
            service.PutBetOnEvent(id, betOnEventDto);
            return Results.NoContent();
        }

    }
}

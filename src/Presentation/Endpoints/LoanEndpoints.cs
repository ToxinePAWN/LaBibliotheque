namespace CleanMinimalApi.Presentation.Endpoints;

using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Presentation.Filters;
using CleanMinimalApi.Presentation.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Commands = Application.Loans.Commands;
using Entities = Application.Loans.Entities;
using Queries = Application.Loans.Queries;

public static class LoanEndpoints
{
    public static WebApplication MapLoanEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/loans")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("loans")
            .WithDescription("Lookup, Find and Manipulate loans");

        _ = root.MapGet("/", GetReviews)
            .Produces<List<Entities.Loan>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all Reviews")
            .WithDescription("\n    GET /review");

        _ = root.MapGet("/{id}", GetReviewById)
            .Produces<Entities.Loan>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Lookup a Review by its Ids")
            .WithDescription("\n    GET /review/00000000-0000-0000-0000-000000000000");

        _ = root.MapPost("/", CreateLoan)
            .Produces<Entities.Loan>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Create a Review")
            .WithDescription("\n    POST /review\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }");

        _ = root.MapPut("/{id}", UpdateReview)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Update a Review")
            .WithDescription("\n    PUT /review/00000000-0000-0000-0000-000000000000\n     {         \"authorId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"movieId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\",         \"stars\": 5       }");

        return app;
    }

    public static async Task<Results<Ok<List<Entities.Loan>>, ProblemHttpResult>> GetReviews([FromServices] ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new Queries.GetReviews.GetLoansQuery()));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<Ok<Entities.Loan>, NotFound<string>, ProblemHttpResult>> GetReviewById([Validate][FromRoute] Guid id, [FromServices] ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new Queries.GetReviewById.GetLoanByIdQuery
            {
                Id = id
            }));
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<Created<Entities.Loan>, NotFound<string>, Conflict<string>, ProblemHttpResult>> CreateLoan([Validate][FromBody] CreateReviewRequest request, [FromServices] ISender sender)
    {
        try
        {
            var response = await sender.Send(new Commands.CreateReview.CreateLoanCommand
            {
                MemberId = request.MemberId,
                BookId = request.BookId,
            });

            return TypedResults.Created($"/api/loans/{response.Id}", response);
        }
        catch (BusinessRulesException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<Results<NoContent, NotFound<string>, ProblemHttpResult>> UpdateReview([Validate][FromRoute] Guid id, [Validate][FromBody] UpdateReviewRequest request, [FromServices] ISender sender)
    {
        try
        {
            _ = await sender.Send(new Commands.UpdateReview.UpdateLoanCommand
            {
                Id = id,
                AuthorId = request.AuthorId,
                MovieId = request.MovieId,
                Stars = request.Stars
            });

            return TypedResults.NoContent();
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

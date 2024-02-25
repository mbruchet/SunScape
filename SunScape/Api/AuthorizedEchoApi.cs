using Atc.Rest.MinimalApi.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SunScape.Api;

public class AuthorizedEchoApi : IEndpointDefinition
{
    internal const string ApiRouteBase = "/api/echo";


    public void DefineEndpoints(WebApplication app)
    {
        var an = app.NewVersionedApi("AuthorizedApi");

        var anV1 = an
            .MapGroup(ApiRouteBase)
            .HasApiVersion(1.0);

        anV1
            .MapGet("/", GetAuthorizedEchoFromServer)
            .RequireAuthorization()
            .WithName("GetAuthorizedEchoFromServer");
    }

    internal Task<IResult> GetAuthorizedEchoFromServer(HttpContext context, CancellationToken cancellationToken)
    {
        var userName = context?.User.Identity.Name;
        return Task.FromResult(Results.Ok($"Echo from server {Environment.MachineName} - user {userName}"));
    }
}


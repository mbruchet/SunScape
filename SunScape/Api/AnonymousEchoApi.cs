using Atc.Rest.MinimalApi.Abstractions;

namespace SunScape.Api
{
    public class AnonymousEchoApi : IEndpointDefinition
    {
        internal const string ApiRouteBase = "/api/anoymousecho";


        public void DefineEndpoints(WebApplication app)
        {
            var an = app.NewVersionedApi("AnonymousEcho");

            var anV1 = an
                .MapGroup(ApiRouteBase)
                .HasApiVersion(1.0);

            anV1
                .MapGet("/", GetAnonymousEchoFromServer)
                .WithName("GetAnonymousEchoFromServer");
        }

        internal Task<IResult> GetAnonymousEchoFromServer(
        CancellationToken cancellationToken)
        => Task.FromResult(Results.Ok($"Echo from server {Environment.MachineName}"));
    }
}

using LodgeSpotGo.RecommodationSystem.Core.Model;
using Neo4jClient;

namespace LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

public class RecommodationRepository : IRecommodationRepository
{
    private readonly IGraphClient _client;

    public RecommodationRepository(IGraphClient client)
    {
        _client = client;
    }

    public async Task<Boolean> getRecommodations()
    {
        // var recommodations = await _client.Cypher.Match("(n:Recommodation)")
        //     .Return(n => n.As<Recommodation>()).ResultsAsync;
        var rec = new Recommodation
        {
            name = "laaaaa"
        };
        await _client.Cypher.Create("(r:Recommodation $rec)")
            .WithParam("rec", rec)
            .ExecuteWithoutResultsAsync();
        return true;
    }
}
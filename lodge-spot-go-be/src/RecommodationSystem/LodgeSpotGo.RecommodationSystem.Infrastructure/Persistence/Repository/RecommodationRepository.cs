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

    public async Task<bool> getRecommodations()
    {
        // var recommodations = await _client.Cypher.Match("(n:Recommodation)")
        //     .Return(n => n.As<Recommodation>()).ResultsAsync;
        var rec = new Recommodation
        {
            name = "asgdsdasfdsfd"
        };
        await _client.Cypher.Create("(r:Recommodation $rec)")
            .WithParam("rec", rec)
            .ExecuteWithoutResultsAsync();
        return true;
    }
    
    public async Task<Guest> CreateGuest(Guest request)
    {
        await _client.Cypher.Create("(r:User $request)")
            .WithParam("request", request)
            .ExecuteWithoutResultsAsync();
        return request;
    }
    public async Task<Accommodation> CreateAccommodation(Accommodation request)
    {
        await _client.Cypher.Create("(r:Accommodation $request)")
            .WithParam("request", request)
            .ExecuteWithoutResultsAsync();
        return request;
    }
}
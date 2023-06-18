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

    public async Task<bool> MakeReservation(Guest guest, string accommodationId)
    {
        await _client.Cypher.Match("(a:Accommodation),(u:Guest)")
            .Where((Accommodation a, Guest u) => a.Id == accommodationId && u.Name == guest.Name)
            .Create("(u) - [r:hasReserved]->(a)")
            .ExecuteWithoutResultsAsync();
        return true;
    }

    public async Task<Guest> CreateGuest(Guest request)
    {
        await _client.Cypher.Create("(r:Guest $request)")
            .WithParam("request", request)
            .ExecuteWithoutResultsAsync();
        return request;
    }

    public async Task<Guest?> GetGuestByMail(string mail)
    {
        var guests = await _client.Cypher.Match("(g:Guest)")
            .Where((Guest g) => g.Name == mail)
            .Return(g => g.As<Guest>()).ResultsAsync;
        return guests.FirstOrDefault();
    }
    
    public async Task<Accommodation?> GetAccommodationById(string id)
    {
        var accommodations = await _client.Cypher.Match("(a:Accommodation)")
            .Where((Accommodation a) => a.Id == id)
            .Return(a => a.As<Accommodation>()).ResultsAsync;
        return accommodations.FirstOrDefault();
    }

    public async Task<List<Guest>> GetGuestsByReservedAccommodations(string guestEmail)
    {
        var guests = await _client.Cypher
            .Match("(givenGuest:Guest)-[:hasReserved]->(accommodation:Accommodation)<-[:hasReserved]-(guest:Guest)")
            .Where((Guest givenGuest)=> givenGuest.Name == guestEmail)
            .AndWhere((Guest guest)=> guest.Name != guestEmail)
            .With("givenGuest, guest, accommodation, COUNT(accommodation) AS reservations")
            .Where("reservations >= 2")
            .ReturnDistinct(guest => guest.As<Guest>())
            .ResultsAsync;

        return guests.ToList();
    }

    public async Task<List<Guest>> GetGuestsByGradedAccommodations(string guestEmail)
    {
        var guests = await _client.Cypher
            .Match("(guest1:Guest)-[grade1:hasGraded]->(accommodation:Accommodation)<-[grade:hasGraded]-(guest:Guest)")
            .Where((Guest guest1)=> guest1.Name == guestEmail)
            .AndWhere((Guest guest)=> guest.Name != guestEmail)
            .AndWhere("grade1.grade = grade.grade OR abs(grade1.grade - grade.grade) = 1")
            .WithParam("guestEmail", guestEmail)
            .ReturnDistinct(guest => guest.As<Guest>())
            .ResultsAsync;

        return guests.ToList();
    }

    public async Task<bool> MakeAccommodationGrade(Guest guest, string accommodationId,int grade)
    {
        await _client.Cypher.Match("(a:Accommodation),(u:Guest)")
            .Where((Accommodation a, Guest u) => a.Id == accommodationId && u.Name == guest.Name)
            .Create("(u) - [r:hasGraded {grade: $grade}]->(a)")
            .WithParam("grade", grade)
            .ExecuteWithoutResultsAsync();
        return true;
    }

    public async Task<Accommodation> CreateAccommodation(Accommodation request)
    {
        await _client.Cypher.Create("(r:Accommodation $request)")
            .WithParam("request", request)
            .ExecuteWithoutResultsAsync();
        return request;
    }
    
    // MATCH (givenGuest:Guest {Name: "a@gmail.com"})-[:hasGraded]->(accommodation:Accommodation)<-[:hasGraded]-(guest:Guest)
    // Where givenGuest <> guest And give
    //     Return guest
}
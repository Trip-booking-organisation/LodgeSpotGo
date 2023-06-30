using LodgeSpotGo.RecommodationSystem.Core.Model;
using MassTransit.Initializers;
using Neo4j.Driver;
using Neo4jClient;


namespace LodgeSpotGo.RecommodationSystem.Infrastructure.Persistence.Repository;

public class RecommodationRepository : IRecommodationRepository
{
    // private readonly IGraphClient _client;
    
    private readonly IDriver driver;
    private DbSettings _dbSettings;
    public RecommodationRepository(DbSettings dbSettings)
    {
        
        // _client = client;
        _dbSettings = dbSettings;
        driver = GraphDatabase.Driver(_dbSettings.Neo4jDb, AuthTokens.Basic(dbSettings.DbName, dbSettings.DbPassword));
    }

    public async Task<bool> getRecommodations()
{
    await using var session = driver.AsyncSession();
    await session.WriteTransactionAsync(async tx =>
    {
        var rec = new Recommodation
        {
            name = "asgdsdasfdsfd"
        };

        var queryText = @"
        CREATE (r:Recommodation $rec)
        ";
        await tx.RunAsync(queryText, new { rec });
    });

    return true;
}

public async Task<bool> MakeReservation(Guest guest, string accommodationId)
{
    await using var session = driver.AsyncSession();
    await session.WriteTransactionAsync(async tx =>
    {
        var queryText = @"
        MATCH (a:Accommodation),(u:Guest)
        WHERE a.Id = $accommodationId AND u.Name = $guestName
        CREATE (u)-[r:hasReserved]->(a)
        ";
        await tx.RunAsync(queryText, new { accommodationId, guestName = guest.Name });
    });

    return true;
}

public async Task<Guest> CreateGuest(Guest request)
{
    await using var session = driver.AsyncSession();
    await session.WriteTransactionAsync(async tx =>
    {
        var queryText = @"
        CREATE (r:Guest $request)
        ";
        await tx.RunAsync(queryText, new { request });
    });

    return request;
}

public async Task<Guest?> GetGuestByMail(string mail)
{
    await using var session = driver.AsyncSession();
    var result = await session.ReadTransactionAsync(async tx =>
    {
        var queryText = @"
        MATCH (g:Guest)
        WHERE g.Name = $mail
        RETURN g
        ";
        var queryResult = await tx.RunAsync(queryText, new { mail });
        return await queryResult.ToListAsync();
    });

    var guestNode = result.FirstOrDefault();
    if (guestNode != null)
    {
        var guestProperties = guestNode["g"].As<INode>().Properties;
        return new Guest
        {
            Name = guestProperties["Name"].As<string>()
            // Set other properties as needed
        };
    }

    return null;
}

public async Task<Accommodation?> GetAccommodationById(string id)
{
    await using var session = driver.AsyncSession();
    var result = await session.ReadTransactionAsync(async tx =>
    {
        var queryText = @"
        MATCH (a:Accommodation)
        WHERE a.Id = $id
        RETURN a
        ";
        var queryResult = await tx.RunAsync(queryText, new { id });
        return await queryResult.ToListAsync();
    });

    var accommodationNode = result.FirstOrDefault();
    if (accommodationNode != null)
    {
        var accommodationProperties = accommodationNode["a"].As<INode>().Properties;
        return new Accommodation
        {
            Id = accommodationProperties["Id"].As<string>(),
            // Set other properties as needed
        };
    }

    return null;
}
    
public async Task<List<Guest>> GetGuestsByReservedAccommodations(string guestEmail)
{
    await using var session = driver.AsyncSession();
    var guestList  = await session.ExecuteReadAsync(async tx =>
    {
        var queryText = @"
        MATCH (givenGuest:Guest {Name: $email})-[:hasReserved]->(accommodation:Accommodation)<-[:hasReserved]-(guest:Guest) 
        WHERE givenGuest <> guest WITH guest, COUNT(DISTINCT accommodation) AS reservationCount 
        WHERE reservationCount >= 2 
        RETURN guest";
        var result = await tx.RunAsync(queryText, new { email = guestEmail });


        return await result.ToListAsync(record =>
        {
            var guest = record["guest"].As<INode>();
            var name = guest.Properties["Name"].As<string>();
            return new Guest { Name = name };
        });
    });
    return guestList;
}

public async Task<List<Accommodation>> GetGuestsReservedAccommodations(string guestName)
{
    await using var session = driver.AsyncSession();
    var accommodationList = await session.ExecuteReadAsync(async qr =>
    {
        var queryText =
            @"MATCH (givenGuest:Guest {Name: $email})-[reserved:hasReserved]->(accommodation:Accommodation)  " +
            "Return accommodation";
        var result = await qr.RunAsync(queryText, new { email = guestName });
        return await result.ToListAsync(record =>
        {
            var accommodation = record["accommodation"].As<INode>();
            var id = accommodation.Properties["Id"].As<string>();
            var name = accommodation.Properties["Name"].As<string>();
            return new Accommodation { Id = id, Name = name };
        });
    });
    return accommodationList;
}


public async Task<List<Guest>> GetGuestsByGradedAccommodations1(string guestEmail)
    {
        await using var session = driver.AsyncSession();
        var guestList  = await session.ExecuteReadAsync(async tx =>
        {
            var queryText = @"
            MATCH (givenGuest:Guest {Name: $email})-[grade1:hasGraded]->(accommodation:Accommodation)<-[grade:hasGraded]-(guest:Guest)
            WHERE givenGuest <> guest AND (grade1.grade = grade.grade OR abs(grade1.grade - grade.grade) = 1)
            RETURN guest
            ";
            var result = await tx.RunAsync(queryText, new { email = guestEmail });


            return await result.ToListAsync(record =>
            {
                var guest = record["guest"].As<INode>();
                var name = guest.Properties["Name"].As<string>();
                return new Guest { Name = name };
            });
        });
        return guestList;
    }
    // string cypherQuery = @"
    //         MATCH (givenGuest:Guest {Name: $email})-[grade1:hasGraded]->(accommodation:Accommodation)<-[grade:hasGraded]-(guest:Guest)
    //         Where givenGuest <> guest 
    //         and (grade1.grade = grade.grade OR abs(grade1.grade - grade.grade) = 1) 
    //         Return guest";
    // var cursor = await tx.RunAsync(query, new { email });
    // var result = await _client.Cypher
    //     .WithParam("email", guestEmail)
    //     .Match(cypherQuery)
    //     .ReturnDistinct(guest => guest.As<Guest>())
    //     .ResultsAsync;
    //
    // List<Guest> guests = result
    //     .Select(record => new Guest { Name = record.Name })
    //     .ToList();
    public async Task<bool> MakeAccommodationGrade(Guest guest, string accommodationId, int grade)
    {
        await using var session = driver.AsyncSession();
        await session.WriteTransactionAsync(async tx =>
        {
            var queryText = @"
        MATCH (a:Accommodation), (u:Guest)
        WHERE a.Id = $accommodationId AND u.Name = $guestName
        CREATE (u)-[r:hasGraded { grade: $grade }]->(a)
        ";
            var parameters = new { accommodationId, guestName = guest.Name, grade };
            await tx.RunAsync(queryText, parameters);
        });

        return true;
    }
    //
    public async Task<Accommodation> CreateAccommodation(Accommodation request)
    {
        await using var session = driver.AsyncSession();
        await session.WriteTransactionAsync(async tx =>
        {
            var queryText = @"
        CREATE (r:Accommodation $request)
        ";
            var parameters = new { request };
            await tx.RunAsync(queryText, parameters);
        });

        return request;
    }
    //
    // MATCH (givenGuest:Guest {Name: "s@gmail.com"})-[grade1:hasGraded]->(accommodation:Accommodation)<-[grade:hasGraded]-(guest:Guest)
    // Where givenGuest <> guest and (grade1.grade = grade.grade OR abs(grade1.grade - grade.grade) = 1)
    // Return guest
}
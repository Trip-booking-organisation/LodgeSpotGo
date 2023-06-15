using Grpc.Core;
using JetSetGo.AccommodationManagement.Application.Common.Persistence;

namespace JetSetGo.AccommodationManagement.Grpc.Services.Grades;

public class HostAccommodationService :HostAccommodationApp.HostAccommodationAppBase
{
    private readonly IAccommodationRepository _accommodationRepository;

    public HostAccommodationService(IAccommodationRepository accommodationRepository)
    {
        _accommodationRepository = accommodationRepository;
    }

    public override async Task<GetAccommodationHostResponse> GetAccommodationsByHost(GetAccommodationHostRequest request, ServerCallContext context)
    {
        var accommodations = await _accommodationRepository.GetByHost(Guid.Parse(request.HostId));
        var response = new GetAccommodationHostResponse();
        accommodations.ForEach(x =>
        {
            var acc = new AccommodationHost
            {
                Id = x.Id.ToString()
            };
            response.Accommodations.Add(acc);
        });
        return response;
    }
}
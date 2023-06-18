using JetSetGo.RecommodationSystem.Grpc;

namespace JetSetGo.AccommodationManagement.Grpc.Clients.Recommendation;

public interface IRecommendationClient
{
    GetRecommodationsResponse GetRecommendations(GetRecommodationReqest request);
}
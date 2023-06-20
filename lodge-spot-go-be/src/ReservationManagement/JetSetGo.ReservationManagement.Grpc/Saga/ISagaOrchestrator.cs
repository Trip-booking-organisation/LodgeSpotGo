namespace JetSetGo.ReservationManagement.Grpc.Saga;

public interface ISagaOrchestrator<in TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    public Task<TResponse> CreateSaga(TRequest request);
}
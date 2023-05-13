using AutoMapper;
using Grpc.Core;
using JetSetGo.ReservationManagement.Application.Common.Persistence;
using JetSetGo.ReservationManagement.Domain.Reservation;
using JetSetGo.ReservationManagement.Domain.Reservation.ValueObjects;

namespace JetSetGo.ReservationManagement.Grpc.Services;

public class ReservationService : ReservationApp.ReservationAppBase
{

    private readonly ILogger<ReservationService> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository reservationRepository, ILogger<ReservationService> logger, IMapper mapper)
    {
        _logger = logger;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public override async Task<GetResrvationListResponse> GetReservationList(GetResrvationListRequest request, ServerCallContext context)
    {
        var list = new GetResrvationListResponse();
        var reservations = await _reservationRepository.GetAllAsync();
        _logger.LogInformation(@"List {}", reservations.ToString());
        var respoinseList = reservations.Select(res => _mapper.Map<ReadReservationDto>(res)).ToList();
        respoinseList.ForEach( dto => list.Reservations.Add(dto));
        return list;
    }

    public override Task<CreateReservationResponse> CreateReservation(CreateReservationRequest request, ServerCallContext context)
    {
        _logger.LogInformation(@"Request {request.Reservation}", request.Reservation);
        var reservation = new Reservation
        {
            Id = new Guid(),
            AccommodationId = Guid.Parse(request.Reservation.AccommodatoinId),
            DateRange = new DateRange
            {
                From = request.Reservation.DateRange.From.ToDateTime(),
                To = request.Reservation.DateRange.To.ToDateTime()
            }
        };
        _reservationRepository.CreateAsync(reservation);
        return Task.FromResult(new CreateReservationResponse
        {
            CreatedId = reservation.Id.ToString()
        });
    }
}
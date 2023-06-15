using Google.Protobuf.Collections;
using Grpc.Core;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Application.MessageBroker;
using JetSetGo.UsersManagement.Application.Common.Persistence;
using JetSetGo.UsersManagement.Domain.HostGrade.Entities;
using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Client.Accommodations;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class HostService
{
    private readonly IAccommodationClient _accommodationClient;
    private readonly IReservationClient _reservationClient;
    private readonly IEventBus _eventBus;
    private readonly IHostGradeRepository _gradeRepository;

    public HostService(
        IAccommodationClient accommodationClient, 
        IReservationClient reservationClient,
        IEventBus eventBus,
        IHostGradeRepository gradeRepository)
    {
        _accommodationClient = accommodationClient;
        _reservationClient = reservationClient;
        _eventBus = eventBus;
    }

    public async Task<bool> GetOutstandingHost(Guid id)
    {
        await Task.CompletedTask;
        var grades = await _gradeRepository.GetAllByHost(id);
        var averageGrade = CheckIfAverageGradeIsOutstanding(grades);
        var accommodations = _accommodationClient.GetAccommodationByHost(id);
        var reservations = new RepeatedField<GetReservationAccommodation>();
        foreach (var a in accommodations.Accommodations)
        {
            var r = _reservationClient.GetReservationsAccommodation(Guid.Parse(a.Id));
            reservations.AddRange(r.Reservations);
        }
        var fiveRes = CheckIfHostHadMoreThan5ReservationsInThePast(reservations);
        var reservationDaysOver50 = CheckIfReservationsLastMoreThan50Days(reservations);
        var cancelPercentage = CheckIfCancelPercentageIsUnder5(reservations);
        return fiveRes && reservationDaysOver50 && cancelPercentage && averageGrade;
    }

    private bool CheckIfAverageGradeIsOutstanding(List<HostGrade> grades)
    {
        var gradesCount = 0.0; 
        grades.ForEach(x =>
        {
            gradesCount += x.Number;
        });
        var averageGrade = gradesCount / grades.Count;
        return averageGrade > 4.7;
    }

    private bool CheckIfCancelPercentageIsUnder5(RepeatedField<GetReservationAccommodation> reservations)
    {
       var canceledRes =  reservations.Count(x => x.Deleted);
       var totalRes =  reservations.Count;
       var percent = 100 * canceledRes / totalRes;
       return percent < 5;
    }

    private bool CheckIfReservationsLastMoreThan50Days(RepeatedField<GetReservationAccommodation> reservations)
    {
        var daysCount = 0;
        foreach (var reservation in reservations)
        {
            if (reservation.Deleted == false)
            {
                var from = reservation.DateRange.From.ToDateTime();
                var to = reservation.DateRange.To.ToDateTime();
                daysCount += (to - from).Days;
            }
        }

        return daysCount > 50;
    }

    private bool CheckIfHostHadMoreThan5ReservationsInThePast(RepeatedField<GetReservationAccommodation> reservations)
    {
        var count = reservations.Count(x => x.DateRange.To.ToDateTime() < DateTime.Now);
        return count >= 5;
    }
}
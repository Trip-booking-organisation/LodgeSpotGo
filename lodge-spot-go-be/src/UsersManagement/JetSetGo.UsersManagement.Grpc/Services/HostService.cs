using Google.Protobuf.Collections;
using Grpc.Core;
using JetSetGo.UserManagement.Grpc;
using JetSetGo.UsersManagement.Grpc.Client;
using JetSetGo.UsersManagement.Grpc.Client.Accommodations;

namespace JetSetGo.UsersManagement.Grpc.Services;

public class HostService
{
    private readonly IAccommodationClient _accommodationClient;
    private readonly IReservationClient _reservationClient;

    public HostService(IAccommodationClient accommodationClient, IReservationClient reservationClient)
    {
        _accommodationClient = accommodationClient;
        _reservationClient = reservationClient;
    }

    public async Task<bool> GetOutstandingHost(Guid id)
    {
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
        return fiveRes && reservationDaysOver50 && cancelPercentage;
    }

    private bool CheckIfCancelPercentageIsUnder5(RepeatedField<GetReservationAccommodation> reservations)
    {
       var canceledRes =  reservations.Count(x => x.Deleted == true);
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
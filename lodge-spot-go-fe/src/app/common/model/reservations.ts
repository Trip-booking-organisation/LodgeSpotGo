import {Address} from "./addres";
import {SpecialPrice} from "./specialPrice";
import {DateRange} from "./dateRange";
import {ReservationStatus} from "./reservationStatus";

export interface IReservation {
  id?: string;
  accommodationId?:string;
  guestId?:string;
  dateRange?: DateRange;
  deleted?: boolean;
  reservationStatus?: ReservationStatus;
  numberOfGuests?: number;
}

import {DateRange} from "./dateRange";
import {ReservationStatus} from "./reservationStatus";

export interface IReservation {
  id?: string;
  accommodationId?:string;
  guestId?:string;
  dateRange?: DateRange;
  deleted?: boolean;
  status?: string;
  numberOfGuest?: number;
}

import {IReservation} from "./reservations";
import {Accommodation} from "./accommodation";


export interface IReservationAccommodation {
  reservation?: IReservation,
  accommodation?: Accommodation,
  disabled? : boolean
}

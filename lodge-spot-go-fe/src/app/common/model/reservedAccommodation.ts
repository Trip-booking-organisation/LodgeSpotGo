import {IReservation} from "./reservations";
import {Accommodation} from "./accommodation";
import {IAccommodationDto} from "./accommodation-dto";


export interface IReservationAccommodation {
  reservation?: IReservation,
  accommodation?: IAccommodationDto,
  disabled? : boolean,
  deleted?: number
}

import {Address} from "./addres";
import {Seat} from "./Seat";
import {AddressFlight} from "./AddressFlight";

export interface Flight{
  id: string,
  totalTicketPrize: number,
  seats: Seat[],
  departureAddress: AddressFlight,
  arrivalAddress: AddressFlight,
  departureTime: Date,
  departureDate: Date,
  arrivalTime: Date,
  arrivalDate: Date,
  companyName: string,
}

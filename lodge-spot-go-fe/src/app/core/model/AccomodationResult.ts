import { Address } from "./Address";
export interface MyDate{
  date?:string;
}
export interface AccomodationResult {
    country?: string,
    city: string,
    numberOfGuests?: number,
    startDate: MyDate,
    endDate: MyDate
  }

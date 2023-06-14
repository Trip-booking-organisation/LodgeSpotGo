import {Address} from "./addres";
import {SpecialPrice} from "./specialPrice";

export interface IAccommodationDto {
  id? : string
  name?: string;
  address?:Address;
  maxGuests?:number;
  minGuests?:number;
  amenities?:string[];
  photos?:string[];
  specialPrices?:SpecialPrice[];
  hostId?:string;
}

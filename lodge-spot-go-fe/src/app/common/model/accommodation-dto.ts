import {Address} from "./addres";
import {SpecialPrice} from "./specialPrice";

export interface IAccommodationDto {
  id? : string
  name?: string;
  address?:Address;
  location?:Address;
  max_guests?:number;
  min_guests?:number;
  amenities?:string[];
  photos?:string[];
  specialPrices?:SpecialPrice[]
}

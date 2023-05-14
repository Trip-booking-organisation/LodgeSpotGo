import {Address} from "./addres";
import {SpecialPrice} from "./specialPrice";
import {Photo} from "./Photo";

export interface Accommodation {
  name?: string;
  address?:Address;
  max_guests?:number;
  min_guests?:number;
  amenities?:string[];
  photos?:string[];
  specialPrices?:SpecialPrice[]
}

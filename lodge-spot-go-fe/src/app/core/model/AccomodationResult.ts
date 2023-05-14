import { Address } from "./Address";
export interface AccomodationResult {
    id: string,
    name: string,
    address: Address,
    maxGuests: number,
    minGuests: number,
    totalPrice: number,
    pricePerNight: number
  }
  
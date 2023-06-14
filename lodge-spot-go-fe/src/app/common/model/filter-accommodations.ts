import {IAccommodationDto} from "./accommodation-dto";

export interface FilterAccommodations{
  accommodations : IAccommodationDto[],
  amenities : string[],
  minGrade : number,
  maxGrade: number,
  outstandingHost : boolean
}

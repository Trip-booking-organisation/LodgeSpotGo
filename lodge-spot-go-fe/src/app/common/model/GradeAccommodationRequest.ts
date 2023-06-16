export interface GradeAccommodationRequest{
  accommodationId : string,
  guestId : string,
  number : number,
  guestEmail: string
}
export interface GradeHostRequst{
  accomodationId : string,
  guestId : string,
  number : number,
  hostId:string,
  guestEmail:string
}

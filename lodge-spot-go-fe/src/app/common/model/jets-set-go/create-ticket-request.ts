import {CreateTicketInfoRequest} from "./create-ticket-info-request";

export interface CreateTicketsRequest{
  token : string,
  flightId:string,
  passengerId : string,
  newTickets : CreateTicketInfoRequest[]
}

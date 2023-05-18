import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../../environments/environment.development";
import {UpdateReservationStatusRequest} from "../model/UpdateReservationStatusRequest";
import {ReservationCreate} from "../model/reservation.create";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private baseUrl =  environment.apiGateway;
  private reservationUrl = "https:localhost:7105/api/v1/reservations/"
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) {
  }
  public createReservation(reservation: ReservationCreate):Observable<any> {
    return this.httpClient.post(`${this.baseUrl}api/reservations`,reservation, {headers : this.headers});

  }
  public getByGuestId(id: string):Observable<any>{
    return this.httpClient.get(`${this.reservationUrl + id} ` , {headers : this.headers});
  }
  public cancelReservation(id: string):Observable<any>{
    return this.httpClient.delete(`${this.reservationUrl + id} ` , {headers : this.headers});
  }
  public getByAccommodationId(id: string):Observable<any>{
    return this.httpClient.get(`${this.reservationUrl + 'accommodation/' + id} ` , {headers : this.headers});
  }
  public updateReservationStatus(reservation: UpdateReservationStatusRequest):Observable<any>{
    return this.httpClient.put<UpdateReservationStatusRequest>(`${this.reservationUrl} ` , reservation,{headers : this.headers});
  }
  public getDeletedReservationsCount(guestId: string):Observable<any>{
    return this.httpClient.get(`${this.reservationUrl+'deleted/'+ guestId } ` ,{headers : this.headers});
  }
  public deleteReservation(id: string):Observable<any>{
    return this.httpClient.delete(`${this.reservationUrl +'delete/'+ id} ` , {headers : this.headers});
  }
}

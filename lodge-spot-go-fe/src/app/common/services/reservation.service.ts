import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../../environments/environment.development";
import {IReservation} from "../model/reservations";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private baseUrl =  environment.apiGateway;
  private reservationUrl = "https:localhost:7105/api/v1/reservations/"
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) {
  }

  public getByGuestId(id: string):Observable<any>{
    return this.httpClient.get(`${this.reservationUrl + id} ` , {headers : this.headers});
  }
  public cancelReservation(id: string):Observable<any>{
    return this.httpClient.delete(`${this.reservationUrl + id} ` , {headers : this.headers});
  }
}

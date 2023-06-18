import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {Flight} from "../model/flight";
import {SignInRequest} from "../model/jets-set-go/sign-in-request";
import {CreateTicketsRequest} from "../model/jets-set-go/create-ticket-request";
import {environment} from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class JetSetGoService {
  private baseUrl =  'https://localhost:7000/api/v1/flights';
/*
  private ticketUrl =  'https://localhost:7000/api/v1/tickets';
*/
  private authUrl = 'https://localhost:7000/authentication/signIn'
  private ticketUrl = `${environment.apiGateway}`;
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) { }
  public getFlights():Observable<Flight[]>{
    return this.httpClient.get<Flight[]>(`${this.baseUrl }` , {headers : this.headers});
  }
  public bookSeats(tickets:CreateTicketsRequest):Observable<any>{
    return this.httpClient.post<any>(`${this.ticketUrl}api/users/tickets`,tickets , {headers : this.headers});
  }
  signInUser(signInRequest: SignInRequest): Observable<any> {
    return this.httpClient.post<any>(this.authUrl, signInRequest, {headers: this.headers});
  }
}

import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {ReservationFilterRequest} from "../model/reservation-filter-request";
import {environment} from "../../../environments/environment.development";
import {FilterAccommodations} from "../model/filter-accommodations";

@Injectable({
  providedIn: 'root'
})
export class SearchAndFilterService {
  private baseUrl =  "https://localhost:7231/api/v1/search/";
  private url = environment.apiGateway;
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) { }
  public searchAccommodations(numberOfGuests: number,startDate: number,endDate: number,city: string, country: string):Observable<any>{
    return this.httpClient.get(`${this.baseUrl +numberOfGuests+'/' + startDate+'/' + endDate + '/' + city + '/' + country} ` ,
      {headers : this.headers});
  }
  public filterSearchedResults(request: FilterAccommodations):Observable<any>{
    return this.httpClient.post(`${this.url}api/filter`,request , {headers : this.headers});
  }
}

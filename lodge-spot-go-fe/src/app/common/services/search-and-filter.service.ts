import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SearchAndFilterService {
  private baseUrl =  "https://localhost:7231/api/v1/search/";
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) { }
  public searchAccommodations(numberOfGuests: number,startDate: number,endDate: number,city: string, country: string):Observable<any>{
    return this.httpClient.get(`${this.baseUrl +numberOfGuests+'/' + startDate+'/' + endDate + '/' + city + '/' + country} ` ,
      {headers : this.headers});
  }
}

import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Accommodation} from "../model/accommodation";

import {AccomodationResult} from "../../core/model/AccomodationResult";

import {environment} from "../../../environments/environment.development";
import {AccommodationResponse} from "../../shered/model/accommodationResponse";


@Injectable({
  providedIn: 'root'
})
export class AccommodationService{
  private baseUrl =  `${environment.apiGateway}`;
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) {
  }

  public createAccommodation(accommodation: Accommodation):Observable<any>{
    return this.httpClient.post<Accommodation>(`${this.baseUrl}/api/accommodations`,accommodation, {headers : this.headers});
  }

  public searchAccomodation(accomodationRequest: AccomodationResult):Observable<any> {
    console.log("https://localhost:7132/api/v1/search/" + accomodationRequest.numberOfGuests + "/" + accomodationRequest.startDate + "/" + accomodationRequest.endDate + "/" + accomodationRequest.city + "/" + accomodationRequest.country)
    return this.httpClient.post<AccomodationResult>("https://localhost:7132/api/v1/search", accomodationRequest, {headers: this.headers});

  }

  public getAllAccommodations():Observable<any> {
    return this.httpClient.get<AccommodationResponse[]>(this.baseUrl);
  }
  public getAccommodationById(id:string):Observable<any>{
    return this.httpClient.get(this.baseUrl + id,{headers : this.headers});
  }
  public getAccommodationByHost(id:string):Observable<any>{
    return this.httpClient.get(this.baseUrl +"host/"+ id,{headers : this.headers});

  }
}

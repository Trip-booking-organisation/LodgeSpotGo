import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Accommodation} from "../model/accommodation";
import {AccomodationResult} from "../../core/model/AccomodationResult";

@Injectable({
  providedIn: 'root'
})
export class AccommodationService{
  private baseUrl =  "https://localhost:7132/api/v1/accommodations";
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) {
  }

  public createAccommodation(accomodation:Accommodation):Observable<any>{
    return this.httpClient.post<Accommodation>(this.baseUrl,accomodation, {headers : this.headers});
  }
  public searchAccomodation(accomodationRequest: AccomodationResult):Observable<any>{
    console.log("https://localhost:7132/api/v1/search/" + accomodationRequest.numberOfGuests + "/" + accomodationRequest.startDate + "/" + accomodationRequest.endDate + "/" + accomodationRequest.city + "/" + accomodationRequest.country)
    return this.httpClient.post<AccomodationResult>("https://localhost:7132/api/v1/search",accomodationRequest, {headers : this.headers} );

  }
}

import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Accommodation} from "../model/accommodation";

@Injectable({
  providedIn: 'root'
})
export class AccommodationService{
  private baseUrl =  "https://localhost:7132/api/v1/accommodations/";
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) {
  }

  public createAccommodation(accomodation:Accommodation):Observable<any>{
    return this.httpClient.post<Accommodation>(this.baseUrl,accomodation, {headers : this.headers});
  }
  public getAccommodationById(id:string):Observable<any>{
    return this.httpClient.get(this.baseUrl + id,{headers : this.headers});
  }
  public getAccommodationByHost(id:string):Observable<any>{
    return this.httpClient.get(this.baseUrl +"host/"+ id,{headers : this.headers});
  }
}

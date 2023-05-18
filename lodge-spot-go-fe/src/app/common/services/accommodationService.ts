import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Accommodation} from "../model/accommodation";

import {environment} from "../../../environments/environment.development";
import {PriceData} from "./accommodation.price.model";


@Injectable({
  providedIn: 'root'
})
export class AccommodationService{
  private baseUrl =  `${environment.apiGateway}`;
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) {
  }

  public createAccommodation(accommodation: Accommodation):Observable<any>{
    return this.httpClient.post<Accommodation>(`${this.baseUrl}api/accommodations`,accommodation, {headers : this.headers});
  }
  public updatePrice(price: PriceData):Observable<any>{
    return this.httpClient.put<PriceData>(`${this.baseUrl}api/accommodations`,price, {headers : this.headers});
  }

  public getAllAccommodations():Observable<any> {
    return this.httpClient.get(`${this.baseUrl}api/accommodations`);
  }

  public getAccommodationById(id:string):Observable<any>{
    return this.httpClient.get(`${this.baseUrl}api/accommodations/${id}`,{headers : this.headers});
  }
  public getAccommodationByHost(id:string):Observable<any>{
    return this.httpClient.get(this.baseUrl +"api/v1/accommodations/host/"+ id,{headers : this.headers});
  }
}

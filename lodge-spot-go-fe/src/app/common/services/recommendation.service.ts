import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
private baseUrl = environment.apiGateway
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private httpClient: HttpClient) { }
  public getRecommendedAccommodations(email:string):Observable<any>{
    return this.httpClient.get(`${this.baseUrl }/api/recommendation/${email}` , {headers : this.headers});
  }
}

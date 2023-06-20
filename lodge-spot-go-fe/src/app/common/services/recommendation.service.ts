import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {GetRecommenedRequest} from "../../core/model/GetRecommenedRequest";

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
private baseUrl = environment.apiGateway
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private httpClient: HttpClient) { }
  public getRecommendedAccommodations(request:GetRecommenedRequest):Observable<any>{
    return this.httpClient.post("https://localhost:7132/api/v1/get-recommended" ,request, {headers : this.headers});
  }
}

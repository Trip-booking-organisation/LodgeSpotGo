import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Accommodation} from "../model/accommodation";
import {Observable} from "rxjs";
import {GradeAccommodationRequest} from "../model/GradeAccommodationRequest";

@Injectable({
  providedIn: 'root'
})
export class GradeAccommodationService {
  private baseUrl =  `${environment.apiGateway}`;
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) { }
  public gradeAccommodation(request: GradeAccommodationRequest):Observable<any>{
    return this.httpClient.post<any>(`${this.baseUrl}api/grades`,request, {headers : this.headers});
  }
}

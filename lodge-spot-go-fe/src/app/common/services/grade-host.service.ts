import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {GradeAccommodationRequest, GradeHostRequst} from "../model/GradeAccommodationRequest";

@Injectable({
  providedIn: 'root'
})
export class GradeHostService {
  private baseUrl =  `${environment.apiGateway}`;
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly httpClient:HttpClient) { }
  public gradeAccommodation(request: GradeHostRequst):Observable<any>{
    return this.httpClient.post<any>(`${this.baseUrl}api/users/gradeHost`,request, {headers : this.headers});
  }
}

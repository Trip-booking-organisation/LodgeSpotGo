import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Accommodation} from "../model/accommodation";
import {Observable} from "rxjs";
import {GradeAccommodationRequest} from "../model/GradeAccommodationRequest";
import {UpdateAccommodationGradeRequest} from "../model/update-accommodation-grade-request";

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
  public getGradesByGuest(guestId: string):Observable<any>{
    return this.httpClient.get(`${this.baseUrl}api/grades/guest/${guestId}`, {headers : this.headers});
  }
  public deleteGrade(id: string):Observable<any>{
    return this.httpClient.delete(`${this.baseUrl}api/grades/${id}`, {headers : this.headers});
  }
  public updateAccommodationGrade(request: UpdateAccommodationGradeRequest):Observable<any>{
    return this.httpClient.put<any>(`${this.baseUrl}api/grades`,request, {headers : this.headers});
  }
}

import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {ReservationCreate} from "../model/reservation.create";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {
  private baseUrl: string = `http://localhost:5228/api/v1/users`
  constructor(private readonly http:HttpClient) { }
  public deleteUser(userId:string, role:string):Observable<any> {
    const headers = new HttpHeaders({ 'accept': '*/*' });
    const params = new HttpParams()
      .set('userId', userId)
      .set('role', role);
    return this.http.delete(this.baseUrl, { headers, params });
  }

}

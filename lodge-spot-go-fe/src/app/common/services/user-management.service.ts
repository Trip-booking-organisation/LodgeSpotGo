import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment.development";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {ReservationCreate} from "../model/reservation.create";
import {Observable} from "rxjs";
import {KeycloakUser} from "../model/keycloakUser";
import {GetHostsGradesRequest} from "../model/getHostsGradesRequest";
import {HostGrade, HostGrades} from "../model/host-grade";

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {
  private baseUrl: string = `http://localhost:5228/api/v1/users`
  private baseHttpsUrl: string = `https://localhost:7169/api/v1/users`
  headers : HttpHeaders = new HttpHeaders({'Content-Type':'application/json'})
  constructor(private readonly http:HttpClient) { }
  public deleteUser(userId:string, role:string):Observable<any> {
    const headers = new HttpHeaders({ 'accept': '*/*' });
    const params = new HttpParams()
      .set('userId', userId)
      .set('role', role);
    return this.http.delete(this.baseUrl, { headers, params });
  }

  public getUser(id: string):Observable<KeycloakUser>{
    return this.http.get<KeycloakUser>(this.baseHttpsUrl+'/getUser/'+id , {headers : this.headers});
  }

  public getHostsGrades(request: GetHostsGradesRequest):Observable<HostGrades>{
    return this.http.post<HostGrades>(this.baseHttpsUrl+'/getGradesByHost/' ,request, {headers : this.headers});
  }

}

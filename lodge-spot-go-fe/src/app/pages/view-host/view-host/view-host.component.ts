import {AfterViewInit, Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UserManagementService} from "../../../common/services/user-management.service";
import {KeycloakUser} from "../../../common/model/keycloakUser";
import {HostGrade, HostGrades} from "../../../common/model/host-grade";
import {GetHostsGradesRequest} from "../../../common/model/getHostsGradesRequest";
import {Observable, switchMap} from "rxjs";

@Component({
  selector: 'app-view-host',
  templateUrl: './view-host.component.html',
  styleUrls: ['./view-host.component.scss']
})
export class ViewHostComponent implements OnInit,AfterViewInit{
  ngAfterViewInit(): void {

  }
  hostId: string | null;
  host:KeycloakUser
  hostGrades$: Observable<HostGrades>
  isOutstandingHost : Observable<any>

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.hostId = params['hostId'] || null;
      this.hostGrades$ = this.userService.getUser(params['hostId']).pipe(
        switchMap(value => {
          this.host = value
          return this.userService.getHostsGrades({
            hostId: value.id
          })
        }))
      this.isOutstandingHost = this.userService.getOutstanding(this.hostId)

    });


  }
  constructor(private  route:ActivatedRoute, private userService:UserManagementService) {
  }
}

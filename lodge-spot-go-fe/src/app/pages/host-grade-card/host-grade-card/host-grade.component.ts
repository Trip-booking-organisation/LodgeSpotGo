import {Component, Input, OnInit} from '@angular/core';
import {AccommodationGradeResponse} from "../../../common/model/accommodation-grade-response";
import {HostGrade} from "../../../common/model/host-grade";
import {KeycloakUser} from "../../../common/model/keycloakUser";
import {UserManagementService} from "../../../common/services/user-management.service";

@Component({
  selector: 'app-host-grade',
  templateUrl: './host-grade.component.html',
  styleUrls: ['./host-grade.component.scss']
})
export class HostGradeComponent implements OnInit{

  constructor(private userService:UserManagementService) {
  }
  ngOnInit(): void {
    this.userService.getUser(this.hostGrade.guestId).subscribe({
      next: res =>{
        this.guest = res
      }
    })
  }
  @Input() hostGrade : HostGrade
  guest:KeycloakUser
}

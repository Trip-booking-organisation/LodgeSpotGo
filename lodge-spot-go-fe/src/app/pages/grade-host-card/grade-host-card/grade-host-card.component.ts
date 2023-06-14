import {Component, Input, OnInit} from '@angular/core';
import {UserManagementService} from "../../../common/services/user-management.service";
import {HostGrade} from "../../../common/model/host-grade";
import {KeycloakUser} from "../../../common/model/keycloakUser";
import {Observable} from "rxjs";

@Component({
  selector: 'app-grade-host-card',
  templateUrl: './grade-host-card.component.html',
  styleUrls: ['./grade-host-card.component.scss']
})
export class GradeHostCardComponent implements OnInit{
  constructor(private userService:UserManagementService) {
  }
  ngOnInit(): void {
    this.guest$ = this.userService.getUser(this.hostGrade.guestId)
    this.userService.getUser(this.hostGrade.guestId).subscribe({
      next: res =>{
        this.guest = res
        console.log(this.hostGrade)

      }
    })
  }
  @Input() hostGrade : HostGrade
  guest:KeycloakUser
  guest$:Observable<KeycloakUser>

}
